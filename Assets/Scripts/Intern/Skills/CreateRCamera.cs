using UnityEngine;
using System.Collections;

using Extinction.FX;
using Extinction.Enums;
using Extinction.Characters;

namespace Extinction
{
    namespace Skills
    {
        public class CreateRCamera : ActiveSkill
        {
            [SerializeField]
            private GameObject _rCameraModel;

            public override void beginActivation()
            {

            }

            public override void activate(Vector3 position)
            {
                Instantiate(_rCameraModel, position, Quaternion.identity);
                //TODO : make the instantiation synchnized on the network

                StartCoroutine(handleCooldown());
            }
        }
    }
}
