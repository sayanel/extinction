// Created by Clement & Maximilien
//Last update : 17/12/2015

using UnityEngine;
using System.Collections;
using System;
using Extinction.Characters;
using Extinction.Utils;


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

        public class Creaker : UncontrolableRobot, ITriggerable
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

            [SerializeField] private int idGroup = 0;
            private Boolean isSeeingTarget = false;
            private int lambdaDist = 10;

            [SerializeField] private float delayAttackTime;
            [SerializeField] private bool canAttack;
            [SerializeField] private float nextAttack;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            public Creaker()
            {
               
            }

            /// <summary>
            /// Initialize one creaker
            /// </summary>
            public void Awake()
            {

                //  _target = GameObject.FindGameObjectWithTag("target").transform;
                //  _survivor = GameObject.FindGameObjectWithTag("Player");
                _survivorsStealthCollider = GameObject.FindGameObjectsWithTag("playerDetectionCollider");
                _nav = GetComponent<NavMeshAgent>();
                _waypoints = GameObject.FindGameObjectsWithTag("waypoint");
                _AIstate = AIState.WANDER;
                _target = _waypoints[UnityEngine.Random.Range(0, _waypoints.Length)].transform;
                //detectionAngle *= Random.Range(min, max);
                detectionRadius *= UnityEngine.Random.Range(min, max);

                canAttack = true;
                delayAttackTime = 2f;
                nextAttack = 0f;
            }

            void Update()
            {
                if (this.idGroup != 0)
                {
                    if (!Horde.targetIsSurvivor(this.idGroup) && Vector3.Distance(Horde.getGroupTarget(this.idGroup).position, transform.position) < lambdaDist)
                    {
                        Horde.setNewWaypoint(this.idGroup);
                    }
                    followTarget(Horde.getGroupTarget(idGroup));
                }
                else
                {
                    if (!Horde.targetIsSurvivor(this.idGroup) && Vector3.Distance(Horde.getGroupTarget(this.idGroup).position, transform.position) <= lambdaDist)
                        _target = _waypoints[UnityEngine.Random.Range(0, _waypoints.Length)].transform;
                    followTarget(_target);
                }
                    
            }

            public void triggerEnter(Collider other, string tag)
            {
                
                if(tag == "CreakerDetectionCollider") // If its our detection collider who collides
                {
                    if (other.gameObject.tag == "creakerDetectionCollider") // if the collider we collide with is creaker
                    {
                        Creaker c = other.gameObject.transform.parent.GetComponent<Creaker>();
                        //if (c.getIdGroup() != this.idGroup)
                        //{
                            if (this.idGroup == 0 && c.getIdGroup() == 0) // We do not belong to any group
                            {
                                int id = Horde.createNewGroup();
                                setIdGroup(id);
                                c.setIdGroup(id);
                            }
                            else if (c.getIdGroup() == 0)
                            {
                                c.setIdGroup(this.idGroup);
                                Horde.addOneCreaker(this.idGroup);
                            }
                            else
                            {
                                if (Horde.getGroupSize(c.getIdGroup()) > Horde.getGroupSize(this.idGroup))
                                {
                                    Horde.removeOneCreaker(this.idGroup);
                                    setIdGroup(c.getIdGroup());
                                    Horde.addOneCreaker(this.idGroup);
                                }
                            }
                        //}
                    }
                    else if (other.gameObject.tag == "playerDetectionCollider") // if the collider we collide with is a player
                    {
                        Character c = other.gameObject.transform.parent.GetComponent<Character>();
                        if(this.idGroup==0) this.idGroup = Horde.createNewGroup(c, 1);
                        Horde.setCharacterTarget(c, this.idGroup);
                        if (!this.isSeeingTarget) Horde.targetFound(this.idGroup); //verifier que la nouvelle target est identique à celle du groupe
                        this.isSeeingTarget = true;
                    }

                   
                }

                else if (tag == "CreakerRangeCollider") // if its our range colliger which collides
                {
                    //Debug.LogError("CreakerRangeColliderCollides");
                    if(other.gameObject.tag == "playerRangeCollider") // if our range collider collides with a player range collider
                    {
                        Survivor s = other.gameObject.transform.parent.GetComponent<Survivor>();
                        attackSurvivor(s);
                        //InvokeRepeating("attackSurvivor(s)", 0f, delayAttackTime);
                        //Debug.LogError("trigger enter touch");
                    }
                }
            }

            public void triggerExit(Collider other, string tag)
            {       
                if (tag == "CreakerDetectionCollider") // If its our detection collider who collides
                { 
                    if (other.gameObject.tag == "stealthCollider" && this.isSeeingTarget) // if a survivor get out of the collider
                    {          
                        //Horde.setCharacterTarget(null, this.idGroup);
                        Horde.addTargetLost(this.idGroup);
                        this.isSeeingTarget = false;      
                    }
                }
            }

            //public void triggerStay(Collider other, string tag)
            //{
            //    if (tag == "CreakerRangeCollider") // if its our range colliger which collides
            //    {
            //        //Debug.LogError("CreakerRangeColliderCollides");
            //        if (other.gameObject.tag == "playerRangeCollider" && Time.time > nextAttack) // if our range collider collides with a player range collider
            //        {
            //            Survivor s = other.gameObject.transform.parent.GetComponent<Survivor>();
            //            attackSurvivor(s);

            //            nextAttack = Time.time + delayAttackTime;
            //            //canAttack = false;
            //            //yield return new WaitForSeconds(1);
            //            //canAttack = true;
            //        }
            //    }
            //}

            //public void OnTriggerStay(Collider other)
            //{

            //}


            public int getIdGroup()
            {
                return this.idGroup;
            }

            public void setIdGroup(int id)
            {
                this.idGroup = id;
            }

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

            public void followTarget(Transform target)
            {
                _nav.SetDestination(target.position);
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
                //Debug.LogError("kikoo");
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

            public static implicit operator Creaker(Horde v)
            {
                throw new NotImplementedException();
            }
        }
    }
}
