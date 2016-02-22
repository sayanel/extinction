using UnityEngine;
using System.Collections;

using Extinction.FX;
using Extinction.Enums;
using Extinction.Characters;
using Extinction.Herbie;

namespace Extinction
{
    namespace Skills
    {
        public class CreateRCamera : ActiveSkill
        {
            //a reference to herbie's fog manager needs to be stored to properly initialyze Rcameras.
            private FogManager _fogManager;

            [SerializeField]
            private GameObject _rCameraModel;

            /// <summary>
            /// Layer names of the objects which can block the visibility of the robot.
            /// Set to Terrain by default.
            /// </summary>   
            private string[] _terrainMasks = new string[] { "Terrain" };

            public override void beginActivation()
            {

            }

            public override void activate(Vector3 position)
            {
                GameObject rCamera = Instantiate(_rCameraModel, position, Quaternion.identity) as GameObject;
                //TODO : make the instantiation synchnized on the network

                rCamera.GetComponent<RCamera>().TerrainMasks = _terrainMasks;
                rCamera.GetComponent<RCamera>().FogManager = _fogManager;

                StartCoroutine(handleCooldown());
            }

            public override void init( SpecialRobot robot )
            {
                _terrainMasks = robot.TerrainMasks;
                _fogManager = robot.FogManager;
            }
        }
    }
}
