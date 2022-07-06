using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionThreadSync : MonoBehaviour
{
    #region Initialization and Start
    public static ActionThreadSync Instance { get; private set; }

    public List<PInputsBase.InputBuffer> inputBuffers = new List<PInputsBase.InputBuffer>();

    void Start()
    {
        Instance = this;
    }
    #endregion

    #region Tracking
    public int FixedFrameCounter { get; private set; }
    private void UpdateFixedFrameCounter()
    {
        FixedFrameCounter++;
        if (FixedFrameCounter >= 5)
        {
            FixedFrameCounter = 0;
        }
    }
    #endregion

    //========================================[[         EVENTS         ]]===================================================

    #region Events
    public event Action onGetInput;
    public void GetInput()
    {
        if (onGetInput == null) return;

        onGetInput.Invoke();
    }

    public event Action onFixedAction;
    public void FixedAction()
    {
        if (onFixedAction == null) return;

        onFixedAction.Invoke();
    }
    #endregion

    //========================================[[         CYCLE          ]]===================================================

    #region Cycle
    private void FixedUpdate()
    {
        UpdateFixedFrameCounter();
        SetListeners();

        /// FixedUpdate happens first, so in the interest of the fastest possible
        /// input gathering, check for inputs here.
        /// ... at least, that WOULD be the case, except for the fact that if your
        /// framerate is lower than the fixed update rate, rewired's button down
        /// event can last longer than two or more fixed update cyclces. As a result,
        /// I have commented this out and will rely only on Update() for input
        /// fetching from now on.
        //GetInput();

        /// There should be an event here that player actions listen for in order
        /// to trigger actions, in order to enforce order of operations from
        /// gathering inputs (input loop) into performing actions.
        FixedAction();

        /// Do upkeep on the input buffer at THE END of fixedupdate. Because actions
        /// are only triggered during fixed update, inputs that would be taken "after"
        /// this point are too late to affect this current frame.
        BufferProgression();
    }

    private void Update()
    {
        /// Check for inputs here, because in the case that your framerate is running
        /// more frequently than FixedUpdate, inputs between fixed updates will be
        /// caught and added to the input buffer.
        GetInput();
    }

    private void LateUpdate()
    {
        /// On each PInput individually, gathered input should reset.
    }
    #endregion

    //========================================[[      MAIN METHODS      ]]===================================================
    #region Main Methods
    public bool NullChecks()
    {
        if (GameController.Instance == null) return true;

        if (GameController.Instance.Players.Count < 1)
            GameController.Instance.FindAllPlayers();

        if (GameController.Instance.Players.Count < 1)
            return true;

        return false;
    }

    private void SetListeners()
    {
        foreach (GameObject obj in GameController.Instance.Players)
        {
            obj.GetComponentInChildren<PInput>().SetListeners();
            obj.GetComponentInChildren<PActions>().SetListeners();
        }
    }

    private bool AssignBuffers()
    {
        // if already finished,
        if (inputBuffers.Count > 0) return true;

        // if checks are null,
        if (NullChecks()) return false;

        foreach (GameObject obj in GameController.Instance.Players)
        {
            if (obj.GetComponentInChildren<PInput>() != null)
            {
                if (!inputBuffers.Contains(obj.GetComponentInChildren<PInput>().Buffer))
                {
                    inputBuffers.Add(obj.GetComponentInChildren<PInput>().Buffer);
                }
            }
        }
        return true;
    }

    private void BufferProgression()
    {
        if (!AssignBuffers()) return; // return if checks null

        foreach (PInputsBase.InputBuffer buffer in inputBuffers)
        {
            buffer.BufferProgression();
        }
    }
    #endregion
}
