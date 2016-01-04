﻿// @author : Pascale, florian

using UnityEngine;
using System.Collections;

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
            protected int _magazineMaxCapacity = 1;
            
            /// <summary>
            /// Current number of ammo in the weapon.
            /// </summary>
            [SerializeField]
            protected int _nbCurrentAmmo = 0;

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

            /// <summary>
            /// Only GameObjects with on of these tags can be hit by the ray.
            /// </summary>
            [SerializeField]
            protected string[] _targetTag;

            protected string[] TargetTag
            {
                get { return _targetTag; }
                set { _targetTag = value; }
            }



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
        }
    }
}
