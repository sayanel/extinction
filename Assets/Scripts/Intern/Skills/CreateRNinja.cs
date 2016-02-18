// @author = florian

using UnityEngine;
using System.Collections;

using Extinction.FX;
using Extinction.Enums;
using Extinction.Characters;
using System;

namespace Extinction
{
    namespace Skills
    {
        public class CreateRNinja : ActiveSkill
        {
            [SerializeField]
            private GameObject _rNinjaModel;

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
                GameObject rNinja = Instantiate(_rNinjaModel, position, Quaternion.identity) as GameObject;
                //TODO : make the instantiation synchnized on the network
                rNinja.GetComponent<RNinja>().TerrainMasks = _terrainMasks;

                StartCoroutine(handleCooldown());
            }

            public override void init( SpecialRobot robot )
            {
                _terrainMasks = robot.TerrainMasks;
            }
        }
    }
}
