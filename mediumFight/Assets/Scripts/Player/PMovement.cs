using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PInputsBase;

public class PMovement : PlayerInputsBase
{
    // This script handles all player movement.
    // HOWEVER, this script should never directly poll for player inputs. This script should
    // only refer to PInput and PPlayer to determine inputs and player state and make the
    // proper movements as are relevant.
    //
    // THIS GAME DOES NOT USE UNITY PHYSICS FOR MOVEMENT AND ATTACKS. All movement should
    // be deliberately designed to function the way I want in a controlled manner.
    // For that reason,
    // This script should handle all movement, including movement enacted upon the player
    // by the world or other entities. This means that the movement methods in this script
    // should hold all movement checks and corrections.

    public float moveMagnitude = 1f;

    private List<MoveAdd> moveAdds;
    private Launch launch;
    private Vector3 airMovement;

    private void FixedUpdate()
    {
        if (!player.CheckNegative())
        {
            // If there is no NegativeState,
            Move(Airborne());
        }
    }

    private void Move(bool air)
    {
        // This move method is used for intentional, player-input movement.

        if (pInput.movement != Vector3.zero)
        {
            //Debug.Log("movement is: " + pInput.movement.ToString());
            gameObject.transform.position += pInput.movement * moveMagnitude;
            gameObject.transform.LookAt(transform.position + pInput.lastDirection, Vector3.up);
            pAnim.SetAnim("Running", true);
        }
        else
        {
            pAnim.SetAnim("Running", false);
        }
    }

    private bool Airborne()
    {
        // Check if the character is airborne.
        // If yes, return true.
        // If not, put the player on the ground and return false.

        if (launch != null) return true;

        airMovement = Vector3.zero;
        return false;
    }

    public void StartLaunch()
    {
        // Use this to put the character into the air.

        launch = null;
    }

    protected class Launch
    {
        public Vector3 initialDirection;
        public int duration; // how many FixedUpdate cycles does this last?
        public int elapsed = 1; // how many cycles has it been since it started?
                                // it starts at elapsed = 1 because this is checked
                                // only at the END of a movement.

        public Launch(Vector3 dir, int dur = -1)
        {
            initialDirection = dir;
            duration = dur < UniversalMechanics.instance.uValues.minimumLaunchTime ?
                UniversalMechanics.instance.uValues.minimumLaunchTime : dur;
        }
    }

    protected class MoveAdd
    {
        // This class contains data for additional movement enacted upon the player from
        // other sources.

        public Vector3 initialDirection; // the direction includes magnitude
        public int duration; // how many FixedUpdate cycles does this last?
        public bool decay; // does this MoveAdd decay over time?
        public int elapsed = 0; // how many cycles has it been since it started?
                                // elapsed is checked only at the END of a movement.

        public MoveAdd(Vector3 dir, int dur, bool dec = false)
        {
            initialDirection = dir;
            duration = dur;
            decay = dec;
        }

        public Vector3 AddDirection()
        {
            // this is the actual direction used to determine movement

            if (!decay) return initialDirection;

            float proportion = 1 - (elapsed / duration);
            return initialDirection * proportion;
        }

        public bool CheckEnd()
        {
            elapsed++;

            // At the end of movement, check only ONCE to see if it's finished.
            if (elapsed >= duration)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
