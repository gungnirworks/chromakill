using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PInputsBase;

public class DebugInputBuffer : MonoBehaviour
{
    public static DebugInputBuffer instance;

    public bool debugOn = true;

    public GameObject bufferElementPrefab;
    public List<GameObject> bufferElements;
    public InputBuffer inputBuffer;

    public int trackingPlayer = 0;
    public Text bufferTitle;

    public Transform offset;

    private void Awake()
    {
        instance = this;
        ResetBuffer();
    }

    private void Update()
    {
        DrawBuffer();
    }

    private void ResetBuffer()
    {
        //Debug.Log("DebugInputBuffer: ResetBuffer() has been called." + "\nbufferElements count is at: " + bufferElements.Count.ToString());
        if (bufferElements != null && bufferElements.Count > 0)
        {
            //Debug.Log("bufferElements is both non-null and at count > 0");
            for (int i = bufferElements.Count -1; i > -1; i--)
            {
                //Debug.Log("Destroying bufferElement at index: " + i.ToString());
                Destroy(bufferElements[i]);
                bufferElements.RemoveAt(i);
            }
        }
        //bufferElements = new List<GameObject>();
        inputBuffer = null;
    }

    private PPlayer Player(int targetPlayer)
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject player in playerObjects)
        {
            if (player.GetComponent<PPlayer>().playerNumber == targetPlayer)
            {
                return player.GetComponent<PPlayer>();
            }
        }

        return null;
    }

    private void DrawBuffer()
    {
        if (!debugOn)
        {
            ResetBuffer();
            bufferTitle.gameObject.SetActive(false);
        }
        else
        {
            if (Player(trackingPlayer) == null)
            {
                Debug.Log("Debug Input Buffer could not track player: " + trackingPlayer.ToString());
                return;
            }

            bufferTitle.gameObject.SetActive(true);
            bufferTitle.text = "Input Buffer: Player " + trackingPlayer.ToString();

            // populate the buffer
            inputBuffer = Player(trackingPlayer).pInput.inputBuffer;
        }

        // draw the buffer

        if (inputBuffer == null || inputBuffer.elements.Count < 1)
        {
            ResetBuffer();
        }

        for (int i = 0; i < bufferElements.Count; i++)
        {

        }
    }
}
