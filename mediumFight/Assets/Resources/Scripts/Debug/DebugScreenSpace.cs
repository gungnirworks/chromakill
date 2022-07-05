using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugScreenSpace : MonoBehaviour
{
    public static DebugScreenSpace instance;

    public Text[] posText;

    public Text screenDebugDisplay;
    public Text screenDebugTitle;

    private void Awake()
    {
        instance = this;
    }
}
