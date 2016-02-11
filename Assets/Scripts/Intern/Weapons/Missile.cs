// @author : Florian

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Extinction.Characters;
using Extinction.Enums;
using Extinction.FX;

namespace Extinction
{
    namespace Weapons
    {
        public class Missile : Projectile
        {

            [SerializeField]
            private float _damageRadius = 1;
            public float DamageRadius{
                get { return _damageRadius; }
                set { _damageRadius = value; }
            }

            [SerializeField]
            private List<CharacterType> _characterDamagedFilter;
            [SerializeField]
            private FXType explosionFX = FXType.ExplosionFX;

            /// <summary>
            /// trigger the missile explosion, detect character inside the explosion radius and apply damage to them.
            /// </summary>
            public void explode()
            {
                Collider[] colliders = Physics.OverlapSphere( transform.position, _damageRadius );

                foreach(Collider collider in colliders)
                {
                    Character character = collider.GetComponent<Character>();
                    if(character != null && _characterDamagedFilter.Contains(character.getCharacterType()))
                    {
                        //apply dammage to hit character

                        //TODO : deal with float for character life
                        character.getDamage((int)_damage); 
                    }
                }

                //launch the visual FX : 
                FXManager.Instance.Activate( explosionFX, transform.position, Quaternion.identity );
            }

            void OnTriggerEnter(Collider other)
            {
                Debug.Log( "missile encounter a target and will explode !" );

                if(!other.isTrigger) // make sure we are not collider a trigger
                    explode(); //trigge the explosion
            }

        }
    }
}
