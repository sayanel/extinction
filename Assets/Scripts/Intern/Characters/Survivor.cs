// @author: Mehdi

using UnityEngine;
using System.Collections;

namespace Extinction
{
    namespace Characters
    {
        public class Survivor : Character
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            private float _lifeMultiplier;
            private float _stealthMultiplier;
            private float _damageMultiplier;
            private float _armorMultiplier;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            public override void getDamage( int amount )
            {
                _health -= amount * _armorMultiplier;
            }

            public override void move( Vector3 vec )
            {
                throw new System.NotImplementedException();
            }

            public override void setOrientation( Vector3 orientation )
            {
                throw new System.NotImplementedException();
            }

            public override void turn( float angle )
            {
                throw new System.NotImplementedException();
            }

            public override void activateSkill1()
            {
                throw new System.NotImplementedException();
            }

            public override void activateSkill2()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
