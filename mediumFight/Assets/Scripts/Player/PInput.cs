using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using PInputsBase;

public class PInput : PlayerInputsBase
{
    // This is the script used to handle all player input.
    // NO OTHER SCRIPT SHOULD BE DIRECTLY ACCEPTING PLAYER INPUTS.

    // For player action controls, all scripts should 
    // reference this script to see what inputs have occurred or
    // are currently occurring.

    protected Camera mainCamera;
    protected Player rewiredPlayer;

    [HideInInspector] public Vector3 movement;
    [HideInInspector] public Vector3 lastDirection;


    public InputBuffer inputBuffer;

    protected override void CustomStart()
    {
        inputBuffer = new InputBuffer(player.playerNumber);
    }

    protected bool SetMainCamera()
    {
        // This looks for the main camera and returns true if it could be found. If the main camera can't be found,
        // it returns false.

        if (mainCamera == null)
        {
            if (Camera.main != null)
            {
                mainCamera = Camera.main;
                return true;
            }
            else
            {
                Debug.Log("PInput for player " + player.playerNumber.ToString() + " could not find the main camera.");
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    protected void CheckRewiredPlayer()
    {
        // Rewired is the best multi-controller input asset on the Unity Asset Store and I'll use it
        // even for single-player games. It's one of the best assets ever released. Please use it, it's
        // so great.

        if (rewiredPlayer == null)
        {
            // Fetch the ReInput player from rewired
            if (ReInput.players.GetPlayer(player.playerNumber) != null)
            {
                rewiredPlayer = ReInput.players.GetPlayer(player.playerNumber);
                Debug.Log("Rewired player for " + player.playerNumber.ToString() + " has been fetched.");
            }
            else
            {
                Debug.Log("Could not find Rewired player for " + player.playerNumber.ToString() + " in ReInput.");
            }
        }
        else
        {
            //Debug.Log("Rewired player for " + player.playerNumber.ToString() + " is: " + rewiredPlayer.id.ToString());
        }
    }

    public void GetStickMovement(bool cam)
    {
        // Player movement directions are ALWAYS based on camera orientation.

        if (!cam)
        {
            Debug.Log("Because the main camera could not be found, camera-relative player movement for player " + player.playerNumber.ToString() + " cannot be calculated.");
            return;
        }

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

        // Speed variables should be used in PMovement, not here.
        //movement = movement.normalized * SpeedVariables();
        //movement.y = 0f;

        if (movement != Vector3.zero)
        {
            lastDirection = movement;
        }

    }

    //========================================[[         CYCLE          ]]===================================================
    protected override void CustomUpdate()
    {
        //Debug.Log("PInput custom update has been called.");

        CheckRewiredPlayer();
        GetStickMovement(SetMainCamera());
        UpdateInputBuffer();

    }

    private void FixedUpdate()
    {

        // do upkeep on the input buffer at THE END of fixedupdate.
        inputBuffer.BufferProgression();
    }

    //==================================================[[       INPUT HANDLING      ]]===================================================

    public bool CheckInputBuffer(int button, int checkType)
    {
        // initialize a temporary check bool
        bool inputFound = false;

        for (int i = 0; i < inputBuffer.elements.Count; i++)
        {
            if (checkType != 0)
            {
                // if we're not looking for a press, the check is simple
                if (inputBuffer.elements[i].buttonPress == button &&
                    inputBuffer.elements[i].checkType == checkType)
                {
                    inputFound = true;
                }
            }
            else
            {
                // if we're looking for a press, we need to also check if easy input was used
                if (inputBuffer.elements[i].checkType == 0 || inputBuffer.elements[i].easy)
                {
                    inputFound = true;
                }
            }

            if (inputFound) // if true
            {
                ReportButtonPress(inputBuffer.elements[i]);
                inputBuffer.elements.RemoveAt(i);
                return inputFound; // return true
            }
        }

        return inputFound; // return false
    }

    protected void ReportButtonPress(BufferElement button)
    {
        // report the button that was pressed. this will be useful for debug purposes
    }

    protected void UpdateInputBuffer()
    {
        if (inputBuffer == null) inputBuffer = new InputBuffer(player.playerNumber);

        // get player inputs and add them to the buffer

        if (rewiredPlayer.GetButtonDown("Jump"))
        {
            inputBuffer.elements.Add(new BufferElement(0,0));
        }
    }
}
