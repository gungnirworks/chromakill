using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public bool CinematicOn { get; set; }

    private void Awake()
    {
        instance = this;
    }
}