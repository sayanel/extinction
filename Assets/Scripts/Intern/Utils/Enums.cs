// @author: Alex
using UnityEngine;
using System.Collections;

namespace Extinction {
    namespace Enums{
        public enum UnitBehavior {
            Idle,
            Moving,
            Attacking
        }

        public enum PlayerType {
            Survivor,
            Herbie
        }

        public enum CharacterType {
            Survivor,
            SpecialRobot,
            Creaker,
            Herbie,
            MiniRobot
        }

        public enum CharacterName {
            Hal,
            Red,
            Anton,
            Malik,
            RController,
            RTank,
            RScout,
            Creaker,
            Herbie,
            RNinja,
            RCamera
        }

        public enum AmmoType {
            Default
        }

        public enum FXType {
            ShootFX,
            ExplosionFX
        }

        /// <summary>
        /// The current state of the character
        /// Can be used for the animation, sounds, etc. 
        /// </summary>
        public enum CharacterState
        {
            Idle = 0,
            Run = 1,
            RunBackward = -1,
            StrafeLeft = -2,
            StrafeRight = 2,
            Sprint = 3,
            Jump = 4
        }

        /// <summary>
        /// The hands state of a survivor
        /// </summary>
        public enum HandState
        {
            Idle = 0,
            Fire = 1,
        }
    }
}