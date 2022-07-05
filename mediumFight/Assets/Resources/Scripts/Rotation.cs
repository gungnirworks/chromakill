using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{

    public float rotationAmount = 15;
    public float magnitude = 50;

    void Update()
    {
        transform.Rotate(0, rotationAmount * Time.deltaTime * magnitude, 0);
    }
}
