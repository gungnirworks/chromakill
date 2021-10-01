using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActionMech;

public class PPlayer : ActionMechanics
{
    // This is the core player class that consolidates all the scripts
    // required to make the player work.

    public int PlayerNumber { get; set; }

    public PInput pInput;
    public PMovement pMovement;
    public PAttacks pAttacks;
    public PAnim pAnim;

    public NegativeState negState;
    public Hitstop hitstop = null;

    public void ClearNegative()
    {
        negState = null;
    }

    public bool CheckNegative()
    {
        // is there a negative state on the player?
        return negState == null ? false : true;
    }

    public void ResetState()
    {
        negState = null;
        StopAllCoroutines();
    }

    public void NegativeInterrupt(NegativeState state)
    {
        ResetState();
        negState = state;

        if (negState is Hitstun)
        {
            Hitstun hitstun = negState as Hitstun;
            GetHit(hitstun);
        }
    }

    void GetHit(Hitstun hitstun)
    {
        if (hitstun.Launch)
        {
        }
    }
}
