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
            ATTACK,
            FOLLOWSURVIVOR,
            FOLLOWCREAKER,
            WANDER,
            DEAD
        };

        public class Creaker : UncontrolableRobot
        {

            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            [SerializeField] protected Transform _target;
            [SerializeField] protected Character _characterTarget;
            [SerializeField] protected GameObject[] _survivorsStealthCollider;
            protected NavMeshAgent _nav; // Reference to the nav mesh agent.
            protected GameObject[] _waypoints;

            [SerializeField] protected float _speed = 1;
            [SerializeField] protected AIState _AIstate;

            const float min = .5f;
            const float max = 1.5f;
            //float detectionAngle = 40;
            protected float detectionRadius = 5;


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
                _target = _waypoints[UnityEngine.Random.Range(0, _waypoints.Length)].transform;
                //detectionAngle *= Random.Range(min, max);
                detectionRadius *= UnityEngine.Random.Range(min, max);
                followTarget();
            }

            void Update()
            {

                if (_AIstate == AIState.WANDER)
                {

                    Debug.Log(this.gameObject.name + " : WANDER");
                }

                else if (_AIstate == AIState.FOLLOWSURVIVOR)
                {
                    if (_target) _target = _characterTarget.transform;
                    Debug.Log(this.gameObject.name + " : FOLLOW");
                }

                else if (_AIstate == AIState.FOLLOWCREAKER)
                {
                    //_characterTarget = 
                    if(_target) _target = _characterTarget.transform;
                    Debug.Log(this.gameObject.name + " : FOLLOW CREAKER");
                }

                else if (_AIstate == AIState.ATTACK)
                {

                    if (_target)  _target = _characterTarget.transform;
                    attackSurvivor((Survivor)_characterTarget);
                    Debug.Log(this.gameObject.name + " : ATTACK");
                }


                if (_target) followTarget();
            }

            //// We check for any collider collision 
            //public void OnTriggerEnter(Collider other)
            //{

            //    Vector3 enemyPos = other.transform.position;
            //    Vector3 direction = Vector3.Normalize(enemyPos - this._position);
            //    Debug.Log(this.tag);
            //    if(_AIstate == AIState.WANDER)
            //    {
            //        // If the entering collider is the survivor himself (we are on him) we change the state to ATTACK
            //        if (other.gameObject.tag == "rangeCollider") // range collider
            //        {
                        
            //            Character survivor = other.gameObject.transform.parent.gameObject.GetComponent<Survivor>();
            //            _characterTarget = survivor;
            //            _target = _characterTarget.transform;
            //            attackSurvivor((Survivor)survivor); 
            //            Debug.Log(this.gameObject.name + " : I AM ATTACKING THE SURVIVOR");
            //            _AIstate = AIState.ATTACK;
            //        }

            //        // If the entering collider is the stealthCollider of the survivor we follow the survivor
            //        // We need to cast a ray to check if the creaker can see the survivor 
            //        else if (other.gameObject.tag == "stealthCollider") // stealth collider
            //        {
                        
            //            Character survivor = other.gameObject.transform.parent.gameObject.GetComponent<Survivor>();
            //            _characterTarget = survivor;
            //            _target = _characterTarget.transform;
            //            Debug.Log(this.gameObject.name + " : I AM FOLLOWING THE SURVIVOR!");
            //            _AIstate = AIState.FOLLOWSURVIVOR;
            //        }

            //        //If the entering collider is an other creaker
            //        else if (other.gameObject.tag == "detectionCollider") // detection collider, other creaker
            //        {
            //            AIState creakerState = other.gameObject.transform.parent.gameObject.GetComponent<Creaker>().getState();

            //            _characterTarget = other.gameObject.transform.parent.gameObject.GetComponent<Creaker>();
            //            _target = _characterTarget.transform;
            //            //_target = other.gameObject.GetComponent<Creaker>().getTarget();

            //            if ( creakerState != AIState.WANDER) // if the other creaker is following a survivor or another creaker we follow him
            //            {
            //                _AIstate = AIState.FOLLOWCREAKER;
            //            }
            //            else 
            //            {
            //                _AIstate = AIState.FOLLOWCREAKER;
            //            }

            //            Debug.Log(this.gameObject.name + " : IS FOLLOWING CREAKER " + getTarget().gameObject.name);
            //        }
            //    }

            //    else if(_AIstate == AIState.FOLLOWCREAKER)
            //    {
            //        // If the entering collider is the survivor himself (we are on him) we change the state to ATTACK
            //        if (other.gameObject.tag == "rangeCollider") // range collider
            //        {
                        
            //            Character survivor = other.gameObject.transform.parent.gameObject.GetComponent<Survivor>();
            //            _characterTarget = survivor;
            //            _target = _characterTarget.transform;
            //            attackSurvivor((Survivor)survivor);
            //            Debug.Log(this.gameObject.name + " : I AM ATTACKING THE SURVIVOR");
            //            _AIstate = AIState.ATTACK;
            //        }

            //        // If the entering collider is the stealthCollider of the survivor we follow the survivor
            //        // We need to cast a ray to check if the creaker can see the survivor 
            //        else if (other.gameObject.tag == "stealthCollider") // stealth collider
            //        {
                        
            //            Character survivor = other.gameObject.transform.parent.gameObject.GetComponent<Survivor>();
            //            _characterTarget = survivor;
            //            _target = _characterTarget.transform;
            //            Debug.Log(this.gameObject.name + " : I AM FOLLOWING THE SURVIVOR!");
            //            _AIstate = AIState.FOLLOWSURVIVOR;
            //        }

            //        //If the entering collider is an other creaker
            //        else if (other.gameObject.tag == "detectionCollider") // detection collider, other creaker
            //        {
            //            //TODO: Implémenter gestion des groupes dans la Horde
            //            Debug.Log(this.gameObject.name + " JUST PASSED BY " + getTarget().gameObject.name);
            //        }
            //    }

            //    else if (_AIstate == AIState.FOLLOWSURVIVOR)
            //    {
            //        // If the entering collider is the survivor himself (we are on him) we change the state to ATTACK
            //        if (other.gameObject.tag == "rangeCollider") // range collider
            //        {      
            //            Character survivor = other.gameObject.transform.parent.gameObject.GetComponent<Survivor>();
            //            _characterTarget = survivor;
            //            _target = _characterTarget.transform;
            //            attackSurvivor((Survivor)survivor);
            //            Debug.Log(this.gameObject.name + " : I AM ATTACKING THE SURVIVOR");
            //            _AIstate = AIState.ATTACK;
            //        }
            //    }


            //}



            //public void OnTriggerStay(Collider other)
            //{
            //    //if (other.gameObject.name == "detectionCollider" && _AIstate == AIState.WANDER)
            //    //{
 
            //    //    Debug.Log(this.gameObject.name + " : IS FOLLOWING <AGAIN> " + other.gameObject.name);
            //    //}
            //}



            //public void OnTriggerExit(Collider other)
            //{
            //    // If the exiting collider is the survivor himself (we are not on him anymore) we follow him
            //    //if (other.gameObject.GetComponent<Character>() == _target && (_AIstate == AIState.ATTACK || _AIstate == AIState.FOLLOWCREAKER))
            //    //{
            //    //    _AIstate = AIState.FOLLOWSURVIVOR;
            //    //    followTarget();
            //    //    Debug.Log(this.gameObject.name + " : I AM FOLLOWING <AGAIN> THE SURVIVOR");
            //    //}

            //    //if (other.gameObject.tag == "rangeCollider" && _AIstate == AIState.ATTACK)
            //    //{

            //    //    Debug.Log(this.gameObject.name + " : I AM FOLLOWING <AGAIN> THE SURVIVOR " + other.gameObject.name);
            //    //}

            //    //else if (other.gameObject.tag == "stealthCollider")
            //    //{

            //    //    Debug.Log(this.gameObject.name + " : I AM WANDERING <AGAIN> ");
            //    //}

            //    if (_AIstate == AIState.FOLLOWCREAKER)
            //    {
            //        //If the exit collider is an other creaker
            //        if (other.gameObject.tag == "detectionCollider") // detection collider, other creaker
            //        {
            //            _AIstate = AIState.WANDER;
            //            Debug.Log(this.gameObject.name + " EXIT TRIGGER CREAKER COLLIDER " + getTarget().gameObject.name);
            //        }
            //    }

            //    else if(_AIstate == AIState.FOLLOWSURVIVOR)
            //    {
            //        if (other.gameObject.tag == "stealthCollider") // stealth collider
            //        {
            //            _AIstate = AIState.WANDER;
            //            //_characterTarget = null;
            //            Debug.Log(getTarget());
            //            Debug.Log(this.gameObject.name + " : EXIT STEALTH COLLIDER");
            //        }
            //    }

            //    else if (_AIstate == AIState.ATTACK)
            //    {
            //        if (other.gameObject.tag == "rangeCollider") // range collider
            //        {
            //            _AIstate = AIState.FOLLOWSURVIVOR;
            //            Debug.Log(this.gameObject.name + " : EXIT RANGE COLLIDER");
            //        }
            //    }


            //}

            private bool isAStealthCollider(GameObject gameObject)
            {
                foreach (GameObject stealthCollider in _survivorsStealthCollider)
                {
                    if (gameObject == stealthCollider) return true;
                }

                return false;
            }

            private bool isACreakerCollider(GameObject gameObject)
            {
                if (gameObject.tag == "detectionCollider") return true;

                return false;
            }

            public void setTarget(GameObject target)
            {
                _target = target.GetComponent<Character>().transform;
            }

            public void followTarget()
            {
                if (_target) _nav.SetDestination(_target.position);
            }

            public void followTarget(GameObject target)
            {
                _nav.SetDestination(target.transform.position);
            }

            public Transform getCreakerTarget(Creaker creaker)
            {
                //_target = creaker.GetComponent<Creaker>().getTarget().GetComponent<Character>();
                return creaker.getTarget();
            }

            public Transform getTarget()
            {
                return this._target;
            }

            public AIState getState()
            {
                return this._AIstate;
            }

            public void attackSurvivor(Survivor survivor)
            {
                //if (_target) _target.getDamage(5);
                survivor.getDamage(5);
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
                target.getDamage(5);
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

            public override Vector3 getOrientation()
            {
                throw new NotImplementedException();
            }
        }
    }
}
