// @author : Mehdi

using UnityEngine;
using System.Collections;
using Extinction.Characters;
using UnityEngine.UI;

namespace Extinction
{
    namespace Utils
    {
        public class GUISurvivorDebugger : MonoBehaviour
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            [SerializeField]
            private Text _survivorStateField;

            [SerializeField]
            private Text _survivorAimingField;

            [SerializeField]
            private Survivor _survivor;

            public void Update()
            {
                _survivorStateField.text = "State : " + _survivor.state;
                _survivorAimingField.text = "Aiming : " + _survivor.isAiming;
            }

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------


        }
    }
}
