using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalValues : MonoBehaviour
{
    // This script should only hold static values and
    // should only be attached to a prefab that is
    // referred to but should NEVER BE PLACED into
    // a scene.
    //
    // The purpose of this is to expose all relevant
    // values in-editor to allow easy tweaking of
    // values related to universal mechanics.

    [Header("Inputs")]
    public int bufferWindow = 8; // how long an input stays in the buffer before it's acted on
    public int easyInput = 5;   // how long you can you hold a button for it to be added to the buffer window at the
                                // earliest possible timing
    public int pressDuration = 5; // how long a button press stays in the window

    // Between these two, the value of (bufferWindow + easyInput) should give players a pretty wide margin
    // of input leniency.
    //
    // REMEMBER that the timing in this game is based on fixedupdate cycles.

    [Header("Attack Levels")]
    public int[] blockStun;
    public int[] hitStun;

    [Header("Attack Pushback")]
    public float defaultBlockPush = 1;
    public float defaultHitPush = 1;

    [Header("Aerial")]
    public int minimumLaunchTime = 10; // This value is number of FixedUpdate steps.
}
