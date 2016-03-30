// @author : mehdi-antoine

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Extinction.Characters;

namespace Extinction
{
    namespace HUD
    {
        public class HUDSurvivorHealth : MonoBehaviour
        {
            [SerializeField]
            private Survivor _survivor;

            public Survivor survivor { set { _survivor = value; } }
            
            [SerializeField]
            private Text _healthText;
            [SerializeField]
            private Image _healthSprite;

            void Update()
            {
                _healthText.text = _survivor.Health.ToString();
                float colorChange = (float)_survivor.Health / (float)_survivor.MaxHealth;
                float redValue = 0.7f * ( 1 - colorChange );
                float greenValue = 0.7f * colorChange;
                _healthText.color = new Color( redValue, greenValue, 0 );
                _healthSprite.color = new Color( redValue, greenValue, 0 );
            }


        }
    }
}

