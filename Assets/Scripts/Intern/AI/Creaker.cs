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

            [SerializeField] protected float _speed;
            [SerializeField] protected float _speedRun;
            //[SerializeField] protected float _life = 100;
            [SerializeField] public bool _isDead = false;
            [SerializeField] private int idGroup = 0;
            [SerializeField] private int nbCreakersGrp;
            private Boolean isSeeingTarget = false;
            private float lambdaDist = 3.0f;

             
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
                _speed = 2 + UnityEngine.Random.Range(0, 1);
                _nav.speed = _speed;
                _speedRun = 4 + UnityEngine.Random.Range(0, 1);
                _nav.acceleration = 1;// UnityEngine.Random.Range(1, 1);
                _target = Horde.getWayPoint();
                followTarget(_target);
            }

            public void UpdateCreaker()
            {
                
                Vector2 creakerPosition = new Vector2(transform.position.x, transform.position.z);
                State previousState = getState();

                nbCreakersGrp = Horde.getGroupSize(idGroup);

                // test 
                //Vector2 WPposition = new Vector2(_target.position.x, _target.position.z);
                //if (!Horde.targetIsSurvivor(this.idGroup) && Vector2.Distance(WPposition, creakerPosition) <= lambdaDist)
                //{
                //    Horde.setNewWaypoint(this.idGroup);
                //    _target = Horde.getWayPoint();

                //    Debug.LogError(Vector2.Distance(WPposition, creakerPosition));
                //    Debug.LogError("idGroup: " + this.idGroup + " last WP: " + WPposition + " new WP : " + Horde.getGroupTarget(this.idGroup));
                //    followTarget(_target);
                //}
                // end test

                if (this.idGroup != 0)
                {
                    Vector2 WPposition = new Vector2(Horde.getGroupTarget(this.idGroup).position.x, Horde.getGroupTarget(this.idGroup).position.z);
                    if (!Horde.targetIsSurvivor(this.idGroup) && Vector2.Distance(WPposition, creakerPosition) <= lambdaDist)
                    {
                        Horde.setNewWaypoint(this.idGroup);
                        //Debug.LogError(Vector2.Distance(WPposition, creakerPosition));
                        //Debug.LogError("idGroup: " + this.idGroup + " last WP: " + WPposition + " new WP : " + Horde.getGroupTarget(this.idGroup));
                    }
                    if (_target != Horde.getGroupTarget(idGroup))
                    {
                        _target = Horde.getGroupTarget(idGroup);
                        followTarget(_target); // follow target ( transform )
                    }

                }
                else
                {
                    Vector2 WPposition = new Vector2(_target.position.x, _target.position.z);
                    if (!Horde.targetIsSurvivor(this.idGroup) && Vector2.Distance(WPposition, creakerPosition) <= lambdaDist)
                    {
                        _target = Horde.getWayPoint();
                        followTarget(_target);
                    }


                    //Debug.LogError("groupe: " + idGroup + " -> " + _target);
                }

                if (previousState != getState())
                    updateAnimator();

            }

            public override void die()
            {
                setState( State.DIE );
                setAnimationState( "Die" );
            }

            private IEnumerator delayedDeath()
            {
                yield return new WaitForSeconds( 5.0F );
                //SkinnedMeshRenderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
                //foreach( SkinnedMeshRenderer renderer in renderers )
                //{
                //    renderer.enabled = false;
                //}
                PhotonNetwork.Destroy( this.gameObject );
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
                        _nav.speed = _speedRun;
                        setState(State.RUN);
                        //Debug.LogError("Collision with survivor " + c + " MyGrp: " + idGroup );
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
                        _nav.speed = _speed;
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
                //_nav.SetDestination(target.position);
                _nav.destination = target.position;
                setState(State.WALK);
            }

            public void followTarget(GameObject target)
            {
                //_nav.SetDestination(target.transform.position);
                _nav.destination = target.transform.position;
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
                float health = _health - amount;
                GetComponent<PhotonView>().RPC( "SetHealth", PhotonTargets.All, health );

                setState( State.GETDAMAGE );

                if( _health <= 0 )
                {
                    _isDead = true;
                    setState( State.DIE );
                    _health = 0;
                    //Horde.removeOneCreaker(this.idGroup);
                }
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
