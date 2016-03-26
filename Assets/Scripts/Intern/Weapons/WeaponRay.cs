// @author : Pascale, florian

using UnityEngine;
using System.Collections;

using Extinction.Characters;
using Extinction.FX;

namespace Extinction
{
    namespace Weapons
    {

        public class WeaponRay : Weapon, IHitTarget
        {

            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// The ray will be casted from _anchor.position + _minDistance.
            /// </summary>
            [SerializeField]
            protected float _minDistance = 1.0f;

            /// <summary>
            /// The extremity of the ray. Automatically deduced with _minDistance and _rayLength.
            /// </summary>
            protected float _maxDistance = 100f;

            /// <summary>
            /// The length of the ray. Automatically deduced with the range of this weapon.
            /// </summary>
            protected float _rayLenght = 100f;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            public void Start()
            {
                _previousTime = Time.time;

                _rayLenght = _range;
                _maxDistance = _rayLenght + _minDistance;

                if( _anchor == null ){
                    if( transform.childCount > 0 ){
                        _anchor = transform.GetChild( 0 );
                    }
                    else{
                        _anchor = transform;
                    }
                }

                if(_anchorFX == null ){
                    _anchorFX = _anchor;
                }
            }

            override
            public void fire() {

                if (canShoot())
                {
                    Debug.DrawRay(_anchor.position + _anchor.forward * _minDistance, _anchor.forward*100, Color.red, 10, false);
                    //fire ray
                    RaycastHit hitInfo;
                    if (Physics.Raycast(_anchor.position + _anchor.forward * _minDistance,
                                        _anchor.forward,
                                        out hitInfo,
                                        _rayLenght,
                                        LayerMask.GetMask(_targetLayer))) {
                        GameObject target = hitInfo.transform.gameObject;
                        onHit(target);
                    }
                    _nbCurrentAmmo--;
                    _previousTime = Time.time;

                    //launch FX
                    FXManager.Instance.Activate((int)Enums.FXType.ShootFX, _anchorFX.position, _anchorFX.rotation);

                }
            }

            override 
            public bool canShoot()
            {
                return ( ( Time.time - _previousTime >= _fireRate ) && //the fireRate is OK ?
                    ( ( _useAmmo && _nbCurrentAmmo > 0 ) || !_useAmmo ) ); //the ammos are OK ?
            }

            public void onHit(GameObject obj)
            {
                Character o = obj.GetComponent<Character>();
                o.getDamage(_damage);
            }

            override
            public void reload(int ammo)
            {
                //number of ammo we can to put on the magazine : 
                int nb = _magazineMaxCapacity - _nbCurrentAmmo;

                //launch FX
                FXManager.Instance.Activate((int)Enums.FXType.ReloadFX, _anchorFX.position, _anchorFX.rotation);

                _nbCurrentAmmo += (ammo > nb) ? nb : ammo;
            }
        }
    }
}