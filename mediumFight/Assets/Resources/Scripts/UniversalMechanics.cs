using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalMechanics : MonoBehaviour
{
    // This script should be attached to an object in the scene
    // to be fetched by other scripts for the sake of obtaining
    // data on universal mechanics and values.

    public static UniversalMechanics instance;

    public UniversalValues uValues;

    private void Start()
    {
        instance = this;
    }
}
