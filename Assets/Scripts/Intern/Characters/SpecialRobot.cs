// @author : florian

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using Extinction.Herbie;
using Extinction.Enums;
using Extinction.Weapons;
using Extinction.HUD;

namespace Extinction
{
    namespace Characters
    {
        public class SpecialRobot : Unit
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// canAttack is false if the special robot can't attack (robot shut down, destroy,...)
            /// </summary>
            [SerializeField]
            private bool _canAttack = true;

            [SerializeField]
            private List<Weapon> _weapons;

            private UnitBehavior _unitBehavior;

            public UnitBehavior UnitBehaviour
            {
                get { return _unitBehavior; }
                set { _unitBehavior = value; }
            }

            private NavMeshAgent _navMeshAgentComponent;

            private IEnumerator _rotateRoutine;

            [SerializeField]
            private float _rotationSmoothFactor;

            private List<Character> _targets = new List<Character>();
            private List<Character> _hiddenTargets = new List<Character>();

            [SerializeField]
            private float _detectionRadius = 10;

            private bool _drivenByAI = true;

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
            /// The visual of this robot. 
            /// </summary>
            private Sprite _visual;

            public Sprite Visual{
                get { return _visual; }
            }


            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            void Awake()
            {
                _navMeshAgentComponent = GetComponent<NavMeshAgent>();
            }

            void Update()
            {
                if(_drivenByAI)
                {
                    AIUpdate();
                }
                else
                {
                    manualUpdate();
                }
            }

            /// <summary>
            /// Execute the command given by the player.
            /// If there is no command, turn the behaviour of this robot to idle (ie : driven by AI).
            /// </summary>
            void manualUpdate()
            {
                //Replace the current command if it is unasigned, or if the command has finished
                if( _currentCommand == null || _currentCommand.IsFinished() )
                {
                    //if there is an other command to execute, set the current command and execute it
                    if( _commandList.Count > 0 )
                    {
                        //properly end the current command
                        if( _currentCommand != null )
                            _currentCommand.End();

                        //change the current command
                        _currentCommand = _commandList.Dequeue();

                        //Execute the new current command
                        _currentCommand.Execute();
                    }
                    //no command are set, return to the idle behaviour
                    else
                    {
                        idle();
                    }
                }
            }

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

            public override void activateSkill1()
            {
                //todo
            }

            public override void activateSkill2()
            {
                //todo
            }

            public override void addPotentialTarget( Character target )
            {
                _potentialTargets.Add( target );
                
                if(Physics.Raycast( transform.forward + new Vector3(0,0,4), target.transform.position, _detectionRadius))
                {
                    _targets.Add( target );
                }
                else
                {
                    _hiddenTargets.Add( target );
                }
            }

            public override void removePotentialTarget( Character target )
            {
                _potentialTargets.Remove( target );
                _targets.Remove( target );
                _hiddenTargets.Remove( target );
            }

            public override Character getTarget( int index )
            {
                if( index < 0 || index >= _potentialTargets.Count )
                    return null;

                return _potentialTargets[index];
            }

            public override Character getPriorityTarget()
            {
                if( _potentialTargets.Count > 0 )
                    return _potentialTargets[0];
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
                    if( !Physics.Raycast( transform.forward + new Vector3( 0, 0, 4 ), _targets[i].transform.position, _detectionRadius ) )
                    {
                        _hiddenTargets.Add( _targets[i] );
                        _targets.Remove( _targets[i] );
                    }
                }
                for( int i = 0; i < _targets.Count; ++i )
                {
                    if( Physics.Raycast( transform.forward + new Vector3( 0, 0, 4 ), _hiddenTargets[i].transform.position, _detectionRadius ) )
                    {
                        _targets.Add( _hiddenTargets[i] );
                        _hiddenTargets.Remove( _hiddenTargets[i] );
                    }
                }
            }

            /// <summary>
            /// return true if this agent is able to attack this a weapon
            /// </summary>
            public bool canAttack()
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
            public bool canAttack( Character target )
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
                foreach(Weapon weapon in _weapons)
                {
                    weapon.fire();
                }
            }

            public override void attack( Character target )
            {
                //compute angle between the actual orientation and the position of the target 
                float angle = Vector3.Angle(transform.forward, target.transform.position - transform.position);

                turn( angle );
                attack();
            }

            public override void getDamage( int amount )
            {
                _health -= amount;
                if(_health <= 0)
                {
                    _isAlive = false;
                }
            }

            public override void move( Vector3 vec )
            {
                _navMeshAgentComponent.updateRotation = true;
                _navMeshAgentComponent.Resume();
                _navMeshAgentComponent.SetDestination( vec );
            }

            public override void setOrientation( Vector3 orientation )
            {
                //nothing
            }

            public override void stopWalking()
            {
                _navMeshAgentComponent.Stop();
                _navMeshAgentComponent.updateRotation = false;
            }

            public override void turn( float angle )
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

            private IEnumerator rotateCoroutine(float angle)
            {
                while(! transform.rotation .eulerAngles.y.AlmostEquals( angle , 0.05f) )
                {
                    Quaternion.RotateTowards( transform.rotation, Quaternion.Euler( 0, angle, 0 ), _rotationSmoothFactor );
                    yield return new WaitForSeconds( 0.5f );
                }
            }

            /// <summary>
            /// Immediatly gives an order to the robot. 
            /// This action remove all the previous commands given to this robot (clear command list).
            /// </summary>
            /// <param name="command"></param>
            public void setDirectCommand(Command command)
            {
                clearCommand();
                _currentCommand = command;
                _drivenByAI = false;

                if( _currentAICommand != null)
                _currentAICommand.End();

                _currentCommand.Execute();
            }

            /// <summary>
            /// Add a command to the command list. 
            /// The commands of the list will be executed one after the other.
            /// </summary>
            /// <param name="command"></param>
            public void addCommand(Command command)
            {
                _commandList.Enqueue( command );
                _drivenByAI = false;

                if( _currentAICommand != null )
                    _currentAICommand.End();
            }

            /// <summary>
            /// Remove all the commands of the list. 
            /// Also remove the current command.
            /// Automaticaly set the behaviour to idle. 
            /// </summary>
            public void clearCommand()
            {
                _commandList.Clear();
                _currentCommand = null;
                idle();
            }

            /// <summary>
            /// Make this robot have a idle behaviour.
            /// ie : It is controlled by AI until the player give it an order. 
            /// The AI of idling special robots is simple : They attack every target which are close enought.
            /// </summary>
            public void idle()
            {
                _unitBehavior = UnitBehavior.Idle;
                _drivenByAI = true;
            }

            /// <summary>
            /// return the HUDInfoDisplayer attached to this gameObject
            /// </summary>
            /// <returns></returns>
            public HUDInfoDisplayer getHUDInfo()
            {
                return _hudInfoDisplayerComponent;
            }

        }
    }
}