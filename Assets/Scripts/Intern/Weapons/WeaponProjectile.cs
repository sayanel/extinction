// @author : Pascale

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
            protected Projectile _projectile;
            protected float _velocity = 10f;
            protected Transform _anchor;
            protected string[] _targetTag;
            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------
            public void Start()
            {
                _previousTime = Time.time;
                if(_anchor == null && transform.childCount > 0){
                    _anchor = transform.GetChild(0);
                }
            }

            override
            public void fire()
            {
                if (Time.time - _previousTime >= _fireRate)
                {
                    Projectile p = Instantiate(_projectile, _anchor.position, _anchor.rotation) as Projectile;
                    p.Dammage = _dammage;
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