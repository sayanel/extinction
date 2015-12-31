// @author : Alex

using UnityEngine;
using System.Collections;

namespace Extinction {
    namespace Game {

        /// <summary>
        /// Preload script: big bang initialization.
        /// Must be attached to the _preload scene containing all managers and singletons.
        /// The _preload scene is used to properly handle Singleton and managers resources through the different levels. 
        /// </summary>

        public class Preload : MonoBehaviour {
            public string startLevelName;

            public void Start() {
                if (startLevelName == null)
                    throw new System.Exception("Game::Preload - The Start Level name must be filled into the unity inspector");

                Application.LoadLevel(startLevelName);
            }
        }
    }
}
