// @author: Mehdi

using UnityEngine;
using System.Collections;
using Extinction.Weapons;
using Extinction.Enums;
using Extinction.Network;
using Extinction.Controllers;
using Extinction.Utils;
using Extinction.HUD;
using Extinction.Cameras;
using Extinction.Game;

namespace Extinction
{
    namespace Characters
    {
        /// <summary>
        /// This class represents a survivor.
        /// </summary>
        public class Survivor : Character, INetworkInitializerPrefab
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// Returns the survivor's orientation, i.e the vector which describes where he is looking at
            /// </summary>
            //public Vector3 orientation { get { return _orientation; } }

            /// <summary>
            /// Return the survivor's weapon animation state.
            /// It can be used for animation.
            /// </summary>
            public string currentWeaponAnimationState { get { return _currentWeaponAnimationState; } }

            /// <summary>
            /// Return if the survivor is aiming or not
            /// </summary>
            public bool isAiming { get { return _isAiming; } }

            /// <summary>
            /// Return if the survivor is aiming or not
            /// </summary>
            public bool isSprinting { get { return _isSprinting; } }

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
            /// A timer activated when the survivor reloads
            /// </summary>
            [SerializeField]
            private Timer _reloadTimer;

            /// <summary>
            /// A timer activated when the survivor dies
            /// </summary>
            [SerializeField]
            private Timer _dieTimer;

            [SerializeField]
            private float _dieAnimationTime = 2;

            /// <summary>
            /// Time spent by reloading action
            /// </summary>
            [SerializeField]
            private float _reloadTime = 2;

            /// <summary>
            /// A pointer to the weapon of the survivor
            /// </summary>
            [SerializeField]
            private Weapon _weapon;

            public Weapon weapon {get {return _weapon;}}

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
            //private Vector3 _orientation = Vector3.forward;

            private string _currentWeaponAnimationState = "Idle";

            [SerializeField]
            private Animator _weaponAnimator;

            private bool _isAiming = false;
            private bool _isSprinting = false;

            private Quaternion _orientationQuaternion;

            public Quaternion orientationQuaternion { get { return _orientationQuaternion; } }

            [SerializeField]
            private HUDLifeBar _lifeBar;

            [SerializeField]
            public Transform _anchorFPS_FX;

            [SerializeField]
            public Transform _anchorThird_FX;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            public void Start()
            {
                _speed = Vector3.zero;
                if(_lifeBar == null ){
                    _lifeBar = GetComponentInChildren<HUDLifeBar>();
                }
                _lifeBar.changeHealth( _health, _maxHealth );

                _reloadTimer.init( _reloadTime, null, null, idleWeapon, null );
                _dieTimer.init( _dieAnimationTime, null, null, switchToFreefly, null );

                if ( _controller != null ) return;

                _controller = GetComponent<CharacterController>();
            }

            public void Update()
            {
                move( _speed );
                applyGravity();

                _speed.x = 0;
                _speed.z = 0;

                _lifeBar.changeHealth( _health, _maxHealth );

                if (_health <= 0)
                    die();
            }

            /// <summary>
            /// Remove the amount of damage from survivor's health (Inherited from Character)
            /// </summary>
            /// <param name="amount">The health that will be removed</param>
            public override void getDamage( int amount )
            {
                float health = _health - amount * _armorMultiplier;
                GetComponent<PhotonView>().RPC("SetHealth", PhotonTargets.All, health);
            }

            public override void die()
            {
                setWeaponAnimationState( "Die" );
                setAnimationState( "Die" );
                GetComponent<InputControllerSurvivor>().enabled = false;
                GetComponentInChildren<CameraFPS>().useAnchor = true;
                GetComponentInChildren<SurvivorAnimationProcedural>().angleOffsetX = 0;
                GetComponentInChildren<SurvivorAnimationProcedural>().angleOffsetY = 0;
                GetComponentInChildren<SurvivorAnimationProcedural>().angleOffsetZ = 0;
                _dieTimer.start();
                GameManager.Instance.changeSurvivorStatus(_characterName, CharacterStatus.Dead);
            }

            public void switchToFreefly()
            {
                GetComponentInChildren<CameraFPS>().enabled = false;
                GetComponentInChildren<InputControllerFreeflyCamera>().enabled = true;
                GetComponent<SurvivorComponentActivator>().deadMode();
                //Game.GameManager.Instance.changeSurvivorStatus( _characterName, CharacterStatus.Dead );
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
                setAnimationState( "Idle" );
            }

