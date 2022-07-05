using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    // =========================================================[[ ATTACKS ]]=============================
    [Serializable]
    public class Attack
    {
        // This is a class designed just to hold the data for any attack.

        //public string attackName;
        public string animParameter;

        public int attackLevel = 0;

        public AttackPhase startup;
        public AttackPhase active;
        public AttackPhase recovery;

        // This class is currently extremely simplified for the sake of implementing
        // core mechanics. I will begin porting over attack code from the previous version
        // of chromakill, which should be fine because it's mostly just custom classes.
    }

    [Serializable]
    public class AttackPhase
    {
        // this is the test version
        public int frames = 5;
        public Vector3 velocity = Vector3.forward;
    }

    // =========================================================[[ ATTACKS (ported from previous version) ]]=============================

    #region Previous Version Attacks
/*
    #region CopyPasted
    [System.Serializable]
    public class Stance
    {
        [Header("Stance Details")]
        public string name;
        public string stanceButton;
        public string stanceAnimTag;
        public bool startAfterAttack = false;
        public int startupFrames;
        public int recoveryFrames;

        public int finiteFrames = 0;
        // if finiteFrames == 0, it will last as long as you hold the button

        public float moveSpeedModifier = 1;
        public bool lockFacing = false;
        public bool crushable = false;
        public bool invincible = false;


        [Header("Stance Attack (Quick)")]
        public List<AttackType> SAQuick = new List<AttackType>();
        [Header("Stance Attack (Strong)")]
        public List<AttackType> SAStrong = new List<AttackType>();
        [Header("Stance DoubleAttack")]
        public List<AttackType> SADouble = new List<AttackType>();
    }

    [System.Serializable]
    public class AttackMovement
    {
        public Vector3 movement;
        public int[] frameTrigger;
    }

    [System.Serializable]
    public class AttackProperties
    {

        public int frames;
        // public string animPropertyTag;
        public int[] crushFrames;
        public int[] invFrames;
        public AttackMovement[] attackMovement;
        public bool branching = false;

        public bool aimWhileAttacking = false;
        public bool moveWhileAttacking = false;
        public float attackMoveSpeedModifier = 0.5f;
    }

    [System.Serializable]
    public class AttackType
    {
        public string name;
        // The name of the attack

        public bool projectile;
        public string animPropertyTag;

        public Transform origin;
        //public Attack_Trigger atkTrigger;

        public bool effectsOnly = false;


        public int projectileAutoFireDelay = 0;
        // By default this is 0.
        // 0 means autofire off.

        public int damage = 1;
        public float knockback = 1f;
        public int lastHitDamage = 1;
        public float lastHitKnockback = 1f;
        public bool knockBackFromCenter = false;
        // if true, knockback will be from the center of the attack
        // rather than in the direction of the attack

        public bool knockdown = false;
        public bool launch = false;
        public float launchMagnitude = 0.8f;

        public int hitStun = 1;
        public int blockStun = 1;

        public int numberOfHits = 1;
        public int hitInterval = 0;
        // if numberOfHits > 0, hitInterval will determine the number of frames
        // each hit has to connect

        public int hitStop = 0;
        // how much hitstop does this cause on the last hit?
        public int hitStopAllHits = 0;
        // how much hitstop does this cause on hits that aren't the last hit?


        public Stance[] stanceType;


        public AttackProperties startup;
        public AttackProperties active;
        public AttackProperties recovery;

    }

    // [System.Serializable]
    public class AttackHitbox
    {
        // this class is just designed to hold values
        // these values should be passed in by a method from an AttackType

        public string name;
        public int playerId;
        public int playerTeam;

        public PAnim attackingPlayerAnim;

        public bool projectile;

        public int numberOfHits = 1;
        public int hitInterval = 0;
        // if numberOfHits > 0, hitInterval will determine the number of frames
        // each hit has to connect

        public bool knockBackFromCenter = false;

        public bool knockdown = false;
        public bool launch = false;
        public float launchMagnitude = 0.8f;

        public int hitStun = 1;
        public int blockStun = 1;

        public int frames;

        public int damage = 1;
        public float knockback = 1f;
        public int lastHitDamage = 1;
        public float lastHitKnockback = 1f;


        public int hitStop = 0;
        // how much hitstop does this cause on the last hit?
        public int hitStopAllHits = 0;
        // how much hitstop does this cause on hits that aren't the last hit?
    }

    public class HitTracking
    {
        // This class is designed to hold information about
        // objects getting hit by attacks

        public GameObject hitObj;
        // the object getting hit

        public Vector3 hitVector;
        // the direction of the force of attack

        public HitTracking(GameObject h)
        {
            hitObj = h;
        }
    }
    #endregion
*/
    #endregion
}
