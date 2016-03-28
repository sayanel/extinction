// @author: Mehdi-Antoine

using UnityEngine;
using System.Collections;
using Extinction.Characters;

namespace Extinction
{
    namespace Controllers
    {
        public class InputControllerSurvivor : InputController
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            [SerializeField]
            private Survivor _survivor;

            [SerializeField]
            private float _mouseSensitivity = 5;

            [SerializeField]
            private float _maximumVerticalRotation = 60;

            private float _horizontalTranslation;
            private float _verticalTranslation;

            private float _horizontalRotation;
            private float _verticalRotation;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            public override void processUserInputs()
            {
                translate();
                rotate();
                movement();
                weapon();
            }

            public void weapon()
            {
                _survivor.aim(Input.GetMouseButton(1));

                _survivor.fire( Input.GetMouseButton( 0 ) );

                if ( Input.GetKey( KeyCode.R ) )
                {
                    _survivor.reload( true );
                }

                if ( Input.GetKey( KeyCode.K ) )
                {
                    _survivor.die();
                }

                if ( Input.GetKey( KeyCode.H ) )
                {
                    _survivor.getDamage(5);
                }
            }

            public void movement()
            {
                if ( Input.GetButton( "Jump" ) )
                {
                    _survivor.jump();
                }

                if ( Input.GetKey( KeyCode.LeftShift ))
                {
                    _survivor.sprint(true);
                }

                if ( Input.GetKeyUp( KeyCode.LeftShift ) )
                {
                    _survivor.sprint(false);
                }
            }

            public void translate()
            {
                _horizontalTranslation = Input.GetAxis( "Horizontal" );
                _verticalTranslation = Input.GetAxis( "Vertical" );

                if ( _horizontalTranslation < 0 )
                {
                    _survivor.strafeLeft( _horizontalTranslation );
                }

                if ( _horizontalTranslation > 0 )
                {
                    _survivor.strafeRight( _horizontalTranslation );
                }

                if ( _verticalTranslation < 0 )
                {
                    _survivor.runBackward( _verticalTranslation );
                }

                if ( _verticalTranslation > 0 )
                {
                    _survivor.run( _verticalTranslation );
                }

                if ( Mathf.Approximately( 0, _horizontalTranslation ) && Mathf.Approximately( 0, _verticalTranslation ) )
                    _survivor.idle();
            }

            public void rotate()
            {
                float mouseX = Input.GetAxis( "Mouse X" ) * _mouseSensitivity;
                float mouseY = Input.GetAxis( "Mouse Y" ) * _mouseSensitivity;

                _horizontalRotation += mouseX;
                _verticalRotation += mouseY;

                _horizontalRotation = Mathf.Repeat( _horizontalRotation, 360 );
                _verticalRotation = Mathf.Clamp( _verticalRotation, -_maximumVerticalRotation, _maximumVerticalRotation );
                
                _survivor.setOrientation( _verticalRotation, _horizontalRotation );
                _survivor.turn( mouseX );
            }

            public void Awake()
            {
                if ( _survivor != null ) return;
                _survivor = GetComponent<Survivor>();
            }

            public void Update()
            {
                processUserInputs();
            }


        }
    }
}
