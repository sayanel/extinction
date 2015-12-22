// @author : Pascale

using UnityEngine;
using System.Collections;
using Extinction.Characters;

namespace Extinction
{
    namespace Weapons
    {

        public class WeaponRay : Weapon, IHitTarget
        {

            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------
            protected float _minDistance = 1.0f;
            protected float _maxDistance = 100f;
            protected float _rayLenght = 100f;
            protected Transform _anchor;
            protected string[] _targetTag;


            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------
            public void Start()
            {
                _previousTime = Time.time;
                _maxDistance = _rayLenght + _minDistance;
                if (_anchor == null && transform.childCount > 0)
                {
                    _anchor = transform.GetChild(0);
                }
            }

            override
            public void fire() {
                if (Time.time - _previousTime >= _fireRate)
                {
                    RaycastHit hitInfo;
                    if(Physics.Raycast(_anchor.position + _anchor.forward * _minDistance, 
                                        _anchor.forward,
                                        out hitInfo,
                                        _rayLenght,
                                        LayerMask.GetMask(_targetTag))){
                        GameObject target = hitInfo.transform.gameObject;
                        onHit(target);
                    }
                    _nbCurrentAmmo--;
                }
                _previousTime = Time.time;
            }

            public void onHit(GameObject obj)
            {
                Character o = obj.GetComponent<Character>();
                o.getDamage(_dammage);
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