using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class DebugInputDisplay : MonoBehaviour
{
    public static DebugInputDisplay instance;

    public GameObject inputDisplay;

    public bool debugOn = true;
    public int TrackingPlayer { get; set; } = 0;

    public Image[] buttonImages;
    public Color unpressedColor;
    public Color pressedColor;
    public Slider sliderX;
    public Slider sliderY;


    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        UpdateInputs();
    }

    private void UpdateInputs()
    {
        inputDisplay.SetActive(debugOn);
        if (!debugOn) return;

        PPlayer targetPlayer = Player(TrackingPlayer);

        if (targetPlayer == null)
        {
            Debug.Log("Debug Input Display could not track player: " + TrackingPlayer.ToString());
            return;
        }

        sliderX.value = targetPlayer.pInput.Movement.x;
        sliderY.value = targetPlayer.pInput.Movement.z;

        if (targetPlayer.pInput.RewiredPlayer == null) return;

        buttonImages[0].color = targetPlayer.pInput.RewiredPlayer.GetButton("Jump") ?
            pressedColor : unpressedColor;
        buttonImages[1].color = targetPlayer.pInput.RewiredPlayer.GetButton("QAtk") ?
            pressedColor : unpressedColor;
        buttonImages[2].color = targetPlayer.pInput.RewiredPlayer.GetButton("SAtk") ?
            pressedColor : unpressedColor;
        buttonImages[3].color = targetPlayer.pInput.RewiredPlayer.GetButton("Stance") ?
            pressedColor : unpressedColor;
        buttonImages[4].color = targetPlayer.pInput.RewiredPlayer.GetButton("Dash") ?
            pressedColor : unpressedColor;
        buttonImages[5].color = targetPlayer.pInput.RewiredPlayer.GetButton("Grab") ?
            pressedColor : unpressedColor;
    }

    public PPlayer Player(int targetPlayer)
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in playerObjects)
        {
            if (player.GetComponent<PPlayer>().PlayerNumber == targetPlayer)
            {
                return player.GetComponent<PPlayer>();
            }
        }

        return null;
    }
}
