using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StageCheck : MonoBehaviour
{
    // Put this script on stage components so we can check for
    // stage properties when collided with etc

    public bool ground = true;

    public Rigidbody RB { get; set; }

    private void Awake()
    {
        RB = GetComponent<Rigidbody>();
    }
}
