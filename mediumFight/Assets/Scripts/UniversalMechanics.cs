using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalMechanics : MonoBehaviour
{
    public static UniversalMechanics instance;

    public UniversalValues uValues;

    private void Start()
    {
        instance = this;
    }
}
