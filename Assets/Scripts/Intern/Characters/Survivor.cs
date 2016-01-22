// @author: Mehdi

using UnityEngine;
using System.Collections;
using Extinction.Weapons;
using Extinction.Enums;
using Extinction.Utils;

namespace Extinction
{
    namespace Characters
    {
        /// <summary>
        /// This class represents a survivor.
        /// </summary>
        public class Survivor : Character
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// Returns if the survivor is aiming or not
            /// </summary>
            public bool isAiming { get { return _aiming; } }

            /// <summary>
            /// Returns the survivor's orientation, i.e the vector which describes where he is looking at
            /// </summary>
            public Vector3 orientation { get { return _orientation; } }

            /// <summary>
            /// Returns the survivor's state.
            /// It can be used for animation.
            /// </summary>
            public CharacterState state { get { return _state; } }

            /// <summary>
            /// Reduce or increase the survivor's lifebar
            /// </summary>
            private float _lifeMultiplier = 1;

            /// <summary>
            /// Reduce or increase the survivor's stealth
            /// </summary>
            private float _stealthMultiplier = 1;

            /// <summary>
            /// Reduce or increase the survivor's damages
            /// </summary>
            private float _damageMultiplier = 1;

            /// <summary>
            /// Reduce or increase the survivor's armor
            /// </summary>
            private float _armorMultiplier = 1;

            /// <summary>
            /// Reduce or increase the survivor's speed
            /// </summary>
            [SerializeField]
            private float _speedMultiplier = 3;

            /// <summary>
            /// A pointer to a Unity CharacterController
            /// The Controller Should be put in the same GameObject than this script
            /// </summary>
            [SerializeField]
            private CharacterController _controller;

            /// <summary>
            /// A pointer to the weapon of the survivor
            /// </summary>
            [SerializeField]
            private Weapon _weapon;

            /// <summary>
            /// Fake gravity used when the survivor jumps
            /// </summary>
            [SerializeField]
            private float _gravity = 0.2f;

            /// <summary>
            /// Some kind of impulse speed when the survivor starts to jump
            /// </summary>
            [SerializeField]
            private float _jumpImpulse = 0.4f;

            [SerializeField]
            private Timer _aimingTimer;

            private float _verticalSpeed = 0;
            private Vector3 _speed;
            private Vector3 _orientation = Vector3.forward;
            private bool _aiming = false;
            private Vector3 _trueOrientation = Vector3.forward;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            public void Start()
            {
                _orientation = Vector3.forward;

                _speed = Vector3.zero;

                if ( _controller != null ) return;

                _controller = GetComponent<CharacterController>();
            }

            public void Update()
            {
                move( _speed );
                applyGravity();

                _speed.x = 0;
                _speed.z = 0;

                float minRange = -1;
                float maxRange = 1;

                Vector3 random = Vector3.Normalize( new Vector3( Random.Range( minRange, maxRange ), Random.Range( minRange, maxRange ), Random.Range( minRange, maxRange ) ) );
                _trueOrientation = (0.3f * random + 1.7f * _orientation)/2;
                Vector3 camPosition = GetComponentInChildren<Camera>().transform.position;
                Debug.DrawLine( camPosition, camPosition + 10 * _orientation );
                Debug.DrawLine( camPosition, camPosition + 10 * _trueOrientation, Color.blue );
            }

            /// <summary>
            /// Remove the amount of damage from survivor's health (Inherited from Character)
            /// </summary>
            /// <param name="amount">The health that will be removed</param>
            public override void getDamage( int amount )
            {
                _health -= amount * _armorMultiplier;
            }

            /// <summary>
            /// The Survivor will move it the given direction
            /// Survivor.position += vec
            /// </summary>
            /// <param name="vec"></param>
            public override void move( Vector3 vec )
            {
                _controller.Move( vec );
            }

            public override void activateSkill1()
            {
                throw new System.NotImplementedException();
            }

            public override void activateSkill2()
            {
                throw new System.NotImplementedException();
            }

            /// <summary>
            /// Change the player's current state to idle
            /// </summary>
            public void idle()
            {
                _state = CharacterState.Idle;
            }

