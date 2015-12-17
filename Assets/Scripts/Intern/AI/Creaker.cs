//@autor : Max, Clement 17/12/15
//Last update : 17/12/2015

using UnityEngine;
using System.Collections;

namespace Extinction {
    namespace AI {
        private class Creaker : Characters.Character {

            enum AIState {
               wait,
               attack
            };

            const float min = .5f;
            const float max = 1.5f;
            //float detectionAngle = 40;
            float detectionRadius = 5;

            /// <summary>
            /// Initialize one creaker
            /// </summary>
            public void Awake()
            {
                //detectionAngle *= Random.Range(min, max);
                detectionRadius *= Random.Range(min, max);
            }

            public void OnCollisionEnter(Collision collision)
            {
                Vector3 enemyPos = collision.transform.position;
                Vector3 direction = Vector3.Normalize(enemyPos - this._position);
                //RaycastHit hit;
                //if(Physics.Raycast(this._position, direction, detectionRadius), hit){

                //}

                Debug.Log("Creaker.cs : VU!");
            }

            public override void getDamage(int dmg)
            {

            }
        }
    }
}
