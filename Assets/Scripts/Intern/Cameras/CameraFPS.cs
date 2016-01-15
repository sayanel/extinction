// @author : Mehdit

using UnityEngine;
using System.Collections;
using Extinction.Characters;
using Extinction.Utils;

namespace Extinction
{
    namespace Cameras
    {
        /// <summary>
        /// This class controls a Unity Camera. 
        /// It uses a Survivor Component to update it's position, rotation & FOV
        /// </summary>
        public class CameraFPS : CameraModule
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// pointer to the survivor
            /// </summary>
            [SerializeField]
            private Survivor _survivor;

            /// <summary>
            /// pointer to the unity Camera
            /// </summary>
            [SerializeField]
            private Camera _camera;

            /// <summary>
            /// The camera is set to _normalFOV when the survivor is not aiming
            /// </summary>
            [SerializeField]
            private float _normalFOV = 60;

            /// <summary>
            /// The camera is set to _accurateFOV when the survivor is aiming
            /// </summary>
            [SerializeField]
            private float _accurateFOV = 30;

            
            /// <summary>
            /// When the observed Survivor starts to aimer, this timer is launched to handle
            /// interpolation from _normalFOV to _accurateFOV
            /// </summary>
            [SerializeField]
            private Timer _timer;

            /// <summary>
            /// Time of the interpolation
            /// </summary>
            [SerializeField]
            private float _zoomTime = 1;

            private float _currentFOV;

            private float _targetFOV;
            private float _triggerFOV;

            private bool _zoomingIn;
            private bool _zoomingOut;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            public void Start()
            {
                if ( _camera == null ) _camera = GetComponent<Camera>();

                _zoomingIn = false;
                _zoomingOut = false;

                _targetFOV = 0;
                _triggerFOV = 0;

                _camera.fieldOfView = _normalFOV;
                _currentFOV = _normalFOV;
            }

            public void Update()
            {
                transform.LookAt( transform.position + _survivor.orientation, Vector3.up );

                if ( _survivor.isAiming && !_zoomingIn )
                {
                    _zoomingOut = false;
                    _zoomingIn = true;
                    float time = ((_accurateFOV - _currentFOV)/(_accurateFOV - _normalFOV)) * _zoomTime;
                    zoom( _accurateFOV, time );
                }

                if ( !_survivor.isAiming && ! _zoomingOut )
                {
                    _zoomingIn = false;
                    _zoomingOut = true;
                    float time = ( ( _normalFOV - _currentFOV ) / ( _normalFOV - _accurateFOV ) ) * _zoomTime;
                    zoom( _normalFOV, time );
                }

            }

            /// <summary>
            /// Set the field of view of the Unity Camera
            /// </summary>
            /// <param name="fieldOfView"></param>
            public override void setFieldOfView( float fieldOfView )
            {
                _camera.fieldOfView = fieldOfView;
            }

            /// <summary>
            /// Set the local position of the Unity Camera
            /// </summary>
            /// <param name="position">The new position of the Camera</param>
            public override void setPosition( Vector3 position )
            {
                transform.localPosition = position;
            }

            /// <summary>
            /// Inherited from CameraModule & Not Implemented for FPS Camera
            /// </summary>
            /// <param name="rotation"></param>
            public override void setRotation( Vector3 rotation )
            {
                throw new System.NotImplementedException();
            }

            /// <summary>
            /// Zoom from the current FOV to a target FOV with time interpolation 
            /// </summary>
            /// <param name="targetFieldOfView">The target FOV</param>
            /// <param name="time">The time of the interpolation</param>
            public override void zoom( float targetFieldOfView, float time )
            {
                _targetFOV = targetFieldOfView;
                _triggerFOV = _currentFOV;
                _timer.init( time, null, zoomRoutine, null, null );
                _timer.start();
            }

            private void zoomRoutine()
            {
                _currentFOV = Mathf.Lerp( _targetFOV, _triggerFOV, ( _timer.maxTime - _timer.currentTime ) / _timer.maxTime );
                setFieldOfView( _currentFOV );
            }
        }   
    }
}
