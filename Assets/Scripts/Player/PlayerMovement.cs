using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement Values")]
    public float MovementSpeed;
    public float JumpSpeed;


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
        _move = Inputs.Player.Move;
        _move.Enable();
    }


    private void OnDisable()
    {
        _move.Disable();
    }
}
