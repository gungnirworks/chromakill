﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPlayer : ActionMechanics
{
    public int playerNumber = 0;

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
        if (hitstun.launch)
        {
        }
    }
}
