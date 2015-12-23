// @author : Pascale

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
            /// Max number of ammo that can be stocked in the weapon
            /// </summary>
            protected int _magazineMaxCapacity = 1;
            /// <summary>
            /// Current number of ammo in the weapon
            /// </summary>
            [SerializeField]
            protected int _nbCurrentAmmo = 0;
            protected bool _isAutomatic = false;
            /// <summary>
            /// Time between two shoots
            /// </summary>
            [SerializeField]
            protected float _fireRate = 1.0f;
            [SerializeField]
            protected int _dammage = 1;
            protected bool _friendlyFire = true;

            protected float _previousTime;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------
            
            /// <summary>
            /// Reaload the weapon
            /// </summary>
            /// <param name="ammo">Quantity of ammo</param>
            public abstract void reload(int ammo);
            public abstract void fire();
        }
    }
}
