using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PInputsBase
{
    public class PlayerInputsBase : MonoBehaviour
    {
        // This is the base class from which all the individual
        // player component scripts will be derived.
        //
        // This script pulls all the relevant information and
        // variables from the core player script automatically,
        // standardizing the lanugage that I use within these scripts.

        protected PPlayer player;
        protected PInput pInput;
        protected PMovement pMovement;
        protected PAttacks pAttacks;
        protected PAnim pAnim;
        protected UniversalMechanics uMech;

        protected void Start()
        {
            //Debug.Log(gameObject.ToString() + ": " + this.ToString() + " Start() has been called.");

            // Fetch all relevant pointers
            if (!FetchScripts())
            {
                Debug.Log(gameObject.ToString() + ": " + this.ToString() + " could not be fully initialized due to missing components.");
            }

            // Run custom start code, if any
            CustomStart();
        }

        protected bool FetchScripts()
        {
            if (GetComponent<PPlayer>() == null)
            {
                Debug.Log(gameObject.ToString() + ": " + this.ToString() + " could not find PPlayer component.");
                return false;
            }
            player = GetComponent<PPlayer>();

            if (player.pInput == null)
            {
                Debug.Log(gameObject.ToString() + ": " + this.ToString() + " could not find PInput component.");
                return false;
            }
            pInput = player.pInput;


            if (player.pMovement == null)
            {
                Debug.Log(gameObject.ToString() + ": " + this.ToString() + " could not find PMovement component.");
                return false;
            }
            pMovement = player.pMovement;

            if (player.pAttacks == null)
            {
                Debug.Log(gameObject.ToString() + ": " + this.ToString() + " could not find PAttacks component.");
                return false;
            }
            pAttacks = player.pAttacks;

            if (player.pAnim == null)
            {
                Debug.Log(gameObject.ToString() + ": " + this.ToString() + " could not find PAnim component.");
                return false;
            }
            pAnim = player.pAnim;

            if (UniversalMechanics.instance == null)
            {
                Debug.Log(gameObject.ToString() + ": " + this.ToString() + " could not find UniversalMechanics instance.");
                return false;
            }
            uMech = UniversalMechanics.instance;

            return true;
        }

        protected virtual void CustomStart()
        {
            // Custom start code for the children if they need it
        }

        private void Update()
        {
            // This is for testing only.
            if (player.negState == null)
            {
                //Debug.Log("No negative state.");
            }

            // Run custom update code, if any
            CustomUpdate();
        }

        protected virtual void CustomUpdate()
        {
            // Custom update code for the children if they need it
        }

        // IDK why I wrote this method when I can just use Debug.Log()
        /*protected virtual void DebugPrint(string message)
        {
            Debug.Log(message);  
        }*/

    }

    public class BufferElement
    {
        public int buttonPress; // which button was pushed
        public int checkType;   // which check are we looking for?
                                //      0 == press
                                //      1 == hold
                                //      2 == release
        public bool easy;       // was this button held for easy input?
        public int elapsed;

        public BufferElement(int bp, int t)
        {
            buttonPress = bp;
            checkType = t;
            easy = checkType == 1 ? true : false;
            elapsed = 0;
        }
    }

    public class InputBuffer
    {
        public List<BufferElement> elements;
        public UniversalMechanics UM;
        public int playerNumber;

        public InputBuffer(int pn)
        {
            elements = new List<BufferElement>();
            UM = UniversalMechanics.instance;
            playerNumber = pn;
        }

        public void BufferProgression()
        {
            if (elements.Count < 1)
            {
                //Debug.Log("Input buffer for player " + playerNumber.ToString() + " is empty.");
                return;
            }

            // Check elapsed time:
            foreach (BufferElement element in elements)
            {
                if (element != null)
                {
                    if (element.elapsed > UM.uValues.bufferWindow &&
                        element.checkType != 1)
                    {
                        // only time out buffer elements if they aren't hold
                        elements.Remove(element);
                    }
                    else if (element.elapsed > UM.uValues.easyInput &&
                        element.checkType == 1)
                    {
                        // hold is only removed when the button release is added
                        element.easy = false;
                    }
                    element.elapsed++;
                }
            }

            // Clean up:
            foreach (BufferElement element in elements)
            {
                if (element == null)
                {
                    elements.Remove(element);
                }
            }
        }

        public void ResetBuffer()
        {
            elements = new List<BufferElement>();
        }

        public void Add(BufferElement element)
        {
            bool inputAlreadyExists = false;

            for (int i = 0; i < elements.Count; i++)
            {
                // iterate through the buffer and check each one
                // to see if an identical element already exists.

                if (elements[i].checkType == element.checkType)
                {
                    if (element.checkType == 1)
                    {
                        // the held button is already there.
                        inputAlreadyExists = true;
                    }
                    else
                    {
                        if (elements[i].elapsed < 1)
                        {
                            // the same input already exists.
                            inputAlreadyExists = true;
                        }
                    }
                }
            }

            if (inputAlreadyExists) return;

            elements.Add(element);

            /*switch (element.checkType)
            {
                case 0: // press
                    {
                    }
                    break;
                case 1: // hold
                    {
                    }
                    break;
                case 2: // release
                    {
                    }
                    break;
            }*/
        }

        public void RemoveHold(int button)
        {
            foreach (BufferElement element in elements)
            {
                if (element.buttonPress == button &&
                    element.checkType == 1)
                {
                    elements.Remove(element);
                }
            }
        }
    }
}