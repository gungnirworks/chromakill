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
    protected UniversalMechanics uMech;

    protected void Start()
    {
        Debug.Log(gameObject.ToString() + ": " + this.ToString() + " Start() has been called.");

        // Fetch all relevant pointers
        if (!FetchScripts())
        {
            Debug.Log(gameObject.ToString() + ": " + this.ToString() + " could not be fully initialized due to missing components.");
        }

        // Run custom start code, if any
        CustomStart();
    }

    protected bool FetchScripts()
    {
        if (GetComponent<PPlayer>() == null)
        {
            Debug.Log(gameObject.ToString() + ": " + this.ToString() + " could not find PPlayer component.");
            return false;
        }
        player = GetComponent<PPlayer>();

        if (player.pInput == null)
        {
            Debug.Log(gameObject.ToString() + ": " + this.ToString() + " could not find PInput component.");
            return false;
        }
        pInput = player.pInput;


        if (player.pMovement == null)
        {
            Debug.Log(gameObject.ToString() + ": " + this.ToString() + " could not find PMovement component.");
            return false;
        }
        pMovement = player.pMovement;

        if (player.pAttacks == null)
        {
            Debug.Log(gameObject.ToString() + ": " + this.ToString() + " could not find PAttacks component.");
            return false;
        }
        pAttacks = player.pAttacks;

        if (player.pAnim == null)
        {
            Debug.Log(gameObject.ToString() + ": " + this.ToString() + " could not find PAnim component.");
            return false;
        }
        pAnim = player.pAnim;

        if (UniversalMechanics.instance == null)
        {
            Debug.Log(gameObject.ToString() + ": " + this.ToString() + " could not find UniversalMechanics instance.");
            return false;
        }
        uMech = UniversalMechanics.instance;

        return true;
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