using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PAttacks : PlayerInputsBase
{
    // This script handles all player attacks.
    // HOWEVER, this script should never directly poll for player inputs. This script should
    // only refer to PInput and PPlayer to determine inputs and player state and make the
    // proper attacks as are relevant.

    public bool CheckAttacks()
    {
        return false;
    }
}
