using UnityEngine;
using System.Collections;

namespace Extinction
{
    namespace Skills
    {
        public class MachineGun : ActiveSkill
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
            private int _bulletCount = 20;

            [SerializeField]
            private float _damageArea = 10;

            [SerializeField]
            private float _offsetFromCharacter;

            [SerializeField]
            private FXType _fireFX;

            [SerializeField]
            private string[] _damageFilter;

            [SerializeField]
            private float _damage = 10;

            public override void beginActivation()
            {
                //immediatly activate the skill
                activate(Vector3.zero);
            }

            public override void activate(Vector3 position)
            {
                StartCoroutine(fireCoroutine(position));

                StartCoroutine(handleCooldown());
                Cursor.SetCursor(null, _cursorHotSpot, _cursorLockMode);
            }

            IEnumerator fireCoroutine(Vector3 position)
            {
                for (int i = 0; i < _bulletCount; i++)
                {
                    Vector2 fireDirection = Random.insideUnitCircle.normalized;
                    Vector2 fireOffset = fireDirection * _offsetFromCharacter;
                    Vector3 instantiatedPosition = new Vector3(position.x + fireOffset.x, position.y, position.z + fireOffset.y);

                    //activates FX on network
                    FXManager.Activate(_fireFX, instantiatedPosition, Quaternion.LookRotation(fireDirection, Vector3.up));

                    //deals damage to hit characters 
                    RaycastHit hitInfo;
                    if(Physics.Raycast(transform.position + new Vector3(fireOffset.x, 0, fireOffset.y), fireDirection, out hitInfo, _damageArea, LayerMask.GetMask(_damageFilter)))
                    {
                        Character hitCharacter = hitInfo.transform.GetComponent<Character>();
                        hitCharacter.getDamage();
                    }

                    yield return new WaitForSeconds(Random.Range(0.1f, 0.8f));
                }
            }
        }
    }
}