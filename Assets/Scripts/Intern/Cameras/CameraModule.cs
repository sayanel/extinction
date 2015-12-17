// @author : 

using UnityEngine;
using System.Collections;

namespace Extinction
{
    namespace Cameras
    {
        public abstract class CameraModule : MonoBehaviour
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            protected Vector3 _position;
            protected Vector3 _rotation;
            protected float _fieldOfView;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            public abstract void setPosition( Vector3 position );
            public abstract void setRotation( Vector3 rotation );
            public abstract void setFieldOfView( float fieldOfView );
            public abstract void zoom( float targetFieldOfView, float time );
        }
    }
}
