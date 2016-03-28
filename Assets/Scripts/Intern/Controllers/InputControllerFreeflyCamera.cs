// @author : mehdi
using UnityEngine;
using System.Collections;

namespace Extinction
{
    namespace Controllers
    {
        public class InputControllerFreeflyCamera : InputController
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            [SerializeField]
            private Transform _cameraTransform;
            private Camera _camera;

            [SerializeField]
            private float _mouseSensitivity = 5;

            [SerializeField]
            private float _maximumVerticalRotation = 60;

            private float _horizontalTranslation;
            private float _verticalTranslation;

            private float _horizontalRotation;
            private float _verticalRotation;

            private Quaternion _orientationQuaternion;
            private Vector3 _orientation = Vector3.forward;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            void Update()
            {
                processUserInputs();
            }

            public override void processUserInputs()
            {
                _verticalTranslation = Input.GetAxis( "Vertical" );
                _horizontalTranslation = Input.GetAxis( "Horizontal" );
                float mouseX = Input.GetAxis( "Mouse X" ) * _mouseSensitivity;
                float mouseY = Input.GetAxis( "Mouse Y" ) * _mouseSensitivity;

                _horizontalRotation += mouseX;
                _verticalRotation += mouseY;

                _horizontalRotation = Mathf.Repeat( _horizontalRotation, 360 );
                _verticalRotation = Mathf.Clamp( _verticalRotation, -_maximumVerticalRotation, _maximumVerticalRotation );

                Quaternion quat = Quaternion.Euler( -_verticalRotation, _horizontalRotation, 0 );
                _orientationQuaternion = quat;
                _orientation = quat * Vector3.forward;

                transform.position = transform.position + _verticalTranslation * _orientation;
                transform.position = transform.position + new Vector3( _orientation.z, 0, -_orientation.x ) * _horizontalTranslation;

                transform.LookAt( transform.position + _orientation, Vector3.up );
            }

        }
    }
}

