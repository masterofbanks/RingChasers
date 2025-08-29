using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement Values")]
    public float MaxMovementSpeed;
    public float HorizontalMovementForce;
    public float JumpSpeed;
    public float MinSpeedToEnterIdle;
    public enum State
    {
        idle, running, jumping, crouching, crouchWalking, squeezed
    }
    public State state;
    [Header("Tunnel Stuff")]
    public float TunnelSpeed;
    public float DelayGravityBuffer;
    public float PushOutForce;

    [Header("Testing")]
    public bool Grounded;
    //true to use WASD; false to Use Arrows
    public bool Cat;
    public bool InTunnel;

    //componenets
    public PlayerInputActions Inputs;
    private Rigidbody2D _rb;
    private Animator anime;
    private Vector2 _directionalInput;
    private float NormGravityScale;

    //input actions
    private InputAction _move;
    private InputAction _jump;

    //abilites
    public bool _doubleJump;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
        NormGravityScale = _rb.gravityScale;
    }

    private void Awake()
    {
        Inputs = new PlayerInputActions();
    }

    // Update is called once per frame
    void Update()
    {
        //TestGrounded();
        MovePlayer();
        StateController();
        anime.SetInteger("state", (int)state);

    }


    private void MovePlayer()
    {
        Vector2 rawInput = _move.ReadValue<Vector2>();
        _directionalInput = new Vector2(System.MathF.Sign(rawInput.x), System.MathF.Sign(rawInput.y));
        if(Mathf.Abs(_rb.velocity.x) < MaxMovementSpeed && !InTunnel)
        {
            _rb.AddForce(new Vector2(_directionalInput.x * HorizontalMovementForce, 0));
        }

        else if (InTunnel)
        {
            _rb.velocity = _directionalInput * TunnelSpeed;
        }
    }

    private void StateController()
    {
        if (InTunnel)
        {
            state = State.squeezed;
        }
        
        
        else if (Grounded)
        {
            if(Mathf.Abs(_rb.velocity.x) < MinSpeedToEnterIdle)
            {
                if(_directionalInput.y == -1 && Cat)
                {
                    state = State.crouching;
                }

                else
                {
                    state = State.idle;
                }

            }

            else if(_directionalInput.y == -1 && Cat)
            {
                state = State.crouchWalking;
            }
            else
            {
                state = State.running;
            }
        }


        else
        {
            state = State.jumping;
        }
    }
    


    private void Jump(InputAction.CallbackContext context)
    {
        if (!InTunnel)
        {
            if (Grounded && !InTunnel)
                _rb.velocity = new Vector2(_rb.velocity.x, JumpSpeed);
            else if (Cat && _doubleJump)
            {
                _doubleJump = false;
                _rb.velocity = new Vector2(_rb.velocity.x, JumpSpeed);
            }
        }
        
    }

    private void OnEnable()
    {
        if (Cat)
        {
            _move = Inputs.Player.WASDMove;
            _jump = Inputs.Player.JumpWASD;

        }

        else
        {
            _move = Inputs.Player.ArrowMove;
            _jump = Inputs.Player.JumpArrows;
        }
        _move.Enable();
        _jump.Enable();
        _jump.performed += Jump;
    }


    private void OnDisable()
    {
        _move.Disable();
        _jump.Disable();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tunnel"))
        {
            InTunnel = true;
            _rb.gravityScale = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tunnel"))
        {
            InTunnel = false;
            _rb.velocity = ShortenXComponent(_directionalInput * PushOutForce, 0.5f);
            _rb.gravityScale = NormGravityScale;
        }
    }

    private Vector2 ShortenXComponent(Vector2 vec, float k)
    {
        return new Vector2(k * vec.x, vec.y);
    }

    
}