            /// <summary>
            /// Change the player's current state to strafeLeft
            /// Move the player left
            /// </summary>
            /// <param name="value"></param>
            public void strafeLeft( float value )
            {
                _state = CharacterState.StrafeLeft;
                horizontalMovement( value );
            }

            /// <summary>
            /// Change the player's current state to strafeRight
            /// Move the player right
            /// </summary>
            /// <param name="value"></param>
            public void strafeRight( float value )
            {
                _state = CharacterState.StrafeRight;
                horizontalMovement( value );
            }

            /// <summary>
            /// Change the player's current state to run
            /// Move the player front
            /// </summary>
            /// <param name="value"></param>
            public void run( float value )
            {
                if(_state != CharacterState.Sprint) 
                    _state = CharacterState.Run;

                verticalMovement( value );
            }

            /// <summary>
            /// Change the player's current state to runBackward
            /// Move the player back
            /// </summary>
            /// <param name="value"></param>
            public void runBackward( float value )
            {
                if ( _state == CharacterState.Sprint )
                {
                    return;
                }

                _state = CharacterState.RunBackward;
                verticalMovement( value );
            }

            /// <summary>
            /// Change the player's current state to sprint
            /// The player will move faster as long as Sprinting Key is pressed
            /// </summary>
            /// <param name="value"></param>
            public void sprint(bool sprinting)
            {
                if ( _state != CharacterState.Run && _state != CharacterState.Sprint && _state != CharacterState.Idle ) return;

                _state = sprinting ? CharacterState.Sprint : CharacterState.Idle;
                _aiming = false;
            }

            /// <summary>
            /// Change the player's current state to jump
            /// If the player is grounded, it creates a speed impulse
            /// </summary>
            /// <param name="value"></param>
            public void jump()
            {
                if ( !_controller.isGrounded ) return;

                _speed.y = _jumpImpulse * Time.deltaTime;
            }

            /// <summary>
            /// The survivor uses his weapon
            /// </summary>
            public void fire()
            {
                _weapon.fire();
            }

            /// <summary>
            /// When the survivor is aiming, he's more accurate but he cannot sprint
            /// </summary>
            /// <param name="aiming">True or false</param>
            public void aim( bool aiming )
            {
                if(_state != CharacterState.Sprint) _aiming = aiming;
            }

            /// <summary>
            /// The orientation vector is rotated around X & Y axis
            /// </summary>
            /// <param name="verticalOrientation">The new vertical angle in degrees</param>
            /// <param name="horizontalOrientation">The new horizontal angle in degrees</param>
            public void setOrientation( float verticalOrientation, float horizontalOrientation )
            {
                _orientation = Quaternion.Euler( -verticalOrientation, horizontalOrientation, 0 ) * Vector3.forward;
            }

            /// <summary>
            /// Turn the GameObject around Y Axis
            /// </summary>
            /// <param name="angle">The rotation angle in Degrees</param>
            public override void turn( float angle )
            {
                transform.Rotate( Vector3.up, angle );
            }

            /// <summary>
            /// Apply a Horizontal Translation to the Gameobject
            /// </summary>
            /// <param name="horizontalValue"></param>
            private void horizontalMovement( float horizontalValue )
            {
                Vector3 movement = new Vector3( _orientation.z, 0, -_orientation.x ) * horizontalValue * _defaultCharacterSpeed * _speedMultiplier * Time.deltaTime;

                _speed.x += movement.x;
                _speed.z += movement.z;
            }

            /// <summary>
            /// Apply a Vertical Translation to the Gameobject
            /// </summary>
            private void verticalMovement( float verticalValue )
            {
                float multiplier = _state == CharacterState.Sprint ? 2 : 1;
                multiplier *= verticalValue;
                multiplier *= _defaultCharacterSpeed;
                multiplier *= _speedMultiplier;

                Vector3 movement = new Vector3( _orientation.x, 0, _orientation.z ) * multiplier * Time.deltaTime;

                _speed.x += movement.x;
                _speed.z += movement.z;
            }

            /// <summary>
            /// If the player is not grounded, applies gravity
            /// </summary>
            private void applyGravity()
            {
                if ( _controller.isGrounded )
                {
                    _speed.y = 0;
                    return;
                }

                _speed.y -= _gravity * Time.deltaTime;
            }
        }
    }
}
