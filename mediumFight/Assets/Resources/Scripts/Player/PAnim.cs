using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PInputsBase;

[RequireComponent(typeof(PPlayer))]
public class PAnim : PlayerInputsBase
{
    // This script is responsible for running animations.
    // Instead of accessing the animator component directly, all scripts
    // needing to run animations should go through this component, in order
    // to prevent conflicts.

    public Animator animator;

    private void Update()
    {
        if (pMovement.Airborne && !pActions.InAction)
        {
            SetAnim("Falling", true);
        }
        else if (!pMovement.Airborne)
        {
            SetAnim("Falling", false);
        }
    }

    public void ResetAnim()
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            //Debug.Log("Param type: " + param.type.ToString());
            if (param.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(param.name, false);
            }
            else if (param.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(param.name);
            }
        }
    }

    public void SetAnim(string anim, bool state)
    {
        /*switch (anim)
        {
            case "Running":
                {
                    animator.SetBool(anim, state);
                }
                break;
        }*/
        animator.SetBool(anim, state);
    }
}
