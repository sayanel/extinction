// Created by Clement & Maximilien
//Last update : 17/12/2015

using UnityEngine;
using System.Collections;
using System;
using Extinction.Characters;

namespace Extinction {
    namespace AI {

        enum AIState
        {
            wait,
            attack
        };

        public class Creaker : UncontrolableRobot
        {
            Transform _target;               // Reference to the player's position.
            NavMeshAgent _nav;              // Reference to the nav mesh agent.
            
            const float min = .5f;
            const float max = 1.5f;
            //float detectionAngle = 40;
            float detectionRadius = 5;




            /// <summary>
            /// Initialize one creaker
            /// </summary>
            public void Awake()
            {

                _target = GameObject.FindGameObjectWithTag("Target").transform;
                _nav = GetComponent<NavMeshAgent>();

                //detectionAngle *= Random.Range(min, max);
                detectionRadius *= UnityEngine.Random.Range(min, max);
            }

            void update()
            {
                // ... set the destination of the nav mesh agent to the player.
                _nav.SetDestination(_target.position);

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

            public override void addPotentialTarget(Character target)
            {
                throw new NotImplementedException();
            }

            public override void removePotentialTarget(Character target)
            {
                throw new NotImplementedException();
            }

            public override Character getPriorityTarget()
            {
                throw new NotImplementedException();
            }

            public override Character getTarget(int index)
            {
                throw new NotImplementedException();
            }

            public override void stopWalking()
            {
                throw new NotImplementedException();
            }

            public override void attack(Character target)
            {
                throw new NotImplementedException();
            }

            public override void attack()
            {
                throw new NotImplementedException();
            }

            public override void move(Vector3 vec)
            {
                throw new NotImplementedException();
            }

            public override void turn(float angle)
            {
                throw new NotImplementedException();
            }

            public override void setOrientation(Vector3 orientation)
            {
                throw new NotImplementedException();
            }

            public override void getDamage(int amount)
            {
                throw new NotImplementedException();
            }

            public override void activateSkill1()
            {
                throw new NotImplementedException();
            }

            public override void activateSkill2()
            {
                throw new NotImplementedException();
            }
        }
    }
}
