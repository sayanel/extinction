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
            /// Returns the survivor's orientation, i.e the vector which describes where he is looking at
            /// </summary>
            public Vector3 orientation { get { return _accuracyDeviation ? _trueOrientation : _orientation; } }

            /// <summary>
            /// Returns the survivor's state.
            /// It can be used for animation.
            /// </summary>
            public CharacterState state { get { return _state; } }

            /// <summary>
            /// Return the survivor's hand state.
            /// It can be used for animation.
            /// </summary>
            public HandState handState { get { return _handState; } }

            /// <summary>
            /// Return if the survivor is aiming or not
            /// </summary>
            public bool isAiming { get { return _isAiming; } }

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
            /// Reduce or increase the survivor's accuracy
            /// </summary>
            [SerializeField]
            private float _accuracyMultiplier = 0.5f;

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


            private float _verticalSpeed = 0;
            private Vector3 _speed;
            private Vector3 _orientation = Vector3.forward;

            private HandState _handState = HandState.Idle;
            private bool _isAiming = false;

            private Vector3 _trueOrientation = Vector3.forward;
            private Vector3 _lastOrientation = Vector3.forward;
            private Vector3 _nextOrientation = Vector3.forward;

            [SerializeField]
            private bool _accuracyDeviation = true;

            [SerializeField]
            private AnimationCurve _accuracyInterpolation;

            [SerializeField]
            private float _accuracyDeviationTime = 2;

            [SerializeField]
            private float _maxAccuracyDeviation = 20;
            [SerializeField]
            private float _minAccuracyDeviation = -20;

            [SerializeField]
            private Timer _accuracyTimer;

            private Quaternion _orientationQuaternion;

            public Quaternion orientationQuaternion { get { return _orientationQuaternion; } }

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            public void Start()
            {
                _orientation = Vector3.forward;

                _speed = Vector3.zero;

                if ( _accuracyDeviation )
                {
                    _accuracyTimer.init( _accuracyDeviationTime, startAccuracyRoutine, accuracyRoutine, endAccuracyRoutine, null );
                    _accuracyTimer.start();
                }
                

                if ( _controller != null ) return;

                _controller = GetComponent<CharacterController>();
            }

            public void Update()
            {
                move( _speed );
                applyGravity();

                _speed.x = 0;
                _speed.z = 0;
            }

            private void accuracyRoutine()
            {
                float factor = _accuracyInterpolation.Evaluate( _accuracyTimer.currentTime / _accuracyTimer.maxTime );

                Vector3 randomOrientation = Vector3.Normalize( ( factor * _nextOrientation + ( 1 - factor ) * _lastOrientation ) / 2 );
                float multiplier = 0.3f * _accuracyMultiplier * (_isAiming ? 0.2f : 1);
                _trueOrientation = Vector3.Normalize( ( multiplier * randomOrientation + (1-multiplier) * _orientation ) / 2 );

                Vector3 camPosition = GetComponentInChildren<Camera>().transform.position;
                Debug.DrawLine( camPosition, camPosition + 10 * _orientation );
                Debug.DrawLine( camPosition, camPosition + 10 * _trueOrientation, Color.blue );
            }

            private void startAccuracyRoutine()
            {
                float minRange = -20;
                float maxRange = 20;
                _lastOrientation = _nextOrientation;

                _nextOrientation = Quaternion.Euler( Random.Range( minRange, maxRange ), Random.Range( minRange, maxRange ), Random.Range( minRange, maxRange ) ) * Vector3.forward;
            }

            private void endAccuracyRoutine()
            {
                _accuracyTimer.start();
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
                _handState = HandState.Idle;
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
            public void fire(bool fire)
            {
                //_weapon.fire();
                _handState = fire ? HandState.Fire : HandState.Idle;
            }

            /// <summary>
            /// When the survivor is aiming, he's more accurate but he cannot sprint
            /// </summary>
            /// <param name="aiming">True or false</param>
            public void aim( bool aiming )
            {
                if ( _state != CharacterState.Sprint ) _isAiming = aiming;
            }

            /// <summary>
            /// The orientation vector is rotated around X & Y axis
            /// </summary>
            /// <param name="verticalOrientation">The new vertical angle in degrees</param>
            /// <param name="horizontalOrientation">The new horizontal angle in degrees</param>
            public void setOrientation( float verticalOrientation, float horizontalOrientation )
            {
                Quaternion quat = Quaternion.Euler( -verticalOrientation, horizontalOrientation, 0 );
                _orientationQuaternion = quat;
                _orientation = quat * Vector3.forward;
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
