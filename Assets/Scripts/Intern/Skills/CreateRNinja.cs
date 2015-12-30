// @author = florian

using UnityEngine;
using System.Collections;

using Extinction.FX;
using Extinction.Enums;
using Extinction.Characters;

namespace Extinction
{
    namespace Skills
    {
        public class CreateRNinja : ActiveSkill
        {
            [SerializeField]
            private GameObject _rNinjaModel;

            public override void beginActivation()
            {

            }

            public override void activate(Vector3 position)
            {
                Instantiate(_rNinjaModel, position, Quaternion.identity);
                //TODO : make the instantiation synchnized on the network

                StartCoroutine(handleCooldown());
            }
        }
    }
}
