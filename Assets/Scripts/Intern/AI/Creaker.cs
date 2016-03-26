// Created by Clement & Maximilien
//Last update : 17/12/2015

using UnityEngine;
using System.Collections;
using System;
using Extinction.Characters;
using Extinction.Utils;


namespace Extinction {
    namespace AI {

        public enum State{
            IDLE,
            WALK,
            RUN,
            ATTACK,
            GETDAMAGE,
            DIE,
            SCREAM
        }

        public class Creaker : UncontrolableRobot, ITriggerable
        {

            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            [SerializeField] protected Transform _target;
            protected NavMeshAgent _nav; // Reference to the nav mesh agent.

            [SerializeField] State _state;

            [SerializeField] protected float _speed = 40;
            [SerializeField] protected float _life = 100;
            [SerializeField] public bool _isDead = false;
            [SerializeField] private int idGroup = 0;
            private Boolean isSeeingTarget = false;
            private int lambdaDist = 2;

             
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

                _state = State.IDLE;
                _nav = GetComponent<NavMeshAgent>();
                _animator = GetComponent<Animator>(); 
                _nav.speed = UnityEngine.Random.Range(1, 1);
                _nav.acceleration = UnityEngine.Random.Range(1, 1);
                _target = Horde.getWayPoint();
            }

            public void UpdateCreaker()
            {
                Vector2 WPposition = new Vector2(Horde.getGroupTarget(this.idGroup).position.x, Horde.getGroupTarget(this.idGroup).position.z);
                Vector2 creakerPosition = new Vector2(transform.position.x, transform.position.z);
                State previousState = getState();

                if (this.idGroup != 0)
                {
                    
                    if (!Horde.targetIsSurvivor(this.idGroup) && Vector3.Distance(WPposition, creakerPosition) <= lambdaDist)
                    {
                        Horde.setNewWaypoint(this.idGroup);
                        //Debug.LogError("idGroup: " + this.idGroup + " last WP: " + WPposition + " new WP : " + Horde.getGroupTarget(this.idGroup));
                    }
                    if(_target != Horde.getGroupTarget(idGroup))
                    {
                        _target = Horde.getGroupTarget(idGroup);
                        followTarget(_target); // follow target ( transform )
                    }
                    
                }
                else
                {
                    if (!Horde.targetIsSurvivor(this.idGroup) && Vector3.Distance(WPposition, creakerPosition) <= lambdaDist)
                        _target = Horde.getWayPoint(); 
                        followTarget(_target); // transform
                    //Debug.LogError("groupe: " + idGroup + " -> " + _target);
                }

                if(previousState != getState())
                    updateAnimator();

            }

            public State getState()
            {
                return _state;
            }

            public void setState(State s)
            {
                if (s == _state) return;
                else _state = s;
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
                                    setState(State.SCREAM);
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
                        _nav.speed += 5;
                        setState(State.RUN);
                        Debug.LogError("Collision with survivor " + c + " MyGrp: " + idGroup );
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
                        _nav.speed -= 5;
                        setState(State.WALK);
                    }
                }
            }

            public void updateAnimator()
            {
                switch (_state)
                {
                    case State.IDLE:
                        setAnimationState("Idle");
                        break;
                    case State.RUN:
                        setAnimationState("Run");
                        break;
                    case State.WALK:
                        setAnimationState("Walk");
                        break;
                    case State.ATTACK:
                        setAnimationState("Attack");
                        break;
                    //case State.GETDAMAGE:
                    //    setAnimationState("GetDamage");
                    //    break;
                    //case State.DIE:
                    //    setAnimationState("Die");
                    //    break;
                    //case State.SCREAM:
                    //    setAnimationState("Scream");
                    //    break;
                    default:
                        setAnimationState("Idle");
                        break;
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
                setState(State.WALK);
            }

            public void followTarget(Transform target)
            {
                _nav.SetDestination(target.position);
                setState(State.WALK);
            }

            public void followTarget(GameObject target)
            {
                _nav.SetDestination(target.transform.position);
                setState(State.WALK);
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
                setState(State.ATTACK);
                survivor.getDamage(5);
            }

            public void getDamage(float damage)
            {
                _life -= damage;
                setState(State.GETDAMAGE);
                if (_life <= 0)
                {
                    _isDead = true;
                    setState(State.DIE);
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
