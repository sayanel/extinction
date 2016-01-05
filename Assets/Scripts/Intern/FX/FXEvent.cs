// @author : Alex

using UnityEngine;
using System.Collections;

namespace Extinction {
    namespace FX {
        /// <summary>
        /// Represents an fx event: abstract.
        /// On() and Off() must implement what components have to be enabled and disabled.
        /// </summary>
        public abstract class FXEvent: MonoBehaviour {

            /// <summary>
            /// How long the FX must be "played", in second. 
            /// </summary>
            [SerializeField]
            protected float _duration = 1;

            public abstract void On();
            public abstract void Off();

            /// <summary>
            /// Activates the FX during _duration time in seconds and stops the FX.
            /// </summary>
            public void Activate() {
                StopCoroutine(ActivateRoutine());
                this.gameObject.SetActive(true);
                On();
                StartCoroutine(ActivateRoutine());
            }

            public IEnumerator ActivateRoutine() {
                yield return new WaitForSeconds(_duration);
                Off();
                this.gameObject.SetActive(false);
            }
        }
    }
}
