using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PInputsBase;

[RequireComponent(typeof(PPlayer))]
public class PMovement : PlayerInputsBase
{
    // This script handles all player movement.
    // HOWEVER, this script should never directly poll for player inputs. This script should
    // only refer to PInput and PPlayer to determine inputs and player state and make the
    // proper movements as are relevant.
    //

    public float moveMagnitude = 1f;
    public float jumpMagnitude = 1f;

    private List<MoveAdd> moveAdds;
    private Launch launch;
    private Vector3 airMovement;

    public bool Airborne { get; set; }

    private void FixedUpdate()
    {
        if (launch != null)
        {
            if (launch.Finished()) launch = null;
        }

        CheckAirborne();
    }

    public void Move()
    {
        // This move method is used for intentional, player-input movement.

        if (pInput.Movement != Vector3.zero)
        {
            //Debug.Log("movement is: " + pInput.movement.ToString());

            if (!Airborne) // if grounded,
            {
                gameObject.transform.position += pInput.Movement * moveMagnitude;
                pAnim.SetAnim("Running", true);
            }
            else
            {               // if airborne,
                if (pInput.Movement.magnitude > 0)
                {
                    gameObject.transform.position += pInput.Movement * moveMagnitude * uMech.uValues.airMovementScaling;
                }
            }

            gameObject.transform.LookAt(transform.position + pInput.LastDirection, Vector3.up);
        }
        else
        {
            pAnim.SetAnim("Running", false);
        }
    }

    public void CheckAirborne()
    {
        // Check if the character is airborne.
        // There are two different airborne checks: One of them is done here in this method,
        // and the other is done via collision detection in PPlayer. Each one allows us to check
        // different types of ground collisions. This check uses a raycast from the center of the
        // player position, while the other one uses the actual hitbox collisions using unity physics.

        // I may remove the collision method later. For now I'm going to use them both.

        if (launch != null)
        {
            // Debug.Log("Airborne should be true.");
            Airborne = true;
            return;
        }

        if (player.RB.velocity.y > 0)
        {
            Airborne = true;
            return;
        }

        if (!Airborne) return;
        // at this point, if you're still grounded, don't bother doing any more checks.

        Vector3 yOffset = Vector3.up * 0.05f;
        float yVelocityCheckMagnitude = player.RB.velocity.y * Time.fixedDeltaTime;

        bool downcheck = Physics.Raycast(player.transform.position + yOffset, Vector3.down, out RaycastHit hit,
            0.05f + Mathf.Abs(yVelocityCheckMagnitude));
/*
        Debug.DrawLine(player.transform.position + (Vector3.up * 0.05f),
            player.transform.position - yOffset + (Vector3.up * yVelocityCheckMagnitude), Color.red, 1f);*/

        if (downcheck)
        {
            if (hit.collider.transform.CompareTag("Stage") &&
                hit.collider.GetComponent<StageCheck>() != null)
            {
                if (hit.collider.GetComponent<StageCheck>().ground)
                {
                    Debug.Log("Raycast Downcheck found the stage ground.");
                    Airborne = false;
                    return;
                }
            }
        }
    }

    public void ResetLaunch()
    {
        launch = null;
    }

    public void StartLaunch(bool intentional)
    {
        // Use this to put the character into the air.
        launch = null;

        if (!intentional)
        {

            return;
        }

        launch = new Launch(pInput.Movement + (Vector3.up * jumpMagnitude));
        player.RB.velocity = launch.initialDirection;
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

        public bool Finished()
        {
            if (elapsed >= duration)
            {
                return true;
            }

            elapsed++;
            return false;
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
