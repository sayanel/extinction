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

                _survivor.aim(Input.GetMouseButton(1) ? true:false);

                if ( Input.GetMouseButton( 0 ) )
                {
                    _survivor.fire();
                }
            }

            public void movement()
            {
                if ( Input.GetButton( "Jump" ) )
                {
                    _survivor.jump();
                }
            }

            public void translate()
            {
                _horizontalTranslation = Input.GetAxis( "Horizontal" );
                _verticalTranslation = Input.GetAxis( "Vertical" );

                if ( !Mathf.Approximately( 0, _horizontalTranslation ) )
                    _survivor.horizontalMovement( _horizontalTranslation );

                if ( !Mathf.Approximately( 0, _verticalTranslation ) )
                    _survivor.verticalMovement( _verticalTranslation );
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
