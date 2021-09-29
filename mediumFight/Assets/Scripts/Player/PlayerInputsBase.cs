using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputsBase : MonoBehaviour
{
    // This is the base class from which all the individual
    // player component scripts will be derived.

    // This script pulls all the relevant information and
    // variables from the core player script automatically,
    // standardizing the lanugage that I use within these scripts.

    protected PPlayer player;
    protected PInput pInput;
    protected PMovement pMovement;
    protected PAttacks pAttacks;
    protected PAnim pAnim;

    protected void Start()
    {
        // Fetch all relevant pointers
        player = GetComponent<PPlayer>();

        pInput = player.pInput;
        pMovement = player.pMovement;
        pAttacks = player.pAttacks;
        pAnim = player.pAnim;

        Debug.Log(gameObject.ToString() + ": " + this.ToString() + " Start() has been called.");

        // Run custom start code, if any
        CustomStart();
    }

    protected virtual void CustomStart()
    {
        // Custom start code for the children if they need it
    }

    private void Update()
    {
        // This is for testing only.
        if (player.negState == null)
        {
            //Debug.Log("No negative state.");
        }

        // Run custom update code, if any
        CustomUpdate();
    }

    protected virtual void CustomUpdate()
    {
        // Custom update code for the children if they need it
    }

    // IDK why I wrote this method when I can just use Debug.Log()
    /*protected virtual void DebugPrint(string message)
    {
        Debug.Log(message);  
    }*/
}