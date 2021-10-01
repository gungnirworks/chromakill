using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camera2P : MonoBehaviour
{
    // This is a 2P camera rig designed to track two individual targets on the screen at once.

    public bool debugOn = true;

    public float centerLerpMagnitude = 5;

    public Transform[] pTransform;
    public Transform[] pLookTarget;

    private Text[] posText;

    private Transform[] pScreenSpace;

    private Text screenDebugDisplay;
    private Text screenDebugTitle;

    public Transform rigAnchor;
    public Transform rigTarget;
    public Transform camAnchor;

    private DebugScreenSpace debugSS;


    /*
     * Everything here sucks, but it's saved for posterity. I'm not using this shit
     * 
     * public float camLerpMagnitude = 5;
    public float camExtraLerp = 1;
    public float camExtraLerpMultiplicative = 10;
    public float lookLerpMagnitude = 5;

    public float camAnchorDefaultDistance = 2.25f;
    public float camAnchorOffset = 0f;
    public float camAnchorAdjustSpeed = 0.1f;

    public float bigXRatio = 0.8f;
    public float bigYRatio = 0.72f;
    public float smallXRatio = 0.7f;
    public float smallYRatio = 0.55f;*/

    //public Transform[] targets;

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
    }

    private void FixedUpdate()
    {
    }

    void AdjustPosition()
    {
        Vector3 midPoint = Vector3.Lerp(pTransform[0].position, pTransform[1].position, 0.5f);
        rigAnchor.position = Vector3.Lerp(rigAnchor.position, midPoint, Time.deltaTime * centerLerpMagnitude);
    }

    void LerpCamera()
    {
        Vector2 tempPos;
        Vector2[] screenPos = new Vector2[2];
        //bool shrinkOK = true;

        for (int i = 0; i < pLookTarget.Length; i++)
        {
            tempPos = new Vector2(posText[i].transform.parent.position.x, posText[i].transform.parent.position.y);
            screenPos[i] = new Vector2(
                Camera.main.WorldToScreenPoint(pLookTarget[i].position).x,
                Camera.main.WorldToScreenPoint(pLookTarget[i].position).y);
            WriteScreenSpace(i, tempPos, screenPos[i]);
        }

        // =============================== Everything here sucks, but it's saved for posterity. I'm not using this shit =======================================================
        /*
                for (int i = 0; i < screenPos.Length; i++)
                {
                    float xCompare = Mathf.Abs(screenPos[i].x - Screen.width) / Screen.width;
                    float yCompare = Mathf.Abs(screenPos[i].y - Screen.height) / Screen.height;
                    if ((xCompare > bigXRatio || xCompare < 1-bigXRatio ) ||
                        (yCompare > bigYRatio || yCompare < 1-bigYRatio))
                    {
                        // if characters are close to the edge
                        camAnchorOffset += (
                            Mathf.Clamp(camAnchorAdjustSpeed - Vector3.Distance(pLookTarget[0].position, pLookTarget[1].position), 0.1f, camAnchorAdjustSpeed * 0.99f)) * Time.deltaTime;
                        camExtraLerp = camExtraLerpMultiplicative * Vector3.Distance(pLookTarget[0].position, pLookTarget[1].position);
                        shrinkOK = false;
                        break;
                    }
                }

                if (shrinkOK)
                {
                    camExtraLerp = 1;
                    for (int i = 0; i < screenPos.Length; i++)
                    {
                        float xCompare = Mathf.Abs(screenPos[i].x - Screen.width) / Screen.width;
                        float yCompare = Mathf.Abs(screenPos[i].y - Screen.height) / Screen.height;
                        if ((xCompare < smallXRatio && xCompare > 1-smallXRatio) &&
                            (yCompare < smallYRatio && yCompare > 1-smallYRatio))
                        {
                            // if character are getting closer together
                            camAnchorOffset = Mathf.Clamp(camAnchorOffset - (camAnchorAdjustSpeed * Time.deltaTime / 2), 0f, 999999f);
                            break;
                        }
                    }
                }

                Vector3 newCamAnchorPosition = camAnchor.localPosition;
                newCamAnchorPosition.z = -camAnchorDefaultDistance - camAnchorOffset;
                camAnchor.localPosition = newCamAnchorPosition;
                //Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, camAnchor.position, Time.deltaTime * camLerpMagnitude * camExtraLerp);
        */


        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, camAnchor.position, 1);
        Vector3 lookTarget = Vector3.Lerp(pLookTarget[0].position, pLookTarget[1].position, 0.5f);
        Camera.main.transform.LookAt(lookTarget);
    }

    void TrackScreenSpace()
    {
        for (int i = 0; i < pTransform.Length; i++)
        {
            posText[i].transform.parent.position = Camera.main.WorldToScreenPoint(pLookTarget[i].position);
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
            "\nWidth: " + Screen.width.ToString();

        posText[i].text = "WorldToScreenPoint: " + sP.ToString() +
            "\nxCompare: " + (Mathf.Abs(tP.x - Screen.width) / Screen.width).ToString() +
            "\nyCompare: " + (Mathf.Abs(tP.y - Screen.height) / Screen.height).ToString();
    }
}
