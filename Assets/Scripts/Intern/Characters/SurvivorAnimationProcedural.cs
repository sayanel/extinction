// @author : mehdi-antoine

using UnityEngine;
using System.Collections;

namespace Extinction {
    namespace Characters
    {
        public class SurvivorAnimationProcedural : MonoBehaviour
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            [SerializeField]
            private Survivor _survivor;
            
            [SerializeField]
            private float _angleOffsetX = 0;
            [SerializeField]
            private float _angleOffsetY = 60;
            [SerializeField]
            private float _angleOffsetZ = 0;

            public float angleOffsetX { get { return _angleOffsetX; } set { _angleOffsetX = value; } }
            public float angleOffsetY { get { return _angleOffsetY; } set { _angleOffsetY = value; } }
            public float angleOffsetZ { get { return _angleOffsetZ; } set { _angleOffsetZ = value; } }

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            public void LateUpdate()
            {
                transform.LookAt( transform.position + _survivor.orientation, Vector3.up );
                transform.Rotate( Vector3.up, _angleOffsetY );
                transform.Rotate( Vector3.right, _angleOffsetX );
                transform.Rotate( Vector3.forward, _angleOffsetZ );
            }

        }
    }
}

