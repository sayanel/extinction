// Created by Clement & Maximilien
//Last update : 17/12/2015

using UnityEngine;
using System.Collections;
using System;
using Extinction.Characters;
using Extinction.Utils;
using Extinction.FX;


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
            private float lambdaDist = 6.0f;

            [SerializeField]
            public GameObject detectorCollider;
            [SerializeField]
            public GameObject rangeCollider;

            //[SerializeField]
            //public GameObject detectorCollider;
            //[SerializeField]
            //public GameObject rangeCollider;
            public enum LogicState { WANDERING, ATTACKING };
            [SerializeField]
            public LogicState _logicState = LogicState.WANDERING;
            [SerializeField]
            public Character _characterTarget;


            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------


            void Awake()
            {
                if(_animator == null)
                    _animator = GetComponent<Animator>();
            }

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

                detectorCollider.SetActive(true);
                rangeCollider.SetActive(true);
            }



            public void UpdateCreaker()
            {
                /*
                Vector2 creakerPosition = new Vector2(transform.position.x, transform.position.z);

                nbCreakersGrp = Horde.getGroupSize(idGroup);

                if (this.idGroup != 0)
                {
                    Vector2 WPposition = new Vector2(Horde.getGroupTarget(this.idGroup).position.x, Horde.getGroupTarget(this.idGroup).position.z);
                    if (!Horde.targetIsSurvivor(this.idGroup) && Vector2.Distance(WPposition, creakerPosition) <= lambdaDist)
                    {
                        Horde.setNewWaypoint(this.idGroup);
                        //Debug.LogError(Vector2.Distance(WPposition, creakerPosition));
                        //Debug.LogError("idGroup: " + this.idGroup + " last WP: " + WPposition + " new WP : " + Horde.getGroupTarget(this.idGroup));
                    }
                    //if (_target != Horde.getGroupTarget(idGroup))
                    //{
                    //    _target = Horde.getGroupTarget(idGroup);
                    //    followTarget(_target); // follow target ( transform )
                    //}
                    _target = Horde.getGroupTarget(this.idGroup);
                    followTarget(_target);

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
                }*/
                if (_logicState == LogicState.ATTACKING)
                {
                    if (_characterTarget == null)
                    {
                        _logicState = LogicState.WANDERING;
                        _target = Horde.getRandomWaypoint();
                        Horde.setNewWaypoint(this.idGroup);
                        _nav.SetDestination(_target.position);
                        setState(State.WALK);
                        Debug.Log("setDestination : " + Time.time);
                    }
                    else if (_characterTarget != null && _characterTarget.Health <= 0)
                    {
                        _characterTarget = null;
                        _logicState = LogicState.WANDERING;
                        _target = Horde.getRandomWaypoint();
                        Horde.setNewWaypoint(this.idGroup);
                        _nav.SetDestination(_target.position);
                        setState(State.WALK);
                        Debug.Log("setDestination : " + Time.time);
                    }
                    else
                    {
                        _nav.SetDestination(_characterTarget.transform.position);
                        Debug.Log("setDestination : " + Time.time);
                        Debug.Log("attacking !");
                    }
                }

            }

            public override void die()
            {
                StopAllCoroutines();
                StartCoroutine(delayedDeath());
                setAnimationState("Die");
                setState(State.DIE);
                _nav.Stop();
                //Debug.Log("I AM DEAD");
            }

            private IEnumerator delayedDeath()
            {
                yield return new WaitForSeconds(5.0F);
                //yield return new WaitForSeconds(5.0f);
                //Debug.Log("destroy creacker");
                //SkinnedMeshRenderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
                //foreach( SkinnedMeshRenderer renderer in renderers )
                //{
                //    renderer.enabled = false;
                //}
                PhotonNetwork.Destroy(this.gameObject);
            }

            public State getState()
            {
                return this._state;
            }

            public void setState(State s)
            {
                //if (this._state == State.DIE) {
                //    updateAnimator(); return;
                //}

                //if (s == State.DIE) {
                //    this._state = State.DIE;
                //    updateAnimator();
                //    return;
                //}
                if (s == this._state) return;
                else this._state = s;
                updateAnimator();
            }

            public void triggerEnter(Collider other, string tag)
            {

                
                if (tag == "CreakerDetectionCollider") // If its our detection collider who collides
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
                            else if(this.idGroup != 0 && c.getIdGroup() != 0 && this.idGroup != c.getIdGroup() )
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
                        //_nav.speed = _speedRun;
                        setState(State.RUN);

                        _logicState = LogicState.ATTACKING;
                        _characterTarget = other.transform.parent.GetComponent<Character>();
                        Debug.Log("switch to ATTACKING logic state.");
                        setAnimationState("Attack");

                        //Debug.LogError("Collision with survivor " + c + " MyGrp: " + idGroup);
                    }

                }

                else if (tag == "CreakerRangeCollider") // if its our range colliger which collides
                {
                    if(other.gameObject.tag == "playerRangeCollider") // if our range collider collides with a player range collider
                    {
                        Survivor s = other.gameObject.transform.parent.GetComponent<Survivor>();
                        attackSurvivor(s);
                        setAnimationState("Attack");
                    }
                    else if (other.gameObject.tag == "waypoint")
                    {
                        StartCoroutine(findWaypoint());
                    }
                }

            }

            IEnumerator findWaypoint()
            {
                Transform newTarget = Horde.getRandomWaypoint();
                if (_target == null)
                    _target = newTarget;
                while( Vector3.Distance(newTarget.position, _target.position) < 5)
                {
                    newTarget = Horde.getRandomWaypoint();
                    setState(State.SCREAM);

                    yield return new WaitForSeconds(2.0f);
                }

                _target = newTarget;
                Horde.setNewWaypoint(this.idGroup);
                _nav.SetDestination(_target.position);
                Debug.Log("setDestination : " + Time.time);
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

                if (tag == "CreakerDetectionCollider") // If its our RANGE collider who collides
                {
                    if (other.gameObject.tag == "playerRangeCollider") // if a survivor get out of the collider
                    {
                        setState(State.WALK);
                        //followTarget();

                    }
                }

            }

            public void updateAnimator()
            {
                float animationLength = 0;
                    switch (this._state)
                    {

                        case State.IDLE:
                            setAnimationState("Idle");
                            //Debug.LogError("State = Idle");
                            break;
                        case State.RUN:
                            setAnimationState("Run");
                            //Debug.LogError("State = Run");
                            break;
                        case State.WALK:
                            setAnimationState("Walk");
                            //Debug.LogError("State = Walk");
                            break;
                        case State.ATTACK:
                            setAnimationState("Attack");
                            animationLength = _animator.GetCurrentAnimatorStateInfo(0).length;
                            StartCoroutine(switchToWalkState(animationLength));
                        //Debug.LogError("State = Attack");
                        break;
                        case State.GETDAMAGE:
                            setAnimationState("GetDamage");
                            animationLength = _animator.GetCurrentAnimatorStateInfo(0).length;
                            StartCoroutine(switchToWalkState(animationLength));
                        //Debug.LogError("State = GetDamage");
                        break;
                        case State.DIE:
                            setAnimationState("Die");
                            //Debug.LogError("State = Die");
                            break;
                        case State.SCREAM:
                            setAnimationState("Scream");
                            animationLength = _animator.GetCurrentAnimatorStateInfo(0).length;
                            StartCoroutine(switchToWalkState(animationLength));
                            //setState(State.WALK);
                            //Debug.LogError("State = Scream");
                            break;
                        default:
                            setAnimationState("Walk");
                            //Debug.LogError("State = Default Walk");
                            break;
                    }
            }

            //return to walk state after a delay
            IEnumerator switchToWalkState(float delay)
            {
                yield return new WaitForSeconds(delay);
                setState(State.WALK);
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
                if (_target)
                {
                    _nav.SetDestination(_target.position); //TODO
                    setState(State.WALK);
                }
                else setState(State.IDLE);
                Debug.Log("setDestination : " + Time.time);
            }

            public void followTarget(Transform target)
            {
                //_nav.SetDestination(target.position);
                _nav.destination = target.position;
                setState(State.WALK);
                //setAnimationState("Walk");
                Debug.Log("setDestination : " + Time.time);

            }

            public void followTarget(GameObject target)
            {
                //_nav.SetDestination(target.transform.position);
                _nav.destination = target.transform.position;
                setState(State.WALK);
                //setAnimationState("Walk");
                Debug.Log("setDestination : " + Time.time);

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
                //setAnimationState("Attack");
                survivor.getDamage(5);
                //Debug.LogError("Attack");
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


                if( _health <= 0 )
                {
                    _isDead = true;
                    setState( State.DIE );
                    _health = 0;
                    //Horde.removeOneCreaker(this.idGroup);
                }
            }

            public override void onHealthChanged()
            {
                if (_health > 0)
                {
                    setState(State.GETDAMAGE);
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
