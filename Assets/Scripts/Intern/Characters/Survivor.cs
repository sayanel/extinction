// @author: Mehdi

using UnityEngine;
using System.Collections;
using Extinction.Weapons;

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
                //_controller.Move( _speed );
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
                _controller.Move( vec /** Time.deltaTime*/ );
            }

            public void setOrientation( float verticalOrientation, float horizontalOrientation )
            {
                _orientation = Quaternion.Euler( -verticalOrientation, horizontalOrientation, 0 ) * Vector3.forward;
            }

            public override void turn( float angle )
            {
                transform.Rotate( Vector3.up, angle );
            }

            public override void activateSkill1()
            {
                throw new System.NotImplementedException();
            }

            public override void activateSkill2()
            {
                throw new System.NotImplementedException();
            }

            public void horizontalMovement( float horizontalValue )
            {
                
                Vector3 movement = new Vector3( _orientation.z, 0, - _orientation.x ) * horizontalValue * _defaultCharacterSpeed * _speedMultiplier * Time.deltaTime;

                _speed.x += movement.x;
                _speed.z += movement.z;
            }

            public void verticalMovement( float verticalValue )
            {
                Vector3 movement = new Vector3( _orientation.x, 0, _orientation.z ) * verticalValue * _defaultCharacterSpeed * _speedMultiplier * Time.deltaTime;

                _speed.x += movement.x;
                _speed.z += movement.z;
            }

            public void jump()
            {
                if ( !_controller.isGrounded ) return;

                _speed.y = _jumpImpulse * Time.deltaTime;
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

            public void fire()
            {
                _weapon.fire();
            }

            public void aim( bool aiming )
            {
                _aiming = aiming;
            }
        }
    }
}
