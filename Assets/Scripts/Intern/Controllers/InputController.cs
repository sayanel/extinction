// Created by Florian, Mehdi-Antoine & Pascale

using UnityEngine;
using System.Collections;

namespace Extinction
{
    namespace Controllers
    {
        public abstract class InputController : MonoBehaviour
        {
            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// Listen to inputs and processes methods
            /// This can be impletemented the way you like
            /// </summary>
            public abstract void processUserInputs();
        }
    }
}
