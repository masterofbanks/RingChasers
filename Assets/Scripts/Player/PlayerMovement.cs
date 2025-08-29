using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement Values")]
    public float NormMaxMovementSpeed;
    public float HorizontalMovementForce;
    public float JumpSpeed;
    public float MinSpeedToEnterIdle;
    public enum State
    {
        idle, running, jumping, crouching, crouchWalking, squeezed, sprinting
    }
    public State state;
    [Header("Cat Stuff")]
    public bool _doubleJump;
    public float TunnelSpeed;
    public float DelayGravityBuffer;
    public float PushOutForce;

    [Header("Dog Stuff")]
    public float SprintSpeed;
    public float StartingDrag;
    public float SprintDrag;
    public float KickDogAmount;

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
    private float _normGravityScale;
    private float _linearDrag;
    private bool _facingRight;

    //input actions
    private InputAction _move;
    private InputAction _jump;

    //abilites
    
    private float _maxRunningSpeed;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
        _normGravityScale = _rb.gravityScale;
        _maxRunningSpeed = NormMaxMovementSpeed;
        _rb.drag = StartingDrag;
        _facingRight = true;
    }

    private void Awake()
    {
        Inputs = new PlayerInputActions();
    }

    // Update is called once per frame
    void Update()
    {
        Flip();
        MovePlayer();
        StateController();
        anime.SetInteger("state", (int)state);

    }


    private void MovePlayer()
    {
        Vector2 rawInput = _move.ReadValue<Vector2>();
        _directionalInput = new Vector2(System.MathF.Sign(rawInput.x), System.MathF.Sign(rawInput.y));
        if(Mathf.Abs(_rb.velocity.x) < _maxRunningSpeed && !InTunnel)
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

            else if(_directionalInput.y == -1)
            {
                if(Cat)
                    state = State.crouchWalking;
                else
                {
                    state = State.sprinting;
                    _maxRunningSpeed = SprintSpeed;
                    _rb.drag = SprintDrag;
                }
            }
            else
            {
                state = State.running;
                _maxRunningSpeed = NormMaxMovementSpeed;
                _rb.drag = StartingDrag;
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

    private void Flip()
    {
        if (_facingRight && System.Math.Sign(_directionalInput.x) == -1.0f)
        {
            transform.localScale = new Vector3(-1 * transform.localScale.x, 1, 1);
            _facingRight = !_facingRight;
        }

        else if (!_facingRight && System.Math.Sign(_directionalInput.x) == 1.0f)
        {
            transform.localScale = new Vector3(-1 * transform.localScale.x, 1, 1);
            _facingRight = !_facingRight;
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
            _rb.gravityScale = _normGravityScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Destroyable") && !Cat)
        {
            if(state == State.sprinting)
            {
                _rb.AddForce(new Vector2(-1 * transform.localScale.x, 2) * KickDogAmount, ForceMode2D.Impulse);
                Destroy(collision.gameObject);

            }



        }
    }

    private Vector2 ShortenXComponent(Vector2 vec, float k)
    {
        return new Vector2(k * vec.x, vec.y);
    }

    
}
