using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement Values")]
    public float MaxMovementSpeed;
    public float HorizontalMovementForce;
    public float JumpSpeed;

    [Header("Ground Check Values")]
    public Transform GroundCheckPosition;
    public float GroundCheckRadius;
    public LayerMask GroundMask;

    [Header("Testing")]
    public bool Grounded;
    //true to use WASD; false to Use Arrows
    public bool Cat; 

    //componenets
    public PlayerInputActions Inputs;
    private Rigidbody2D _rb;
    private Vector2 _directionalInput;

    //input actions
    private InputAction _move;
    private InputAction _jump;

    //abilites
    public bool _doubleJump;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
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


    }


    private void MovePlayer()
    {
        Vector2 rawInput = _move.ReadValue<Vector2>();
        _directionalInput = new Vector2(System.MathF.Sign(rawInput.x), System.MathF.Sign(rawInput.y));
        if(Mathf.Abs(_rb.velocity.x) < MaxMovementSpeed)
        {
            _rb.AddForce(new Vector2(_directionalInput.x * HorizontalMovementForce, 0));
        }
    }

    private void TestGrounded()
    {
        Grounded = Physics2D.OverlapCircle(GroundCheckPosition.position, GroundCheckRadius, GroundMask);
        
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if(Grounded)
            _rb.velocity = new Vector2(_rb.velocity.x, JumpSpeed);
        else if(Cat && _doubleJump)
        {
            _doubleJump = false;
            _rb.velocity = new Vector2(_rb.velocity.x, JumpSpeed);
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
}
