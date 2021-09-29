using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputsBase : MonoBehaviour
{
    protected PPlayer player;
    protected PInput pInput;
    protected PMovement pMovement;
    protected PAttacks pAttacks;
    protected PAnim pAnim;

    protected void Start()
    {
        player = GetComponent<PPlayer>();

        pInput = player.pInput;
        pMovement = player.pMovement;
        pAttacks = player.pAttacks;
        pAnim = player.pAnim;

        DebugPrint(gameObject.ToString() + ": " + this.ToString() + " Start() has been called.");

        CustomStart();
    }

    protected virtual void CustomStart()
    {

    }

    private void Update()
    {
        if (player.negState == null)
        {
            //DebugPrint("No negative state.");
        }

        CustomUpdate();
    }

    protected virtual void CustomUpdate()
    {

    }

    protected virtual void DebugPrint(string message)
    {
        Debug.Log(message);  
    }
}