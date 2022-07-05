using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camera2P : MonoBehaviour
{
    // This is a 2P camera rig designed to track two individual targets on the screen at once.
    // This includes a single player camera to switch to.

    public Camera cam;

    public bool debugOn = true;

    public bool cam2p = true;

    public float centerLerpMagnitude = 5;
    public float camLerpMagnitude = 2;

    public Transform[] pTransform;
    public Transform[] pLookTarget;

    private Vector3 lastLookTarget;

    private Text[] posText;

    private Transform[] pScreenSpace;

    private Text screenDebugDisplay;
    private Text screenDebugTitle;

    public Transform rigAnchor;
    public Transform rigTarget;
    public Transform camAnchor;

    private DebugScreenSpace debugSS;

    private void Start()
    {
        FindDebugDisplay();
    }

    private void FindDebugDisplay()
    {
        debugSS = DebugScreenSpace.instance;
        posText = debugSS.posText;
        screenDebugDisplay = debugSS.screenDebugDisplay;
        screenDebugTitle = debugSS.screenDebugTitle;
    }

    private void Update()
    {
        AdjustPosition();
        TrackScreenSpace();
        LerpCamera();

        if (Input.GetKeyDown(KeyCode.O))
        {
            cam2p = !cam2p;
        }
    }

    private void FixedUpdate()
    {
    }

    void AdjustPosition()
    {
        if (cam2p)
        {
            Vector3 midPoint = Vector3.Lerp(pTransform[0].position, pTransform[1].position, 0.5f);
            rigAnchor.position = Vector3.Lerp(rigAnchor.position, midPoint, Time.deltaTime * centerLerpMagnitude);
        }
        else
        {
            rigAnchor.position = Vector3.Lerp(rigAnchor.position, pTransform[0].position, Time.deltaTime * centerLerpMagnitude);
        }
    }

    void LerpCamera()
    {
        float lookside = 0.5f;

        if (!cam2p)
        {
            // single player cam here:
            lookside = 0;
        }

        Vector2 tempPos;
        Vector2[] screenPos = new Vector2[2];

        for (int i = 0; i < pLookTarget.Length; i++)
        {
            tempPos = new Vector2(posText[i].transform.parent.position.x, posText[i].transform.parent.position.y);
            screenPos[i] = new Vector2(
                cam.WorldToScreenPoint(pLookTarget[i].position).x,
                cam.WorldToScreenPoint(pLookTarget[i].position).y);
            WriteScreenSpace(i, tempPos, screenPos[i]);
        }

        cam.transform.position = Vector3.Lerp(cam.transform.position, camAnchor.position, Time.deltaTime * camLerpMagnitude);
        Vector3 lookTarget = Vector3.Lerp(pLookTarget[0].position, pLookTarget[1].position, lookside);

        // Shit sucks:
        //Camera.main.transform.LookAt(lookTarget);
        //Camera.main.transform.rotation = Quaternion.RotateTowards(Camera.main.transform.rotation, Quaternion.LookRotation(lookTarget, Vector3.up), Time.deltaTime * camLerpMagnitude);

        Quaternion.RotateTowards(cam.transform.rotation, camAnchor.transform.rotation, Time.deltaTime * camLerpMagnitude);
        lastLookTarget = lookTarget;
    }

    void TrackScreenSpace()
    {
        for (int i = 0; i < pTransform.Length; i++)
        {
            posText[i].transform.parent.position = cam.WorldToScreenPoint(pLookTarget[i].position);
        }
    }

    void WriteScreenSpace(int i, Vector2 tP, Vector2 sP)
    {
        if (!debugOn)
        {
            foreach (Text text in posText)
            {
                text.gameObject.SetActive(false);
            }
            screenDebugDisplay.gameObject.SetActive(false);
            screenDebugTitle.gameObject.SetActive(false);
            return;
        }
        else
        {
            foreach (Text text in posText)
            {
                text.gameObject.SetActive(true);
            }
            screenDebugDisplay.gameObject.SetActive(true);
            screenDebugTitle.gameObject.SetActive(true);
        }

        screenDebugDisplay.text =
            "Height: " + Screen.height.ToString() +
            "\nWidth: " + Screen.width.ToString() + 
            "\n" + (1f / Time.deltaTime).ToString() + " FPS";

        posText[i].text = "WorldToScreenPoint: " + sP.ToString() +
            "\nxCompare: " + (Mathf.Abs(tP.x - Screen.width) / Screen.width).ToString() +
            "\nyCompare: " + (Mathf.Abs(tP.y - Screen.height) / Screen.height).ToString();
    }
}
