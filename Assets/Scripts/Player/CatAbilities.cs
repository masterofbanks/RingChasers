using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CatAbilities : MonoBehaviour
{
    [Header("Abilites")]
    public bool DoubleJump;
    public PlayerMovement PlayerMovementScript;


    //components
    public PlayerInputActions PIAs;

    private InputAction _doubleJump;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        PIAs = new PlayerInputActions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        
    }


}
