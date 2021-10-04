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
            // This refers to hitstun and blockstun.

            public int Frames { get; set; }
            // How long will this NegativeState last?

            public int AtkLevel { get; set; }
            // This like attack levels in guilty gear.
            // This determines hit/blockstun and receiving animation.

            public float KnockbackMagnitude { get; set; }
            public Vector3 KnockbackVector { get; set; }
            // This affects the target's movement

            public float PushbackMagnitude { get; set; }
            // This affects the attacker's movement

            public void DefaultKnockbacks()
            {
                KnockbackMagnitude = 1;
            }
        }

        public class Blockstun : NegativeState
        {
            public Blockstun(int fr, Vector3 vec, int hitstr, float kbm = 1, float pbm = 1)
            {
                Frames = fr;
                AtkLevel = hitstr;
                KnockbackVector = vec;
                KnockbackMagnitude = kbm;
                PushbackMagnitude = pbm;
            }
        }

        public class Hitstun : NegativeState
        {
            public bool Launch { get; set; }

            public Hitstun(bool ln, int fr, Vector3 vec, int hitstr, float kbm = 1, float pbm = 1, int wb = 0, int fb = 0)
            {
                Launch = ln;
                Frames = fr;
                AtkLevel = hitstr;
                KnockbackVector = vec;
                KnockbackMagnitude = kbm;
                PushbackMagnitude = pbm;

                WallBounce = wb;
                FloorBounce = fb;
            }

            // on launch, attack levels no longer matter for hitstun.
            public int WallBounce { get; set; }
            public int FloorBounce { get; set; }

            public bool CanWallBounce(int elapsed)
            {
                return elapsed > WallBounce ? false : true;
            }
            public bool CanFloorBounce(int elapsed)
            {
                return elapsed > FloorBounce ? false : true;
            }
        }

        public class Hitstop
        {
            public int Frames { get; set; }
            public int Elapsed { get; set; }
        }
    }
}
