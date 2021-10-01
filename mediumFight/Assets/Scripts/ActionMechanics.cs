using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActionMech
{
    public class ActionMechanics : MonoBehaviour
    {
        public class NegativeState
        {
            // A NegativeState is any state that will generally
            // prevent the player from using an input to take action.
            // This refers to hitstun, blockstun, and even the
            // input-lockout portion of an attack animation that the
            // player activates.
            // This works because all of these states are mutually
            // exclusive: You cannot be in blockstun AND hitstun,
            // you cannot bee in blockstun while attacking, and so on.

            public int frames;
            // How long will this NegativeState last?

            public int atkLevel;
            // This like attack levels in guilty gear.
            // This determines hit/blockstun and receiving animation.

            public float knockbackMagnitude;
            public Vector3 knockbackVector;
            // This affects the target's movement

            public float pushbackMagnitude;
            // This affects the attacker's movement

            public void DefaultKnockbacks()
            {
                knockbackMagnitude = 1;
            }
        }

        public class Blockstun : NegativeState
        {
            public Blockstun(int fr, Vector3 vec, int hitstr, float kbm = 1, float pbm = 1)
            {
                frames = fr;
                atkLevel = hitstr;
                knockbackVector = vec;
                knockbackMagnitude = kbm;
                pushbackMagnitude = pbm;
            }
        }

        public class Hitstun : NegativeState
        {
            public bool launch;

            public Hitstun(bool ln, int fr, Vector3 vec, int hitstr, float kbm = 1, float pbm = 1, int wb = 0, int fb = 0)
            {
                launch = ln;
                frames = fr;
                atkLevel = hitstr;
                knockbackVector = vec;
                knockbackMagnitude = kbm;
                pushbackMagnitude = pbm;

                wallBounce = wb;
                floorBounce = fb;
            }

            // on launch, attack levels no longer matter for hitstun.

            public int wallBounce;
            public int floorBounce;

            public bool CanWallBounce(int elapsed)
            {
                return elapsed > wallBounce ? false : true;
            }
            public bool CanFloorBounce(int elapsed)
            {
                return elapsed > floorBounce ? false : true;
            }
        }

        public class AttackLock : NegativeState
        {

        }

        public class Hitstop
        {
            public int frames;
        }
    }
}
