// @author : Mehdit

using UnityEngine;
using System.Collections;

using Extinction.Characters;

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
            public float _normalFOV = 60;
            [SerializeField]
            public float _accurateFOV = 30;

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
                transform.LookAt( transform.position + _survivor.getOrientation(), Vector3.up );
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
