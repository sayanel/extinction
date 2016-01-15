using UnityEngine;
using System.Collections;

using Extinction.FX;
using Extinction.Enums;
using Extinction.Characters;

namespace Extinction
{
    namespace Skills
    {
        public class Bombing : ActiveSkill
        {

            [SerializeField]
            private GameObject _projectileModel;

            [SerializeField]
            private int _projectileCount = 5;

            [SerializeField]
            private float _projectileHeight = 10;

            [SerializeField]
            private float _damageArea = 3;

            [SerializeField]
            private Transform _launcher;

            [SerializeField]
            private FXType _launchingFX;

            public override void beginActivation()
            {

            }

            public override void activate(Vector3 position)
            {
                StartCoroutine(bombingCoroutine(position));

                StartCoroutine(handleCooldown());
            }

            IEnumerator bombingCoroutine(Vector3 position)
            {
                for (int i = 0; i < _projectileCount; i++)
                {
                    Vector2 randomPositionOffset = Random.insideUnitCircle * _damageArea;
                    Vector3 instantiatedPosition = new Vector3(_launcher.position.x + randomPositionOffset.x, _launcher.position.y , _launcher.position.z + randomPositionOffset.y);
                    //activate the FX on network
                    FXManager.Instance.Activate(_launchingFX, instantiatedPosition, _launcher.rotation);

                    yield return new WaitForSeconds(Random.Range(0.1f, 0.8f));
                }

                yield return new WaitForSeconds(1);

                for(int i = 0; i < _projectileCount; i++)
                {
                    Vector2 randomPositionOffset = Random.insideUnitCircle * _damageArea;
                    Vector3 instantiatedPosition = new Vector3(position.x + randomPositionOffset.x, position.y + _projectileHeight, position.z + randomPositionOffset.y);
                    //TODO : make the instantiation synchonized on the network
                    Instantiate(_projectileModel, instantiatedPosition, Quaternion.identity);

                    yield return new WaitForSeconds(Random.Range(0.1f, 0.8f));
                }
            }
        }
    }
}