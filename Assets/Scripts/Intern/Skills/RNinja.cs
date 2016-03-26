using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Extinction.Herbie;
using Extinction.Enums;
using Extinction.Weapons;
using Extinction.HUD;
using Extinction.Skills;
using Extinction.Utils;
using Extinction.Characters;
using System;

namespace Extinction
{
    namespace Skills
    {
        public class RNinja : Unit, ITriggerable
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            [SerializeField]
            private List<Weapon> _weapons;

            [SerializeField]
            private HUDLifeBar _worldLifeBar; // WORLD life bar

            private NavMeshAgent _navMeshAgentComponent;

            private IEnumerator _rotateRoutine;

            [SerializeField]
            private float _rotationSmoothFactor;

            private List<Character> _targets = new List<Character>();
            private List<Character> _hiddenTargets = new List<Character>();

            /// <summary>
            /// command list. The commands has to be treaten by the controller one after an other
            /// </summary>
            private Queue<Command> _commandList = new Queue<Command>();

            /// <summary>
            /// current treaten command
            /// </summary>
            private Command _currentCommand = null;

            /// <summary>
            /// current treaten command, when the robot is driven by AI
            /// </summary>
            private Command _currentAICommand = null;

            /// <summary>
            /// Layer names of the objects which can block the visibility of the robot.
            /// Set to Terrain by default.
            /// </summary>   
            private string[] _terrainMasks = new string[] { "Terrain" };

            public string[] TerrainMasks{
                get{ return _terrainMasks; }
                set{ _terrainMasks = value; }
            }

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            void Awake()
            {
                _navMeshAgentComponent = GetComponent<NavMeshAgent>();
                _potentialTargets = new List<Character>();
            }

            void Start()
            {
                setAnimationState( "Idle" );
            }

            void Update()
            {
                updateTargets();

                AIUpdate();
            }


            /// <summary>
            /// Update the AI of the robot.
            /// </summary>
            void AIUpdate()
            {
                if( _unitBehavior == UnitBehavior.Idle )
                {
                    if( _targets.Count > 0 )
                    {
                        _currentAICommand = new CommandMoveAndAttack( this, getPriorityTarget(), 0.5f );
                        _currentAICommand.Execute();
                        _unitBehavior = UnitBehavior.Attacking;
                    }
                }
            }

            /// <summary>
            /// Implementation of ITriggerable function.
            /// Can be called by SpecialRobot's detector, when something enter the collider of the detecto.
            /// </summary>
            /// <param name="other">the collider which enter into the detector collider.</param>
            public void triggerEnter( Collider other )
            {
                Character characterComponent = other.GetComponent<Character>();

                if( characterComponent != null )
                {
                    if( characterComponent.getCharacterType() == CharacterType.Survivor )
                    {
                        addPotentialTarget( characterComponent );
                    }
                }
            }

            /// <summary>
            /// Implementation of ITriggerable function.
            /// Can be called by SpecialRobot's detector, when something enter the collider of the detecto.
            /// </summary>
            /// <param name="other">the collider which enter into the detector collider.</param>
            public void triggerExit( Collider other )
            {
                Character characterComponent = other.GetComponent<Character>();

                if( characterComponent != null )
                {
                    if( characterComponent.getCharacterType() == CharacterType.Survivor )
                    {
                        removePotentialTarget( characterComponent );
                    }
                }
            }

            /// <summary>
            /// Implementation of ITriggerable function.
            /// Can be called by SpecialRobot's detector, when something enter the collider of the detecto.
            /// </summary>
            /// <param name="other">the collider which enter into the detector collider.</param>
            /// <param name="tag">A string which can be send by the detector.</param>
            public void triggerEnter( Collider other, string tag )
            {
                triggerEnter( other );
            }

            /// <summary>
            /// Implemtation of ITriggerable function.
            /// Can be called by SpecialRobot's detector, when something leave the collider of the detecto.
            /// </summary>
            /// <param name="other">the collider which leave into the detector collider.</param>
            /// <param name="tag">A string which can be send by the detector.</param>
            public void triggerExit( Collider other, string tag )
            {
                triggerExit( other );
            }

            /// <summary>
            /// Add a character to the potential target list.
            /// </summary>
            /// <param name="target">Character to add to the potantial target list.</param>
            public override void addPotentialTarget( Character target )
            {
                _potentialTargets.Add( target );

                Vector3 rayDirection = target.transform.position - transform.position;
                float rayLength = ( rayDirection ).magnitude;
                rayDirection.Normalize();

                if( !Physics.Raycast( transform.position, rayDirection, rayLength, LayerMask.GetMask( _terrainMasks ) ) )
                {
                    _targets.Add( target );
                }
                else
                {
                    _hiddenTargets.Add( target );
                }
            }

