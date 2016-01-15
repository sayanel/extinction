// @author : Pascale, florian

using UnityEngine;
using System.Collections;

using Extinction.FX;

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
                if((Time.time - _previousTime >= _fireRate) && //the fireRate is OK ?
                    ( (_useAmmo && _nbCurrentAmmo > 0) || !_useAmmo)) //the ammos are OK ?
                {
                    //fire projectile
                    Projectile p = Instantiate(_projectile, _anchor.position, _anchor.rotation) as Projectile;
                    p.Dammage = _damage;
                    p.TargetTag = _targetLayer;
                    Rigidbody pBody = p.GetComponent<Rigidbody>();
                    if (pBody != null)
                        pBody.AddForce(_anchor.forward * _velocity);
                    _previousTime = Time.time;
                    _nbCurrentAmmo--;

                    //launch FX
                    FXManager.Instance.Activate(_fireFX, _anchor.position, _anchor.rotation);
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