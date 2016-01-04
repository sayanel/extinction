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

            private bool _zoomingIn = false;
            private bool _zoomingOut = false;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            public void Start()
            {
                if ( _camera == null ) _camera = GetComponent<Camera>();

                _camera.fieldOfView = _normalFOV;
                _currentFOV = _normalFOV;
            }

            public void Update()
            {
                transform.LookAt( transform.position + _survivor.orientation, Vector3.up );

                if ( _survivor.isAiming && _currentFOV > _accurateFOV && ! _zoomingIn )
                {
                    float time = (_currentFOV - _accurateFOV) / (_normalFOV - _accurateFOV) * _zoomTime;
                    Debug.Log( "Zoom!!!" );
                    _timer = new Timer( time, null, beubeu, null, null );
                    _timer.Start();
                }
            }

            public void beubeu()
            {
                Debug.Log( "haaa" );
            }

            public override void setFieldOfView( float fieldOfView )
            {
                _fieldOfView = fieldOfView;
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
                throw new System.NotImplementedException();
            }

            public void zoomIn()
            {

            }

            public void zoomOut()
            {

            }
            
        }
    }
}
