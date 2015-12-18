// @author : florian

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Extinction
{
    namespace Characters
    {
        public class SpecialRobot : Unit
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            [SerializeField]
            private bool _isAlive = true;

            [SerializeField]
            private List<Weapon> _weapons;

            private UnitBehaviour _unitBehaviour;

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
            /// current treaten command, if the robot is driven by the AI, not by a user input 
            /// </summary>
            private Command _currentAICommand = null;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            void Awake()
            {
                _navMeshAgentComponent = GetComponent<NavMeshAgent>();
            }


            void Start()
            {

            }

            void Update()
            {
                if(_drivenByAI)
                {
                    AIUpdate();
                }
            }

            void AIUpdate()
            {
                if( _unitBehaviour == _unitBehaviour.IDLE )
                {
                    if(_targets.Count > 0)
                        _currentCommand = new CommandMoveAndAttack();
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

            public override Character getPriorityTarget()
            {
                if( _potentialTargets.Count > 0 )
                    return _potentialTargets[0];
                else
                    return null;
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
                _navMeshAgentComponent.updateRotation = false;
                StopCoroutine( _rotateRoutine );
                StartCoroutine( _rotateRoutine );
            }

            private IEnumerator rotateCoroutine(float angle)
            {
                while(! transform.rotation .eulerAngles.y.AlmostEquals( angle , 0.05f) )
                {
                    Quaternion.RotateTowards( transform.rotation, Quaternion.EulerRotation( 0, angle, 0 ), _rotationSmoothFactor );
                    yield return new WaitForSeconds( 0.5f );
                }
            }

            void updateTargets()
            {
                for(int i = 0; i < _targets.Count; ++i )
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

            public void setDirectCommand(Command command)
            {

            }

            public void addDirectCommand(Command command)
            {

            }

            public void clearCommand()
            {

            }

            public void idle()
            {

            }

        }
    }
}