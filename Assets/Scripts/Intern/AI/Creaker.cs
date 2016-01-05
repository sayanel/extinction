// Created by Clement & Maximilien
//Last update : 17/12/2015

using UnityEngine;
using System.Collections;
using System;
using Extinction.Characters;

namespace Extinction {
    namespace AI {

        public enum AIState
        {
            WAIT,
            ATTACK,
            FOLLOWSURVIVOR,
            FOLLOW,
            WANDER
        };

        public class Creaker : UncontrolableRobot
        {

            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            [SerializeField] Character _target;
            GameObject _survivor;
            GameObject[] _survivorsStealthCollider;
            NavMeshAgent _nav;              // Reference to the nav mesh agent.
            GameObject[] _waypoints;

            [SerializeField] private float _speed = 1;
            [SerializeField] AIState _AIstate;

            const float min = .5f;
            const float max = 1.5f;
            //float detectionAngle = 40;
            float detectionRadius = 5;


            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// Initialize one creaker
            /// </summary>
            public void Awake()
            {

                //  _target = GameObject.FindGameObjectWithTag("target").transform;
                //  _survivor = GameObject.FindGameObjectWithTag("Player");
                _survivorsStealthCollider = GameObject.FindGameObjectsWithTag("stealthCollider");
                _nav = GetComponent<NavMeshAgent>();
                _waypoints = GameObject.FindGameObjectsWithTag("waypoint");
                _AIstate = AIState.WANDER;
                //detectionAngle *= Random.Range(min, max);
                detectionRadius *= UnityEngine.Random.Range(min, max);
            }

            void Update()
            {
                // ... set the destination of the nav mesh agent to the survivor.
                if (_AIstate == AIState.FOLLOW) {
                    followTarget();
                    // Debug.Log("pos: " + _survivor.transform.position+ " " + transform.position);
                }

                else if (_AIstate == AIState.WANDER){
                    // We set a random destination to our creaker (TODO)
                    // He is moving toward the waypoint n°2
                    _nav.SetDestination(_waypoints[2].transform.position);
                    // Debug.Log("pos: " + _survivor.transform.position + " " + transform.position + " : WANDER");
                }

                else if (_AIstate == AIState.ATTACK){
                    attack();
                    Debug.Log("ATTACK");
                }


            }

            // We check for any collider collision 
            public void OnTriggerEnter(Collider other)
            {
                
                Vector3 enemyPos = other.transform.position;
                Vector3 direction = Vector3.Normalize(enemyPos - this._position);
                //RaycastHit hit;
                //if(Physics.Raycast(this._position, direction, detectionRadius), hit){

                //}

                // If the entering collider is the stealthCollider of the survivor we follow the survivor
                // We need to cast a ray to check if the creaker can see the survivor 
               
                if (isAStealthCollider(other.gameObject) && _AIstate != AIState.FOLLOW){
                    _AIstate = AIState.FOLLOW;
                    setTarget(other.gameObject);
                    followTarget();
                    Debug.Log("Creaker.cs : I AM FOLLOWING THE SURVIVOR!"); 
                }
                
                // If the entering collider is the survivor himself (we are on him) we change the state to ATTACK
                if (other.gameObject.GetComponent<Survivor>() == _target && _AIstate != AIState.ATTACK){
                    _AIstate = AIState.ATTACK;
                    followTarget();
                    Debug.Log("Creaker.cs : I AM ATTACKING THE SURVIVOR");
                }


            }

            public void OnTriggerExit(Collider other)
            {


                // If the exiting collider is the survivor himself (we are not on him anymore) we follow him
                if (other.gameObject == _target.GetComponent<Survivor>() && _AIstate == AIState.ATTACK)
                {
                    _AIstate = AIState.FOLLOW;
                    followTarget();
                    Debug.Log("Creaker.cs : I AM FOLLOWING <AGAIN> THE SURVIVOR");
                }
            }

            private bool isAStealthCollider(GameObject gameObject)
            {
                if (gameObject == _survivorsStealthCollider[0]) return true;
                return false;
            }

            public void setTarget(GameObject target)
            {
                _target = target.GetComponent<Survivor>();
            }

            public void followTarget()
            { 
                if(_target) _nav.SetDestination(_target.transform.position);
            }

            public void followTarget(GameObject target)
            {
                _nav.SetDestination(target.transform.position);
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
                if(_target) _target.getDamage(5);
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

            public override Vector3 getOrientation()
            {
                throw new NotImplementedException();
            }
        }
    }
}
