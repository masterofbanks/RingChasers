using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundInteraction : MonoBehaviour
{
    public PlayerMovement PlayerMovementScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            PlayerMovementScript.Grounded = true;
            PlayerMovementScript._doubleJump = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain")){
            PlayerMovementScript.Grounded = false;
        }
    }

}
