using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMovement : PlayerInputsBase
{

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
