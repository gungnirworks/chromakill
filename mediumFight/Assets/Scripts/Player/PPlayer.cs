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

    public NegativeState NegState { get; set; }
    public Hitstop HStop { get; set; }

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
}