            /// <summary>
            /// Remove one of the potential targetsfrom _potentialTarget.
            /// </summary>
            /// <param name="target">The character to remove.</param>
            public override void removePotentialTarget( Character target )
            {
                _potentialTargets.Remove( target );
                _targets.Remove( target );
                _hiddenTargets.Remove( target );
            }

            /// <summary>
            /// Get a target from _potentialTargets, by index.
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public override Character getTarget( int index )
            {
                if( index < 0 || index >= _potentialTargets.Count )
                    return null;

                return _potentialTargets[index];
            }

            /// <summary>
            /// Return the most "important" target. ie : The target which is the most dangerous for this special robot.
            /// </summary>
            /// <returns></returns>
            public override Character getPriorityTarget()
            {
                _targets.Sort( ( a, b ) => { return ( a.transform.position - transform.position ).magnitude.CompareTo( ( a.transform.position - transform.position ).magnitude ); } );

                if( _targets.Count > 0 )
                    return _targets[0];
                else
                    return null;
            }

            /// <summary>
            /// Update the targets, to remove hidden targets from target container, 
            /// and move visible target from hiddenTarget to targets.
            /// </summary>
            void updateTargets()
            {
                for( int i = 0; i < _targets.Count; ++i )
                {
                    Vector3 rayDirection = _targets[i].transform.position - transform.position;
                    float rayLength = ( rayDirection ).magnitude;
                    rayDirection.Normalize();

                    if( !Physics.Raycast( transform.position, rayDirection, rayLength, LayerMask.GetMask( _terrainMasks ) ) )
                    {
                        _hiddenTargets.Add( _targets[i] );
                        _targets.Remove( _targets[i] );
                    }
                }
                for( int i = 0; i < _hiddenTargets.Count; ++i )
                {
                    Vector3 rayDirection = _hiddenTargets[i].transform.position - transform.position;
                    float rayLength = ( rayDirection ).magnitude;
                    rayDirection.Normalize();

                    if( !Physics.Raycast( transform.position, rayDirection, rayLength, LayerMask.GetMask( _terrainMasks ) ) )
                    {
                        _targets.Add( _hiddenTargets[i] );
                        _hiddenTargets.Remove( _hiddenTargets[i] );
                    }
                }
            }

            /// <summary>
            /// return true if this agent is able to attack this a weapon
            /// </summary>
            public override bool canAttack()
            {
                if( _weapons.Count == 0 )
                    return false;
                if( !_canAttack )
                    return false;

                return true;
            }

            /// <summary>
            /// return true if this agent can directly attack the target
            /// </summary>
            public override bool canAttack( Character target )
            {
                if( _weapons.Count == 0 )
                    return false;
                if( !_canAttack )
                    return false;

                float distanceToTarget = Vector3.Distance( transform.position, target.transform.position );
                foreach( Weapon weapon in _weapons )
                {
                    if( weapon.Range < distanceToTarget )
                        return false;
                }

                return true;
            }

            public override void attack()
            {
                foreach( Weapon weapon in _weapons )
                {
                    weapon.fire();
                }
                setAnimationState( "Attack" );
            }

            public override void attack( Character target )
            {
                //compute angle between the actual orientation and the position of the target 
                float angle = Vector3.Angle( transform.forward, target.transform.position - transform.position );

                turn( angle );
                attack();

                setAnimationState( "Attack" );
            }

            public override void getDamage( int amount )
            {
                float health = _health - amount;
                GetComponent<PhotonView>().RPC( "SetHealth", PhotonTargets.All, health );

                if( _worldLifeBar != null )
                {
                    _worldLifeBar.changeHealth( _health, _maxHealth );
                }

                if( _health <= 0 )
                {
                    _isAlive = false;
                }
            }

            public override void move( Vector3 vec )
            {
                _navMeshAgentComponent.updateRotation = true;
                _navMeshAgentComponent.Resume();
                _navMeshAgentComponent.SetDestination( vec );

                setAnimationState( "Walk" );
            }

            public override void stopWalking()
            {
                _navMeshAgentComponent.Stop();
                _navMeshAgentComponent.updateRotation = false;
            }

            public override void turn( float angle )
            {
                transform.Rotate( 0, angle, 0 );
            }

            public void smoothTurn( float angle )
            {
                //remove the control over the rotation for the navMeshAgent in order to get a custom rotation to the robot
                _navMeshAgentComponent.updateRotation = false;

                //stop the previous rotation 
                if( _rotateRoutine != null )
                    StopCoroutine( _rotateRoutine );

                //begin a new rotation
                _rotateRoutine = rotateCoroutine( angle );
                StartCoroutine( _rotateRoutine );
            }

            private IEnumerator rotateCoroutine( float angle )
            {
                while( !transform.rotation.eulerAngles.y.AlmostEquals( angle, 0.05f ) )
                {
                    Quaternion.RotateTowards( transform.rotation, Quaternion.Euler( 0, angle, 0 ), _rotationSmoothFactor );
                    yield return new WaitForSeconds( 0.5f );
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

            public override void die()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}

