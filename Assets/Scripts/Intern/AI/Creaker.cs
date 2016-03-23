// Created by Clement & Maximilien
//Last update : 17/12/2015

using UnityEngine;
using System.Collections;
using System;
using Extinction.Characters;
using Extinction.Utils;


namespace Extinction {
    namespace AI {

        public class Creaker : UncontrolableRobot, ITriggerable
        {

            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            [SerializeField] protected Transform _target;
            protected NavMeshAgent _nav; // Reference to the nav mesh agent.
            protected Animator _anim;

            [SerializeField] protected float _speed = 40;
            [SerializeField] protected float _life = 100;
            [SerializeField] public bool _isDead = false;
            [SerializeField] private int idGroup = 0;
            private Boolean isSeeingTarget = false;
            private int lambdaDist = 3;


            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// Initialize one creaker
            /// </summary>
            public void init()
            {
                 // TODO:
                 // Mettre un billet pour le déplacement dans le nav mesh agent pour créer des groupes plûtôt que des lignes 


                _nav = GetComponent<NavMeshAgent>();
                _anim = GetComponent<Animator>();
                _nav.speed = UnityEngine.Random.Range(4, 4);
                _nav.acceleration = UnityEngine.Random.Range(4, 4);
                _target = Horde.getWayPoint();
            }

            public void UpdateCreaker()
            {
                Vector2 WPposition = new Vector2(Horde.getGroupTarget(this.idGroup).position.x, Horde.getGroupTarget(this.idGroup).position.z);
                Vector2 creakerPosition = new Vector2(transform.position.x, transform.position.z);

                if (this.idGroup != 0)
                {
                    
                    if (!Horde.targetIsSurvivor(this.idGroup) && Vector3.Distance(WPposition, creakerPosition) <= lambdaDist)
                    {
                        Horde.setNewWaypoint(this.idGroup);
                        //Debug.LogError("idGroup: " + this.idGroup + " last WP: " + WPposition + " new WP : " + Horde.getGroupTarget(this.idGroup));
                    }
                    _target = Horde.getGroupTarget(idGroup);
                    followTarget(_target); // follow target ( transform )
                }
                else
                {
                    if (!Horde.targetIsSurvivor(this.idGroup) && Vector3.Distance(WPposition, creakerPosition) <= lambdaDist)
                        _target = Horde.getWayPoint(); 
                    followTarget(_target); // transform
                    Debug.LogError("groupe: " + idGroup + " -> " + _target);
                }
                    
            }

            public void triggerEnter(Collider other, string tag)
            {
                
                if(tag == "CreakerDetectionCollider") // If its our detection collider who collides
                {
                    if (other.gameObject.tag == "creakerDetectionCollider") // if the collider we collide with is creaker
                    {
                        Creaker c = other.gameObject.transform.parent.GetComponent<Creaker>();

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
                    }
                    else if (other.gameObject.tag == "playerDetectionCollider") // if the collider we collide with is a player
                    {
                        Character c = other.gameObject.transform.parent.GetComponent<Character>();
                        if(this.idGroup==0) this.idGroup = Horde.createNewGroup(c, 1);
                        Horde.setCharacterTarget(c, this.idGroup);
                        if (!this.isSeeingTarget) Horde.targetFound(this.idGroup); //verifier que la nouvelle target est identique à celle du groupe
                        this.isSeeingTarget = true;
                        _nav.speed += 10;
                    }

                   
                }

                else if (tag == "CreakerRangeCollider") // if its our range colliger which collides
                {
                    if(other.gameObject.tag == "playerRangeCollider") // if our range collider collides with a player range collider
                    {
                        Survivor s = other.gameObject.transform.parent.GetComponent<Survivor>();
                        attackSurvivor(s);
                    }
                }
            }

            public void triggerExit(Collider other, string tag)
            {       
                if (tag == "CreakerDetectionCollider") // If its our detection collider who collides
                { 
                    if (other.gameObject.tag == "stealthCollider" && this.isSeeingTarget) // if a survivor get out of the collider
                    {          
                        Horde.addTargetLost(this.idGroup);
                        this.isSeeingTarget = false;
                        _nav.speed -= 10;      
                    }
                }
            }

            public int getIdGroup()
            {
                return this.idGroup;
            }

            public void setIdGroup(int id)
            {
                this.idGroup = id;
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

            public void attackSurvivor(Survivor survivor)
            {
                survivor.getDamage(5);
            }

            public void getDamage(float damage)
            {
                _life -= damage;

                if (_life <= 0)
                {
                    _isDead = true;
                    Horde.removeOneCreaker(this.idGroup);
                }
                    
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

            public static implicit operator Creaker(Horde v)
            {
                throw new NotImplementedException();
            }
        }
    }
}
