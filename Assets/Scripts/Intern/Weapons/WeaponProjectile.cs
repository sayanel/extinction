// @author : Pascale, florian

using UnityEngine;
using System.Collections;

namespace Extinction
{
    namespace Weapons
    {

        public class WeaponProjectile : Weapon
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// The model of projectile this weapon uses.
            /// </summary>
            [SerializeField]
            protected Projectile _projectile;

            /// <summary>
            /// The velocity of the projectile.
            /// </summary>
            [SerializeField]
            protected float _velocity = 10f;

            /// <summary>
            /// The position from which the projectile will be launched.
            /// If the anchor isn't set in the editor, it will automatically be the child transform of this gameObject.
            /// If there is no child transform, it will be the transform of this gameObject.
            /// </summary>
            [SerializeField]
            protected Transform _anchor;

            /// <summary>
            /// The targetTags will be given to instanced projectile when shooting.
            /// Only GameObjects with on of these tags can be hit by the projectile.
            /// </summary>
            protected string[] _targetTag;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------
            public void Start()
            {
                _previousTime = Time.time;

                if( _anchor == null ){
                    if(transform.childCount > 0){
                        _anchor = transform.GetChild( 0 );
                    }
                    else{
                        _anchor = transform;
                    }
                }
            }

            override
            public void fire()
            {
                if (Time.time - _previousTime >= _fireRate)
                {
                    Projectile p = Instantiate(_projectile, _anchor.position, _anchor.rotation) as Projectile;
                    p.Dammage = _damage;
                    p.TargetTag = _targetTag;
                    Rigidbody pBody = p.GetComponent<Rigidbody>();
                    if (pBody != null)
                        pBody.AddForce(_anchor.forward * _velocity);
                    _previousTime = Time.time;
                    _nbCurrentAmmo--;
                }
            }

            override
            public void reload(int ammo)
            {
                int nb = _magazineMaxCapacity - _nbCurrentAmmo;
                _nbCurrentAmmo += (ammo > nb) ? nb : ammo;
            }
        
        }
    }
}