using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalValues : MonoBehaviour
{
    // This script should only hold static values and
    // should only be attached to a prefab that is
    // referred to but should never be placed into
    // a scene on its own.

    [Header("Attack Levels")]
    public int[] blockStun;
    public int[] hitStun;


    [Header("Attack Pushback")]
    public float defaultBlockPush = 1;
    public float defaultHitPush = 1;
}
