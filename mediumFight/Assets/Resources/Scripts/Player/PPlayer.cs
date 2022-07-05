using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActionMech;

[RequireComponent(typeof(PInput))]
[RequireComponent(typeof(PMovement))]
[RequireComponent(typeof(PActions))]
[RequireComponent(typeof(PAnim))]
[RequireComponent(typeof(Rigidbody))]
public class PPlayer : ActionMechanics
{
    // This is the core player class that consolidates all the scripts
    // required to make the player work.

    public int PlayerNumber { get; set; }

    [HideInInspector] public PInput pInput;
    [HideInInspector] public PMovement pMovement;
    [HideInInspector] public PActions pActions;
    [HideInInspector] public PAnim pAnim;

    public Rigidbody RB { get; set; }

    public NegativeState NegState { get; set; }
    public Hitstop HStop { get; set; }

    private void Awake()
    {
        pInput = GetComponent<PInput>();
        pMovement = GetComponent<PMovement>();
        pActions = GetComponent<PActions>();
        pAnim = GetComponent<PAnim>();
        RB = GetComponent<Rigidbody>();
    }

    public void ClearNegative()
    {
        NegState = null;
    }

    public bool CheckNegative()
    {
        // is there a negative state on the player?
        return NegState == null ? false : true;
    }

    public void ResetState()
    {
        NegState = null;
        pMovement.ResetLaunch();
        pAnim.ResetAnim();
        pActions.ResetActions();
        StopAllCoroutines();
    }

    public void NegativeInterrupt(NegativeState state)
    {
        ResetState();
        NegState = state;

        if (NegState is Hitstun)
        {
            Hitstun hitstun = NegState as Hitstun;
            GetHit(hitstun);
        }
    }

    void GetHit(Hitstun hitstun)
    {
        if (hitstun.Launch)
        {
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Stage" && collision.gameObject.GetComponent<StageCheck>() != null)
        {
            StageCheck sc = collision.gameObject.GetComponent<StageCheck>();

            if (sc.ground)  // if the stage is ground,
            {
                //Debug.Log("Player " + PlayerNumber.ToString() + " currently colliding with ground.");
                if (RB.velocity.y <= 0)
                {
                    // This is one of two methods of checking for ground collision.
                    // This method can cause issues if you touch a block tagged
                    // as ground on its side. Right now there's not much reason
                    // to use this method, but it's here.

                    // See PMovement for the raycast method.
                    // pMovement.Airborne = false;
                }
            }
            else
            {               // if the stage is not ground,
                
            }
        }
    }
}
