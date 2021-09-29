using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PAnim : MonoBehaviour
{
    // This script is responsible for running animations.
    // Instead of accessing the animator component directly, all scripts
    // needing to run animations should go through this component, in order
    // to prevent conflicts.

    public Animator animator;

    public void ResetAnim()
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            animator.SetBool(param.name, false);
        }
    }

    public void SetAnim(string anim, bool state)
    {
        animator.SetBool(anim, state);
    }
}
