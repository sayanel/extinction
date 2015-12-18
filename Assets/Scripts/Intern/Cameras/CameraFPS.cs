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

            private Camera _camera;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            public void Update()
            {
                transform.LookAt( transform.position + _survivor.getOrientation(), Vector3.up );
            }

            public override void setFieldOfView( float fieldOfView )
            {
                throw new System.NotImplementedException();
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
        }
    }
}
