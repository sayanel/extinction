// @author: Alex

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Extinction {
    namespace Network {
        public class NetworkInitializerSurvivor : NetworkInitializer{
            public override void Activate(GameObject go) {
                // go is a suvivor
                go.GetComponentInChildren<Camera>().enabled = true;
                //go.GetComponent<InputControllerSurvivor>().enabled = true;
            }
        }
    }
}