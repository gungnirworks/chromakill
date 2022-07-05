using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using PInputsBase;
using ActionMech;

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

    public Attack[] QAtkChain;
    public Attack[] SAtkChain;

    private void FixedAction()
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
    // ===================================================================[[ LISTENERS ]]========================================================

    #region Listeners

    #endregion

    // ===================================================================[[   CHECKS  ]]========================================================

    #region Checks
    public void ResetActions()
    {
        InAction = true;
        StopAllCoroutines();
    }

    private void DisplayDebugButtonCheck(string check)
    {
        //Debug.Log("PActions is attempting to initiate: " + check);
    }

    private void ButtonChecks()
    {
        // check types: 0 == press
        //              1 == hold
        //              2 == release
        //              3 == logged

        if (!pMovement.Airborne) // ======== grounded button checks
        {
            if (pInput.CheckInputBuffer(0, 0)) //Check Jump:
            {
                DisplayDebugButtonCheck("Jump");
                pAnim.ResetAnim();
                pMovement.StartLaunch(true);
            }
            else if (pInput.CheckInputBuffer(1, 0)) //Check QAtk:
            {
                DisplayDebugButtonCheck("QAtk");
                pAnim.ResetAnim();
            }
            else if (pInput.CheckInputBuffer(2, 0)) //Check SAtk
            {
                DisplayDebugButtonCheck("SAtk");
                //pAnim.ResetAnim();
            }
            else if (pInput.CheckInputBuffer(3, 0)) //Check Stance:
            {
                DisplayDebugButtonCheck("Stance");
                //pAnim.ResetAnim();
            }
            else if (pInput.CheckInputBuffer(4, 0)) //Check Dash:
            {
                DisplayDebugButtonCheck("Dash");
                //pAnim.ResetAnim();
            }
            else if (pInput.CheckInputBuffer(5, 0)) //Check Grab:
            {
                DisplayDebugButtonCheck("Grab");
                //pAnim.ResetAnim();
            }
        }

        // ================================= airborne button checks
        if (!pMovement.Airborne) return; // exit out if not airborne
    }
    #endregion

    // ===================================================================[[  ATTACKS  ]]========================================================

    #region Attacks
    public void StartChainAttack(Attack[] chain, int buttonPress, int chainStart = 0)
    {
        if (chainStart > chain.Length - 1)
        {
            Debug.Log("Attempted to start a chain out of range at index " + chainStart.ToString() + ". chainStart has been changed to 0.");
            chainStart = 0;
        }
    }

    protected IEnumerator AttackCoroutine(Attack[] attack, int buttonPress, int chainStart = 0)
    {
        yield return null;
    }
    #endregion
}
