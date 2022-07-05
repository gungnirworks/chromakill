using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using PInputsBase;

[RequireComponent(typeof(PPlayer))]
public class PInput : PlayerInputsBase
{
    // This is the script used to handle all player input.
    // NO OTHER SCRIPT SHOULD BE DIRECTLY ACCEPTING PLAYER INPUTS.

    // For player action controls, all scripts should 
    // reference this script (via listening for an event) to see
    // what inputs have occurred or are currently occurring.

    #region Initializing

    protected Camera mainCamera;
    protected Player rewiredPlayer;

    public Player RewiredPlayer
    {
        get
        {
            return rewiredPlayer;
        }
    }
    public InputBuffer Buffer { get; private set; }
    #endregion

    #region Tracking
    [HideInInspector] public Vector3 Movement { get; set; }
    [HideInInspector] public Vector3 LastDirection { get; set; }
    public bool ActionControlOK
    {
        get
        {
            // This bool reports whether it's okay for the player to have control
            // of the character in action game mode, as opposed to in a menu or whatever.
            // This has nothing to do with the different player states that can determine
            // whether or not you currently have control of your character (for example,
            // hitstun, or if you're animation locked after you commit to an attack).
            if (GameController.Instance.CinematicOn ||
                MenuControl)
                return false;
            return true;
        }
    }
    public bool MenuControl { get; set; } = false; // a bool to determine if you're controlling a menu or not.
    protected bool gatheredInputDuringFrame = false;
    protected bool alreadySetListener = false;
    #endregion

    #region Basic Methods and Setup

