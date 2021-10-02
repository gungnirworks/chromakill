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

    public Player RewiredPlayer
    {
        get
        {
            return rewiredPlayer;
        }
        set { }
    }

    [HideInInspector] public Vector3 Movement { get; set; }
    [HideInInspector] public Vector3 LastDirection { get; set; }


    public InputBuffer inputBuffer;

    protected override void CustomStart()
    {
        inputBuffer = new InputBuffer(player.PlayerNumber);
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
                Debug.Log("PInput for player " + player.PlayerNumber.ToString() + " could not find the main camera.");
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
            if (ReInput.players.GetPlayer(player.PlayerNumber) != null)
            {
                rewiredPlayer = ReInput.players.GetPlayer(player.PlayerNumber);
                Debug.Log("Rewired player for " + player.PlayerNumber.ToString() + " has been fetched.");
            }
            else
            {
                Debug.Log("Could not find Rewired player for " + player.PlayerNumber.ToString() + " in ReInput.");
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
            Debug.Log("Because the main camera could not be found, camera-relative player movement for player " + player.PlayerNumber.ToString() + " cannot be calculated.");
            return;
        }

        // get flat normalized directions based on camera orientation
        Vector3 cameraForward = new Vector3(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z);
        Vector3 cameraRight = new Vector3(mainCamera.transform.right.x, 0, mainCamera.transform.right.z);
        // Convert axis input into movement based on directions relative to the main camera's direction
        if (!player.CheckNegative())
        {
            Movement = (rewiredPlayer.GetAxis("HMove") * cameraRight) + (rewiredPlayer.GetAxis("VMove") * cameraForward);
            Movement = Vector3.ClampMagnitude(Movement, 1);
            /*Debug.Log("HMove: " + rewiredPlayer.GetAxis("HMove").ToString());
            Debug.Log("VMove: " + rewiredPlayer.GetAxis("VMove").ToString());*/
        }
        else
        {
            Movement = Vector3.zero;
        }

        // Speed variables should be used in PMovement, not here.
        //movement = movement.normalized * SpeedVariables();
        //movement.y = 0f;

        if (Movement != Vector3.zero)
        {
            LastDirection = Movement;
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

        for (int i = 0; i < inputBuffer.Elements.Count; i++)
        {
            if (checkType != 0)
            {
                // if we're not looking for a press, the check is simple
                if (inputBuffer.Elements[i].ButtonPress == button &&
                    inputBuffer.Elements[i].CheckType == checkType)
                {
                    inputFound = true;
                }
            }
            else
            {
                // if we're looking for a press, we need to also check if easy input was used
                if (inputBuffer.Elements[i].CheckType == 0 || inputBuffer.Elements[i].Easy)
                {
                    inputFound = true;
                }
            }

            if (inputFound) // if true
            {
                ReportButtonPress(inputBuffer.Elements[i]);
                inputBuffer.Elements[i].CheckType = 3; // change the button type to be logged
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
        if (inputBuffer == null) inputBuffer = new InputBuffer(player.PlayerNumber);

        // get player inputs and add them to the buffer

        // ========================================[[ JUMP ]]

        if (rewiredPlayer.GetButtonDown("Jump")) // press
        {
            Debug.Log("Jumping");
            inputBuffer.Add(new BufferElement(0, 0));
        }

        if (rewiredPlayer.GetButton("Jump")) // hold
        {
            inputBuffer.Add(new BufferElement(0, 1));
        }

        if (rewiredPlayer.GetButtonUp("Jump")) // release
        {
            inputBuffer.Add(new BufferElement(0, 2));
        }

        // ========================================[[ QAtk ]]

        if (rewiredPlayer.GetButtonDown("QAtk")) // press
        {
            inputBuffer.Add(new BufferElement(1, 0));
        }

        if (rewiredPlayer.GetButton("QAtk")) // hold
        {
            inputBuffer.Add(new BufferElement(1, 1));
        }

        if (rewiredPlayer.GetButtonUp("QAtk")) // release
        {
            inputBuffer.Add(new BufferElement(1, 2));
        }

        // ========================================[[ SAtk ]]

        if (rewiredPlayer.GetButtonDown("SAtk")) // press
        {
            inputBuffer.Add(new BufferElement(2, 0));
        }

        if (rewiredPlayer.GetButton("SAtk")) // hold
        {
            inputBuffer.Add(new BufferElement(2, 1));
        }

        if (rewiredPlayer.GetButtonUp("SAtk")) // release
        {
            inputBuffer.Add(new BufferElement(2, 2));
        }

        // ========================================[[ Stance ]]

        if (rewiredPlayer.GetButtonDown("Stance")) // press
        {
            inputBuffer.Add(new BufferElement(3, 0));
        }

        if (rewiredPlayer.GetButton("Stance")) // hold
        {
            inputBuffer.Add(new BufferElement(3, 1));
        }

        if (rewiredPlayer.GetButtonUp("Stance")) // release
        {
            inputBuffer.Add(new BufferElement(3, 2));
        }

        // ========================================[[ Dash ]]

        if (rewiredPlayer.GetButtonDown("Dash")) // press
        {
            inputBuffer.Add(new BufferElement(4, 0));
        }

        if (rewiredPlayer.GetButton("Dash")) // hold
        {
            inputBuffer.Add(new BufferElement(4, 1));
        }

        if (rewiredPlayer.GetButtonUp("Dash")) // release
        {
            inputBuffer.Add(new BufferElement(4, 2));
        }

        // ========================================[[ Grab ]]

        if (rewiredPlayer.GetButtonDown("Grab")) // press
        {
            inputBuffer.Add(new BufferElement(5, 0));
        }

        if (rewiredPlayer.GetButton("Grab")) // hold
        {
            inputBuffer.Add(new BufferElement(5, 1));
        }

        if (rewiredPlayer.GetButtonUp("Grab")) // release
        {
            inputBuffer.Add(new BufferElement(5, 2));
        }

        //Debug.Log("Input buffer length: " + inputBuffer.Elements.Count.ToString());
    }
}
