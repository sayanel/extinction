// @author: Alex

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Extinction {
    namespace Network {
        /// <summary>
        /// Activate some components when Photon.Instanciate is called.
        /// Activate some components in local.
        /// Warning: the components such as input controller for survivor must be disabled in unity scene because of photon.
        /// </summary>
        public interface INetworkInitializerPrefab {
            void Activate();
        }
    }
}