// @author : mehdi-antoine

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Extinction.Weapons;

namespace Extinction
{
    namespace HUD
    {
        public class HUDWeaponMagazine : MonoBehaviour
        {
            [SerializeField]
            private Weapon _weapon;
            public Weapon weapon { set { _weapon = value; } }
            
            [SerializeField]
            private Text _magazineText;

            void Update()
            {
                _magazineText.text = _weapon.nbCurrentAmmo.ToString();
                float colorChange = (float)_weapon.nbCurrentAmmo / (float)_weapon.magazineMaxCapacity;
                _magazineText.color = new Color( 0.5f + 0.5f * colorChange, colorChange, colorChange);
            }

        }
    }
}
