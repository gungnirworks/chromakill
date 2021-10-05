using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugStateDisplay : MonoBehaviour
{
    public Text playerState;

    private DebugInputDisplay inputDisplay;

    private void Start()
    {
        inputDisplay = DebugInputDisplay.instance;
    }

    private void Update()
    {
        playerState.gameObject.SetActive(inputDisplay.debugOn);

        if (inputDisplay.debugOn)
        {
            playerState.text = "Player " + inputDisplay.TrackingPlayer.ToString() + " airborne state: " +
                inputDisplay.Player(inputDisplay.TrackingPlayer).pMovement.Airborne.ToString();
        }
    }
}
