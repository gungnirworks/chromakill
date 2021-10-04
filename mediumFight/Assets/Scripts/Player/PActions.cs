using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using PInputsBase;

[RequireComponent(typeof(PPlayer))]
public class PActions : PlayerInputsBase
{
    // This script holds the main logic thread for how all player movement and actions are
    // checked and handled.
    //
    // This script handles not only all player attacks, but also player actions that
    // aren't strictly attacks but are other forms of actions. This includes, for example,
    //                      << JUMPING AND DASHING >>
    //
    // This script should never directly poll for player inputs. This script should
    // only refer to PInput and PPlayer to determine inputs and player state and make the
    // proper attacks as are relevant.

    public bool InAction { get; set; } = false;

    private void FixedUpdate()
    {
        if (player.HStop != null)
        {
            // do hitstop stuff

            return;
        }

        if (player.CheckNegative()) return;
        // If there is no NegativeState continue the thread.

        if (InAction) return;
        // if the player is not currently in an action, continue the thread

        pMovement.Move();
        ButtonChecks();
    }

    public void ResetActions()
    {
        StopAllCoroutines();
    }

    private void ButtonChecks()
    {
        // check types: 0 == press
        //              1 == hold
        //              2 == release
        //              3 == logged

        if (!pMovement.Airborne)    // grounded button checks
        {
            //Check Jump:
            if (pInput.CheckInputBuffer(0, 0))
            {
                // do a jump
                Debug.Log("PActions is attempting to initiate jump.");
                pAnim.ResetAnim();
                pMovement.StartLaunch(true);
            }
        }
        else                        // airborne button checks
        {

        }
    }
}
