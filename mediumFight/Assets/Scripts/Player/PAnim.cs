using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PAnim : MonoBehaviour
{
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
