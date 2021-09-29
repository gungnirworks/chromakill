using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PInput : PlayerInputsBase
{
    protected Camera mainCamera;
    protected Player rewiredPlayer;

    [HideInInspector] public Vector3 movement;
    [HideInInspector] public Vector3 lastDirection;

    protected override void CustomStart()
    {
        SetMainCamera();

    }

    protected void SetMainCamera()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    protected override void CustomUpdate()
    {
        //Debug.Log("PInput custom update has been called.");

        CheckRewiredPlayer();
        SetMainCamera();
        GetStickMovement();

    }

    protected void CheckRewiredPlayer()
    {
        if (rewiredPlayer == null)
        {
            //Debug.Log("Rewired player for " + player.playerNumber.ToString() + " is null.");
            rewiredPlayer = ReInput.players.GetPlayer(player.playerNumber);
        }
        else
        {
            //Debug.Log("Rewired player for " + player.playerNumber.ToString() + " is: " + rewiredPlayer.id.ToString());
        }
    }

    public void GetStickMovement()
    {
        // get flat normalized directions based on camera orientation
        Vector3 cameraForward = new Vector3(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z);
        Vector3 cameraRight = new Vector3(mainCamera.transform.right.x, 0, mainCamera.transform.right.z);
        // Convert axis input into movement based on directions relative to the main camera's direction
        if (!player.CheckNegative())
        {
            movement = (rewiredPlayer.GetAxis("HMove") * cameraRight) + (rewiredPlayer.GetAxis("VMove") * cameraForward);

            /*Debug.Log("HMove: " + rewiredPlayer.GetAxis("HMove").ToString());
            Debug.Log("VMove: " + rewiredPlayer.GetAxis("VMove").ToString());*/
        }
        else
        {
            movement = Vector3.zero;
        }

        //movement = movement.normalized * SpeedVariables();
        //movement.y = 0f;

        if (movement != Vector3.zero)
        {
            lastDirection = movement;
        }
        
    }
}
