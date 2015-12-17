// Created by Florian, Mehdi-Antoine  & Pascale

using UnityEngine;
using System.Collections;

namespace Extinction
{
    namespace Characters
    {
        /// <summary>
        /// The current state of the character
        /// Can be used for the animation, sounds, etc. 
        /// </summary>
        public enum CharacterState
        {
        }

        /// <summary>
        /// Abastract class that should be derived for all characters in the game :
        /// Survivors, Unit, Special Robots and son on
        /// </summary>
        public abstract class Character : MonoBehaviour
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// Max health that can have the character
            /// </summary>
            protected static int _maxHealth;

            /// <summary>
            /// Current health of the character
            /// </summary>
            protected int _health;

            /// <summary>
            /// The current state of the character
            /// Can be used for the animation, sounds, etc. 
            /// </summary>
            protected CharacterState _state;

            /// <summary>
            /// Position in the space
            /// </summary>
            protected Vector3 _position;

            /// <summary>
            /// Basically it's a lookat :
            /// This attribute is the vector where the character is looking at
            /// </summary>
            protected Vector3 _orientation;

            // ---------- Static values ---------

            /// <summary>
            /// Default speed of a character, without any passive skill
            /// </summary>
            protected static float _defaultCharacterSpeed = 1;

            // ----------------------------------------------------------------------------
            // ---------------------------------- METHODS ---------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// This method must be derived to describe how to move a character
            /// </summary>
            /// <param name="vec"> It can be a target position, or a new position </param>
            public abstract void move( Vector3 vec );

            /// <summary>
            /// Rotate the Character around Y Axis (up vector)
            /// </summary>
            /// <param name="angle">
            /// angle must be in degrees
            /// positive value : clockwise
            /// negative value : counterclockwise
            /// </param>
            public abstract void turn( float angle );

            /// <summary>
            /// Sets the orientation vector of the character
            /// basically describes in which direction the character is looking at
            /// </summary>
            /// <param name="orientation"> orientation Vector </param>
            public abstract void setOrientation( Vector3 orientation );

            /// <summary>
            /// Decreases character's health
            /// </summary>
            /// <param name="amount">quantity of health to decrease</param>
            public abstract void getDamage( int amount );

            /// <summary>
            /// Activates skill1
            /// </summary>
            public abstract void activateSkill1();

            /// <summary>
            /// Activates skill2
            /// </summary>
            public abstract void activateSkill2();
        }
    }
}