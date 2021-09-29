using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMovement : PlayerInputsBase
{
    // This script handles all player movement.
    // HOWEVER, this script should never directly poll for player inputs. This script should
    // only refer to PInput and PPlayer to determine inputs and player state and make the
    // proper movements as are relevant.

    // THIS GAME DOES NOT USE UNITY PHYSICS FOR MOVEMENT AND ATTACKS. All movement should
    // be deliberately designed to function the way I want in a controlled manner.
    // For that reason,
    // This script should handle all movement, including movement enacted upon the player
    // by the world or other entities. This means that the movement methods in this script
    // should hold all movement checks and corrections.

    public float moveMagnitude = 1f;

    private void FixedUpdate()
    {
        if (!player.CheckNegative() && !player.pAttacks.CheckAttacks())
        {
            Move();
        }
    }

    private void Move()
    {
        if (pInput.movement != Vector3.zero)
        {
            //Debug.Log("movement is: " + pInput.movement.ToString());
            gameObject.transform.position += pInput.movement * moveMagnitude;
            gameObject.transform.LookAt(transform.position + pInput.lastDirection, Vector3.up);
            pAnim.SetAnim("Running", true);
        }
        else
        {
            pAnim.SetAnim("Running", false);
        }
    }
}
