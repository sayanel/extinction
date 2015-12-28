using UnityEngine;
using System.Collections;

namespace Extinction
{
    namespace Skills
    {
        public class Bombing : ActiveSkill
        {
            [SerializeField]
            private Texture2D _cursorTexture;

            [SerializeField]
            private Vector2 _cursorHotSpot = Vector2.zero;

            [SerializeField]
            private CursorMode _cursorLockMode = CursorMode.Auto;

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
                Cursor.SetCursor(_cursorTexture, _cursorHotSpot, _cursorLockMode);
            }

            public override void activate(Vector3 position)
            {
                StartCoroutine(bombingCoroutine(position));

                StartCoroutine(handleCooldown());
                Cursor.SetCursor(null, _cursorHotSpot, _cursorLockMode);
            }

            IEnumerator bombingCoroutine(Vector3 position)
            {
                for (int i = 0; i < _projectileCount; i++)
                {
                    Vector2 randomPositionOffset = Random.insideUnitCircle * _damageArea;
                    Vector3 instantiatedPosition = new Vector3(_launcher.position.x + randomPositionOffset.x, _launcher.position.y , _launcher.position.z + randomPositionOffset.y);
                    //activate the FX on network
                    FXManager.Activate(_launchingFX, instantiatedPosition, _launcher.rotation);

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