// @author: Mehdi

using UnityEngine;
using System.Collections;
using Extinction.Weapons;
using Extinction.Enums;

namespace Extinction
{
    namespace Characters
    {
        public class Survivor : Character
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            private float _lifeMultiplier = 1;
            private float _stealthMultiplier = 1;
            private float _damageMultiplier = 1;
            private float _armorMultiplier = 1;

            [SerializeField]
            private float _speedMultiplier = 3;

            [SerializeField]
            private CharacterController _controller;

            [SerializeField]
            private float _gravity = 0.2f;

            [SerializeField]
            private float _jumpImpulse = 0.4f;

            private float _verticalSpeed = 0;

            private Vector3 _speed;
            private Vector3 _orientation = Vector3.forward;

            [SerializeField]
            private Weapon _weapon;

            private bool _aiming = false;

            public bool isAiming { get { return _aiming; } }

            public Vector3 orientation { get { return _orientation; } }

            public CharacterState state { get { return _state; } }

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
            }

            public override void getDamage( int amount )
            {
                _health -= amount * _armorMultiplier;
            }

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

            public void idle()
            {
                _state = CharacterState.Idle;
            }

            public void strafeLeft( float value )
            {
                _state = CharacterState.StrafeLeft;
                horizontalMovement( value );
            }

            public void strafeRight( float value )
            {
                _state = CharacterState.StrafeRight;
                horizontalMovement( value );
            }

            public void run( float value )
            {
                _state = CharacterState.Run;
                verticalMovement( value );
            }

            public void runBackward( float value )
            {
                _state = CharacterState.RunBackward;
                verticalMovement( value );
            }

            public void jump()
            {
                if ( !_controller.isGrounded ) return;

                _speed.y = _jumpImpulse * Time.deltaTime;
            }

            public void fire()
            {
                _weapon.fire();
            }

            public void aim( bool aiming )
            {
                _aiming = aiming;
            }

            public void setOrientation( float verticalOrientation, float horizontalOrientation )
            {
                _orientation = Quaternion.Euler( -verticalOrientation, horizontalOrientation, 0 ) * Vector3.forward;
            }

            public override void turn( float angle )
            {
                transform.Rotate( Vector3.up, angle );
            }

            private void horizontalMovement( float horizontalValue )
            {
                Vector3 movement = new Vector3( _orientation.z, 0, -_orientation.x ) * horizontalValue * _defaultCharacterSpeed * _speedMultiplier * Time.deltaTime;

                _speed.x += movement.x;
                _speed.z += movement.z;
            }
            private void verticalMovement( float verticalValue )
            {
                Vector3 movement = new Vector3( _orientation.x, 0, _orientation.z ) * verticalValue * _defaultCharacterSpeed * _speedMultiplier * Time.deltaTime;

                _speed.x += movement.x;
                _speed.z += movement.z;
            }

            public void applyGravity()
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
