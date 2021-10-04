using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PInputsBase;

[RequireComponent(typeof(DebugInputDisplay))]
public class DebugInputBuffer : MonoBehaviour
{
    public static DebugInputBuffer instance;
    private DebugInputDisplay inputDisplay;

    public Color[] checkTypeColors;
    public Color[] internalColors;

    public Image[] colorLegend;

    public GameObject bufferElementPrefab;
    public List<GameObject> bufferElements;
    public InputBuffer inputBuffer;

    public Text bufferTitle;

    public Transform offset;
    public float xOffsetAmount = 10f;

    private void Awake()
    {
        instance = this;
        ResetBuffer();
    }

    private void Start()
    {
        inputDisplay = GetComponent<DebugInputDisplay>();
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
                DestroyImmediate(bufferElements[i]);
                //bufferElements.RemoveAt(i);
            }
        }

        if (bufferElements != null && bufferElements.Count > 0)
        {
            //Debug.Log("bufferElements is both non-null and at count > 0");
            for (int i = bufferElements.Count - 1; i > -1; i--)
            {
                if (bufferElements[i] == null)
                bufferElements.RemoveAt(i);
            }
        }
        //bufferElements = new List<GameObject>();
        //inputBuffer = null;
    }

    private void DrawBuffer()
    {
        ResetBuffer();
        if (!inputDisplay.debugOn)
        {
            bufferTitle.gameObject.SetActive(false);
            offset.gameObject.SetActive(false);
            return;
        }
        else
        {
            if (inputDisplay.Player(inputDisplay.TrackingPlayer) == null)
            {
                Debug.Log("Debug Input Buffer could not track player: " + inputDisplay.TrackingPlayer.ToString());
                return;
            }

            bufferTitle.gameObject.SetActive(true);
            offset.gameObject.SetActive(true);
            bufferTitle.text = "Input Buffer: Player " + inputDisplay.TrackingPlayer.ToString();

            for (int i = 0; i < colorLegend.Length; i++)
            {
                if (i == 3)
                {
                    colorLegend[i].color = internalColors[1];
                }
                else if (i == 4)
                {
                    colorLegend[i].color = checkTypeColors[3];
                }
                else
                {
                    colorLegend[i].color = checkTypeColors[i];
                }
            }

            // populate the buffer
            inputBuffer = inputDisplay.Player(inputDisplay.TrackingPlayer).pInput.inputBuffer;
        }

        // draw the buffer

        /*if (inputBuffer == null || inputBuffer.Elements.Count < 1)
        {
            ResetBuffer();
        }*/

        //ResetBuffer();

        for (int i = 0; i < inputBuffer.Elements.Count; i++)
        {
            //Vector3 newOffset = new Vector3(offset.position.x + xOffsetAmount * i, offset.position.y, offset.position.z);
            GameObject newElement = Instantiate(bufferElementPrefab, offset);
            newElement.transform.position = new Vector3(offset.position.x + ( Screen.width * xOffsetAmount * i), offset.position.y, offset.position.z);
            bufferElements.Add(newElement);

            DebugBufferElement bufferElement = newElement.GetComponent<DebugBufferElement>();
            bufferElement.buttonValue.text = inputBuffer.Elements[i].ButtonPress.ToString();
            bufferElement.elapsedText.text = inputBuffer.Elements[i].Elapsed.ToString();
            bufferElement.border.color = checkTypeColors[inputBuffer.Elements[i].CheckType];
            bufferElement.insideImage.color = inputBuffer.Elements[i].Easy ?
                internalColors[1] : internalColors[0];
        }
    }
}
