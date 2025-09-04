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
            PlayerMovementScript.DoubleJump = true;
            PlayerMovementScript.DownwardBoost = true;
            PlayerMovementScript.Slamming = false;
        }

        else if (collision.gameObject.CompareTag("Destroyable"))
        {
            if (PlayerMovementScript.Slamming)
            {
                Destroy(collision.gameObject);
            }

            else
            {
                PlayerMovementScript.Grounded = true;
                PlayerMovementScript.DoubleJump = true;
                PlayerMovementScript.DownwardBoost = true;
            }
            Debug.Log($"Something destroyable is beneath {transform.parent.name}");
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain")){
            PlayerMovementScript.Grounded = false;
        }

        else if(collision.gameObject.CompareTag("Destroyable") && !PlayerMovementScript.Slamming)
        {
            PlayerMovementScript.Grounded = false;
        }
    }

    

}
