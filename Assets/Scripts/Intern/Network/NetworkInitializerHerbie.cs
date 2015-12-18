// @author: Alex

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Extinction {
    namespace Network {
        public class NetworkInitializerHerbie : NetworkInitializer {
            public override void Activate(GameObject go) {
                // go is Herbie
                //go.GetComponentInChildren<Camera>().enabled = true;
                //go.GetComponent<InputControllerSurvivor>().enabled = true;
            }
        }
    }
}