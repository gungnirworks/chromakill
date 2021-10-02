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
            if (player.NegState == null)
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
    }

    //==================================================[[       INPUT BUFFER      ]]===================================================

    public class BufferElement
    {
        public int ButtonPress { get; set; } // which button was pushed
        public int CheckType { get; set; }   // which check are we looking for?
                                                //      0 == press
                                                //      1 == hold
                                                //      2 == release
                                                //      3 == logged for the buffer
        public bool Easy { get; set; }       // was this button held for easy input?
        public int Elapsed { get; set; }

        public BufferElement(int bp, int t)
        {
            ButtonPress = bp;
            CheckType = t;
            Easy = CheckType == 1 ? true : false;
            Elapsed = 0;
        }
    }

    public class InputBuffer
    {
        public List<BufferElement> Elements { get; set; }
        public readonly UniversalMechanics UM;
        public int PlayerNumber { get; set; }

        public InputBuffer(int pn)
        {
            Elements = new List<BufferElement>();
            UM = UniversalMechanics.instance;
            PlayerNumber = pn;
        }

        public void BufferProgression()
        {
            if (Elements.Count < 1)
            {
                //Debug.Log("Input buffer for player " + playerNumber.ToString() + " is empty.");
                return;
            }

            // Check elapsed time:
            //foreach (BufferElement element in Elements)
            for (int i = 0; i < Elements.Count; i++)
            {
                bool elementRemoved = false;
                // we might be removing elements from the buffer

                if (i >= Elements.Count) break;
                // because we're removing elements from the buffer, check
                // to make sure we're still in range

                if (Elements[i] != null)
                {
                    if (Elements[i].Elapsed > UM.uValues.pressDuration &&
                        Elements[i].CheckType == 0)
                    {
                        // time out press elements
                        Elements.Remove(Elements[i]);
                        elementRemoved = true;
                    }
                    else if (Elements[i].Elapsed > UM.uValues.releaseDuration &&
                        Elements[i].CheckType == 2)
                    {
                        // time out release elements if they aren't held
                        Elements[i].CheckType = 3;
                        elementRemoved = true;
                    }
                    else if (Elements[i].Elapsed > UM.uValues.easyInput &&
                        Elements[i].CheckType == 1)
                    {
                        // hold is only removed when the button release is added
                        Elements[i].Easy = false;
                    }
                    else if (Elements[i].Elapsed > UM.uValues.bufferWindow &&
                        Elements[i].CheckType == 3)
                    {
                        // if the logged button times out
                        Elements.Remove(Elements[i]);
                        elementRemoved = true;
                    }

                    if (elementRemoved)
                    { 
                        i--;
                        // if we removed an element, we're gonna use the same index again
                    }
                    else
                    {
                        Elements[i].Elapsed++;
                        // if we didn't remove the element, increment the elapsed timer
                    }
                }
            }

            // Clean up:
            foreach (BufferElement element in Elements)
            {
                if (element == null)
                {
                    Elements.Remove(element);
                }
            }
        }

        public void ResetBuffer()
        {
            Elements = new List<BufferElement>();
        }

        public void Add(BufferElement element)
        {
            bool inputAlreadyExists = false;
            //Debug.Log("Checking: " + element.ButtonPress.ToString() + " with checkType " + element.CheckType.ToString() + " for the input buffer.");

            for (int i = 0; i < Elements.Count; i++)
            {
                // iterate through the buffer and check each one
                // to see if an identical element already exists.

                if (Elements[i].ButtonPress == element.ButtonPress)
                {
                    if (Elements[i].CheckType == 1 && element.CheckType == 1)
                    {
                        // the held button is already there.
                        //Debug.Log("Held button " + element.ButtonPress.ToString() + " already exists in the buffer.");
                        inputAlreadyExists = true;
                    }
                    else
                    {
                        if (Elements[i].Elapsed < 1)
                        {
                            // the same input already exists.
                            inputAlreadyExists = true;
                        }
                    }
                }
            }

            if (inputAlreadyExists) return;

            Elements.Add(element);
            //Debug.Log("Adding: " + element.ButtonPress.ToString() + " with checkType " + element.CheckType.ToString() + " to input buffer.");

            switch (element.CheckType)
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
                        for (int i = 0; i < Elements.Count; i++)
                        {
                            if (Elements[i].ButtonPress == element.ButtonPress &&
                                Elements[i].CheckType == 1) // if the button is being held, remove it because it's been released
                            {
                                Elements.RemoveAt(i);
                                break;
                            }
                        }
                    }
                    break;
            }
        }

        public void RemoveHold(int button)
        {
            foreach (BufferElement element in Elements)
            {
                if (element.ButtonPress == button &&
                    element.CheckType == 1)
                {
                    Elements.Remove(element);
                }
            }
        }
    }
}