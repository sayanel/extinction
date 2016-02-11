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
            Herbie
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
            Herbie
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
            Idle,
            StrafeLeft,
            StrafeRight,
            Run,
            RunBackward,
            Sprint,
            Jump
        }
    }
}