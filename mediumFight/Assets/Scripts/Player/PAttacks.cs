using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PAttacks : PlayerInputsBase
{
    // This script handles not only all player attacks, but also player actions that
    // aren't strictly attacks but are other forms of actions. This includes, for example,
    //                      << JUMPING AND DODGING >>
    // In that sense, "Attacks" in this context simply means "Actions."
    // No, I'm not renaming all the associated scripts to reflect that.
    // I'm just writing this here instead.

    // This script should never directly poll for player inputs. This script should
    // only refer to PInput and PPlayer to determine inputs and player state and make the
    // proper attacks as are relevant.

    public bool CheckAttacks()
    {
        return false;
    }
}
