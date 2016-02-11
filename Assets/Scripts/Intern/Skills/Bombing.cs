using UnityEngine;
using System.Collections;

using Extinction.FX;
using Extinction.Enums;
using Extinction.Characters;
using Extinction.Weapons;

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
            private float _projectileSpeed = 0.1F;

            [SerializeField]
            private float _damageRadius = 3;

            [SerializeField]
            private float _damage = 10;

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
                    Vector2 randomPositionOffset = Random.insideUnitCircle;
                    Vector3 instantiatedPosition = new Vector3(_launcher.position.x + randomPositionOffset.x, _launcher.position.y , _launcher.position.z + randomPositionOffset.y);
                    
                    //activate the FX on network
                    FXManager.Instance.Activate(_launchingFX, instantiatedPosition, _launcher.rotation);

                    yield return new WaitForSeconds(Random.Range(0.1f, 0.8f));
                }

                yield return new WaitForSeconds(1);

                for(int i = 0; i < _projectileCount; i++)
                {
                    Vector2 randomPositionOffset = Random.insideUnitCircle * _damageRadius;
                    Vector3 instantiatedPosition = new Vector3(position.x + randomPositionOffset.x, position.y + _projectileHeight, position.z + randomPositionOffset.y);
                    
                    //TODO : make the instantiation synchonized on the network
                    GameObject newProjectile = Instantiate(_projectileModel, instantiatedPosition, Quaternion.LookRotation(new Vector3(0,-1,0))) as GameObject;
                    Missile missileComponent = newProjectile.GetComponent<Missile>();
                    if( missileComponent != null )
                    {
                        missileComponent.Speed = _projectileSpeed;
                        missileComponent.Damage = _damage;
                        missileComponent.DamageRadius = _damageRadius;
                    }

                    yield return new WaitForSeconds(Random.Range(0.1f, 0.8f));
                }
            }
        }
    }
}