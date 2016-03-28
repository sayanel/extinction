// @author : Pascale, florian

using UnityEngine;
using System.Collections;

using Extinction.Enums;

namespace Extinction
{
    namespace Weapons
    {
        public abstract class Weapon : MonoBehaviour
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// Max number of ammo that can be stocked in the weapon.
            /// </summary>
            [SerializeField]
            protected int _magazineMaxCapacity = 1;

            public int magazineMaxCapacity { get { return _magazineMaxCapacity; } }
            
            /// <summary>
            /// Current number of ammo in the weapon.
            /// </summary>
            [SerializeField]
            protected int _nbCurrentAmmo = 0;

            public int nbCurrentAmmo { get { return _nbCurrentAmmo; } }

            public bool magazineEmpty { get { return _nbCurrentAmmo <= 0; } }
            public bool magazineFull { get { return _nbCurrentAmmo >= _magazineMaxCapacity; } }

            /// <summary>
            /// True if the weapon repeteadly fires while the fire button is hold.
            /// </summary>
            [SerializeField]
            protected bool _isAutomatic = false;
            
            /// <summary>
            /// Time between two shoots.
            /// </summary>
            [SerializeField]
            protected float _fireRate = 1.0f;

            /// <summary>
            /// The dammage of each bullet/projectile.
            /// </summary>
            [SerializeField]
            protected int _damage = 1;

            /// <summary>
            /// True if we can deal damage by shooting our teammates.
            /// </summary>
            [SerializeField]
            protected bool _friendlyFire = true;

            /// <summary>
            /// The range of the weapon.
            /// </summary>
            [SerializeField]
            protected float _range = 1;

            public float Range{
                get{ return _range; }
            }

            /// <summary>
            /// The last time we have shot with this weapon.
            /// </summary>
            protected float _previousTime;

            /// <summary>
            /// The position from which the ray/projectile will be launched.
            /// If the anchor isn't set in the editor, it will automatically be the child transform of this gameObject.
            /// If there is no child transform, it will be the transform of this gameObject.
            /// </summary>
            [SerializeField]
            protected Transform _anchor;

            [SerializeField]
            protected Transform _anchorFX;

            /// <summary>
            /// Only GameObjects which are on these layers can be hit by the ray.
            /// Contains Default layer by default.
            /// </summary>
            [SerializeField]
            protected string[] _targetLayer = { "Default" };

            protected string[] TargetLayer
            {
                get { return _targetLayer; }
                set { _targetLayer = value; }
            }

            /// <summary>
            /// Is this weapon consuming ammo when it is shooting.
            /// </summary>
            [SerializeField]
            protected bool _useAmmo = true;


            /// <summary>
            /// The FX launched when we fire with this weapon.
            /// </summary>
            [SerializeField]
            protected FXType _fireFX;


            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// Reaload the weapon.
            /// </summary>
            /// <param name="ammo">Quantity of ammo</param>
            public abstract void reload(int ammo);

            /// <summary>
            /// Fire with the weapon.
            /// </summary>
            public abstract void fire();

            /// <summary>
            /// check if the weapon can shoot.
            /// </summary>
            /// <returns></returns>
            public abstract bool canShoot();
        }
    }
}
