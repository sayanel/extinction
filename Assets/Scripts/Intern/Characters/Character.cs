// @author: Florian, Mehdi-Antoine  & Pascale

using UnityEngine;
using System.Collections;

using Extinction.Enums;

namespace Extinction
{
    namespace Characters
    {

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
            [SerializeField]
            protected static float _maxHealth = 100;

            public static float MaxHealth{
                get { return _maxHealth; }
            }

            /// <summary>
            /// Current health of the character
            /// </summary>
            [SerializeField]
            protected float _health = 100;

            public float Health{
                get { return _health; }
                set { _health = value; }
            }

            /// <summary>
            /// Parameter to quickly verify if this character is alive or dead.
            /// </summary>
            [SerializeField]
            protected bool _isAlive = true;

            public bool IsAlive{
                get{ return _isAlive; }
            }


            /// <summary>
            /// The animator attached to this Character.
            /// </summary>
            [SerializeField]
            protected Animator _animator;

            /// <summary>
            /// The name of the current animation played by this character.
            /// </summary>
            protected string _currentAnimationState;
            public string CurrentAnimationState{
                get{ return _currentAnimationState; }
            }


            /// <summary>
            /// The current state of the character
            /// Can be used for the animation, sounds, etc. 
            /// </summary>
            protected CharacterState _state = CharacterState.Idle;

            /// <summary>
            /// Default speed of a character, without any passive skill
            /// </summary>
            [SerializeField]
            protected static float _defaultCharacterSpeed = 2;

            /// <summary>
            /// The name of this character.
            /// </summary>
            [SerializeField]
            protected CharacterName _characterName;

            /// <summary>
            /// The type of this character.
            /// </summary>
            [SerializeField]
            protected CharacterType _characterType;

            // ----------------------------------------------------------------------------
            // ---------------------------------- METHODS ---------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// Return the name of this character.
            /// </summary>
            /// <returns></returns>
            public CharacterName getCharacterName()
            {
                return _characterName;
            }

            public CharacterType getCharacterType()
            {
                return _characterType;
            }

            /// <summary>
            /// Use this function to play an animation with name : stateName.
            /// By default, it uses trigger animator value. Override the function to use different value type.
            /// </summary>
            /// <param name="stateName"></param>
            public virtual void setAnimationState(string stateName)
            {
                _currentAnimationState = stateName;

                if( _animator != null)
                    _animator.SetTrigger(stateName);
            }

            /// <summary>
            /// Use this function to change animation played by this character.
            /// By default, it uses trigger animator value. Override the function to use different value type.
            /// </summary>
            /// <param name="oldState"></param>
            /// <param name="newState"></param>
            public virtual void changeAnimationState(string oldState, string newState)
            {
                _currentAnimationState = newState;

                if (_animator != null)
                    _animator.SetTrigger(newState);
            }

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

            /// Decreases character's health
            /// </summary>
            /// <param name="amount">quantity of health to decrease</param>
            public abstract void getDamage( int amount );

            [PunRPC]
            public void SetHealth(float life) {
                _health = life;
            }

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