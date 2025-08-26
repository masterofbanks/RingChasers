using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement Values")]
    public float MovementSpeed;
    public float JumpSpeed;

    [Header("Ground Check Values")]
    public Transform GroundCheckPosition;
    public float GroundCheckRadius;
    public LayerMask GroundMask;

    [Header("Testing")]
    public bool Grounded;
    //true to use WASD; false to Use Arrows
    public bool WASD; 

    //componenets
    public PlayerInputActions Inputs;
    private Rigidbody2D _rb;
    private Vector2 _directionalInput;

    //input actions
    private InputAction _move;
    
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
        Grounded = Physics2D.OverlapCircle(GroundCheckPosition.position, GroundCheckRadius, GroundMask);
        MovePlayer();


    }


    private void MovePlayer()
    {
        Vector2 rawInput = _move.ReadValue<Vector2>();
        _directionalInput = new Vector2(System.MathF.Sign(rawInput.x), System.MathF.Sign(rawInput.y));
        _rb.velocity = new Vector2(_directionalInput.x * MovementSpeed, _rb.velocity.y);
    }

    private void OnEnable()
    {
        if (WASD)
            _move = Inputs.Player.WASDMove;
        else
            _move = Inputs.Player.ArrowMove;    
        _move.Enable();
    }


    private void OnDisable()
    {
        _move.Disable();
    }
}