    protected override void CustomStart()
    {
        Buffer = new InputBuffer(player.PlayerNumber);
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

    public void SetListeners()
    {
        if (alreadySetListener) return;

        if (ActionThreadSync.Instance == null)
        {
            Debug.Log("Action Thread could not be found.");
            return;
        }

        ActionThreadSync.Instance.onGetInput += InputLoop;
        alreadySetListener = true;
    }

    public void RemoveListeners()
    {
        if (ActionThreadSync.Instance == null)
        {
            Debug.Log("Action Thread could not be found.");
            return;
        }

        ActionThreadSync.Instance.onGetInput -= InputLoop;
        alreadySetListener = false;
    }
    #endregion

    //========================================[[         CYCLE          ]]===================================================

    #region Cycle
    private void FixedUpdate()
    {
        /// FixedUpdate happens first, so in the interest of the fastest possible
        /// input gathering, check for inputs here.
        //InputLoop();

        /// There should be an event here that player actions listen for in order
        /// to trigger actions, in order to enforce order of operations from
        /// gathering inputs (input loop) into performing actions.

        /// Do upkeep on the input buffer at THE END of fixedupdate. Because actions
        /// are only triggered during fixed update, inputs that would be taken "after"
        /// this point are too late to affect this current frame.
        //Buffer.BufferProgression();
    }

    protected override void CustomUpdate()
    {
        /// Check for inputs here, because in the case that your framerate is running
        /// more frequently than FixedUpdate, inputs between fixed updates will be
        /// caught and added to the input buffer.
        //InputLoop();
    }

    private void LateUpdate()
    {
        /// At the end of the frame, reset input gathering. I probably need to create
        /// a unique input timing tracker for this so I can reset this bool in the case
        /// that frame rate is lower than fixed update time.
        gatheredInputDuringFrame = false;
    }
    #endregion

    //======================================[[       INPUT HANDLING       ]]=================================================

    #region Input Handling
    protected virtual void InputLoop()
    {
        if (gatheredInputDuringFrame) return;

        CheckRewiredPlayer();
        GetStickMovement(SetMainCamera());
        UpdateInputBuffer();

        gatheredInputDuringFrame = true;
    }

    public bool CheckInputBuffer(int button, int checkType)
    {
        // initialize a temporary check bool
        bool inputFound = false;

        for (int i = 0; i < Buffer.Elements.Count; i++)
        {
            if (checkType != 0)
            {
                // if we're not looking for a press, the check is simple
                if (Buffer.Elements[i].ButtonPress == button &&
                    Buffer.Elements[i].CheckType == checkType)
                {
                    inputFound = true;
                }
            }
            else
            {
                // if we're looking for a press, we need to also check if easy input was used
                if (Buffer.Elements[i].ButtonPress == button && 
                    (Buffer.Elements[i].CheckType == 0 || Buffer.Elements[i].Easy))
                {
                    inputFound = true;
                }
            }

            if (inputFound) // if true
            {
                ReportButtonPress(Buffer.Elements[i]);
                return inputFound; // return true
            }
        }

        return inputFound; // return false
    }

    protected void ReportButtonPress(BufferElement button)
    {
        // report the button that was pressed. this will be useful for debug purposes
        //Debug.Log("CheckInputBuffer found button input: " + button.ButtonPress + " of type: " + button.CheckType);
    }

    protected void UpdateInputBuffer()
    {
        if (Buffer == null) Buffer = new InputBuffer(player.PlayerNumber);

        // get player inputs and add them to the buffer

        // ========================================[[ JUMP ]]

        if (rewiredPlayer.GetButtonDown("Jump")) // press
        {
            //Debug.Log("Jumping");
            Buffer.Add(new BufferElement(0, 0));
        }

        if (rewiredPlayer.GetButton("Jump")) // hold
        {
            Buffer.Add(new BufferElement(0, 1));
        }

        if (rewiredPlayer.GetButtonUp("Jump")) // release
        {
            Buffer.Add(new BufferElement(0, 2));
        }

        // ========================================[[ QAtk ]]

        if (rewiredPlayer.GetButtonDown("QAtk")) // press
        {
            Buffer.Add(new BufferElement(1, 0));
        }

        if (rewiredPlayer.GetButton("QAtk")) // hold
        {
            Buffer.Add(new BufferElement(1, 1));
        }

        if (rewiredPlayer.GetButtonUp("QAtk")) // release
        {
            Buffer.Add(new BufferElement(1, 2));
        }

        // ========================================[[ SAtk ]]

        if (rewiredPlayer.GetButtonDown("SAtk")) // press
        {
            Buffer.Add(new BufferElement(2, 0));
        }

        if (rewiredPlayer.GetButton("SAtk")) // hold
        {
            Buffer.Add(new BufferElement(2, 1));
        }

        if (rewiredPlayer.GetButtonUp("SAtk")) // release
        {
            Buffer.Add(new BufferElement(2, 2));
        }

        // ========================================[[ Stance ]]

        if (rewiredPlayer.GetButtonDown("Stance")) // press
        {
            Buffer.Add(new BufferElement(3, 0));
        }

        if (rewiredPlayer.GetButton("Stance")) // hold
        {
            Buffer.Add(new BufferElement(3, 1));
        }

        if (rewiredPlayer.GetButtonUp("Stance")) // release
        {
            Buffer.Add(new BufferElement(3, 2));
        }

        // ========================================[[ Dash ]]

        if (rewiredPlayer.GetButtonDown("Dash")) // press
        {
            Buffer.Add(new BufferElement(4, 0));
        }

        if (rewiredPlayer.GetButton("Dash")) // hold
        {
            Buffer.Add(new BufferElement(4, 1));
        }

        if (rewiredPlayer.GetButtonUp("Dash")) // release
        {
            Buffer.Add(new BufferElement(4, 2));
        }

        // ========================================[[ Grab ]]

        if (rewiredPlayer.GetButtonDown("Grab")) // press
        {
            Buffer.Add(new BufferElement(5, 0));
        }

        if (rewiredPlayer.GetButton("Grab")) // hold
        {
            Buffer.Add(new BufferElement(5, 1));
        }

        if (rewiredPlayer.GetButtonUp("Grab")) // release
        {
            Buffer.Add(new BufferElement(5, 2));
        }

        //Debug.Log("Input buffer length: " + inputBuffer.Elements.Count.ToString());
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
    #endregion
}