            /// <summary>
            /// Change the player's current weapon animation state to idle
            /// </summary>
            public void idleWeapon()
            {
                setWeaponAnimationState( "Idle" );
            }

            /// <summary>
            /// Change the player's current state to strafeLeft
            /// Move the player left
            /// </summary>
            /// <param name="value"></param>
            public void strafeLeft( float value )
            {
                setAnimationState( "StrafeLeft" );
                horizontalMovement( value );
            }

            /// <summary>
            /// Change the player's current state to strafeRight
            /// Move the player right
            /// </summary>
            /// <param name="value"></param>
            public void strafeRight( float value )
            {
                setAnimationState( "StrafeRight" );
                horizontalMovement( value );
            }

            /// <summary>
            /// Change the player's current state to run
            /// Move the player front
            /// </summary>
            /// <param name="value"></param>
            public void run( float value )
            {
                setAnimationState( "Run" );
                verticalMovement( value );
            }

            /// <summary>
            /// Change the player's current state to runBackward
            /// Move the player back
            /// </summary>
            /// <param name="value"></param>
            public void runBackward( float value )
            {
                setAnimationState( "RunBackward" );
                verticalMovement( value );
            }

            /// <summary>
            /// Change the player's current state to sprint
            /// The player will move faster as long as Sprinting Key is pressed
            /// </summary>
            /// <param name="value"></param>
            public void sprint(bool sprinting)
            {
                _isSprinting = sprinting;
                setWeaponAnimationState( "Idle" );
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
                if ( _currentWeaponAnimationState == "Reload")
                    return;

                if ( _weapon.magazineEmpty ) 
                    fire = false;

                setWeaponAnimationState( fire ? "Fire" : "Idle" );

                if ( _weapon != null && fire )
                    _weapon.fire();
            }

            /// <summary>
            /// The survivor reloads his weapon
            /// </summary>
            public void reload( bool reload )
            {
                if ( _currentWeaponAnimationState == "Reload" || _weapon.magazineFull )
                    return;

                setWeaponAnimationState( reload ? "Reload" : "Idle" );

                if ( _weapon != null && reload )
                    _reloadTimer.start();
                    _weapon.reload(30);
            }

            /// <summary>
            /// When the survivor is aiming, he's more accurate but he cannot sprint
            /// </summary>
            /// <param name="aiming">True or false</param>
            public void aim( bool aiming )
            {
                if(!_isSprinting) _isAiming = aiming;
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
                float multiplier = _isSprinting ? 2 : 1;
                multiplier *= verticalValue;
                multiplier *= _defaultCharacterSpeed;
                multiplier *= _speedMultiplier;

                Vector3 movement = new Vector3( _orientation.x, 0, _orientation.z ) * multiplier * Time.deltaTime;

                _speed.x += movement.x;
                _speed.z += movement.z;
            }

            /// <summary>
            /// Use this function to play an animation with name : stateName.
            /// By default, it uses trigger animator value. Override the function to use different value type.
            /// </summary>
            /// <param name="stateName"></param>
            public override void setAnimationState( string stateName )
            {
                if ( _animator == null || stateName.Equals( _currentAnimationState ) ) {
                    return;
                }
                _currentAnimationState = stateName;
                _animator.SetTrigger( stateName );
            }

            /// <summary>
            /// Same as setAnimationState but for weapons
            /// </summary>
            /// <param name="stateName"></param>
            public void setWeaponAnimationState( string stateName )
            {
                if ( _weaponAnimator == null || stateName.Equals(_currentWeaponAnimationState) )
                    return;

                _currentWeaponAnimationState = stateName;
                _weaponAnimator.ResetTrigger("Fire");
                _weaponAnimator.SetTrigger( stateName );
            }

            /// <summary>
            /// Use this function to change animation played by this character.
            /// By default, it uses trigger animator value. Override the function to use different value type.
            /// </summary>
            /// <param name="oldState"></param>
            /// <param name="newState"></param>
            public override void changeAnimationState( string oldState, string newState )
            {
                _currentAnimationState = newState;

                if ( _animator != null )
                    _animator.SetTrigger( newState );
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

            // INetworkInitializerPrefab method
            public void Activate() {
                GetComponent<SurvivorComponentActivator>().firstPersonMode();
            }
        }
    }
}
