// @author: Alex

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Extinction {
    namespace Network {
        /// <summary>
        /// Activate some componenents when Photon.Instanciate is called.
        /// Warning: the components such as input controller for survivor must be disabled in unity scene because of photon.
        /// Abstract: some attributes could be added in the future
        /// </summary>
        public abstract class NetworkInitializer : MonoBehaviour {
            public abstract void Activate(GameObject go);
        }
    }
}