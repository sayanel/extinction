// @author : Mehdit

using UnityEngine;
using System.Collections;
using Extinction.Characters;
using Extinction.Utils;

namespace Extinction
{
    namespace Cameras
    {
        public class CameraFPS : CameraModule
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            [SerializeField]
            private Survivor _survivor;

            [SerializeField]
            private Camera _camera;

            private float _currentFOV;

            [SerializeField]
            private float _normalFOV = 60;
            [SerializeField]
            private float _accurateFOV = 30;
            [SerializeField]
            private float _zoomTime = 1;
            [SerializeField]
            private Timer _timer;

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

            public override void setFieldOfView( float fieldOfView )
            {
                _camera.fieldOfView = fieldOfView;
            }

            public override void setPosition( Vector3 position )
            {
                throw new System.NotImplementedException();
            }

            public override void setRotation( Vector3 rotation )
            {
                throw new System.NotImplementedException();
            }

            public override void zoom( float targetFieldOfView, float time )
            {
                _targetFOV = targetFieldOfView;
                _triggerFOV = _currentFOV;
                _timer.init( time, null, zoomRoutine, null, null );
                _timer.start();
            }

            public void zoomRoutine()
            {
                _currentFOV = Mathf.Lerp( _targetFOV, _triggerFOV, ( _timer.maxTime - _timer.currentTime ) / _timer.maxTime );
                setFieldOfView( _currentFOV );
            }
        }   
    }
}
