// @author : 

using UnityEngine;
using System.Collections;

using Extinction.Characters;

namespace Extinction
{
    namespace Herbie
    {
        /// <summary>
        /// An abstract Command class, to implement the Command Design Patern
        /// </summary>
        public abstract class Command : ScriptableObject
        {

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            //execute the command
            public abstract void Execute();

            //the agent has finished the command
            public abstract bool IsFinished();

            //properly end the command
            public abstract void End();
        }

        /// <summary>
        /// Command implementation, to move the agent to a position
        /// </summary>
        public class CommandMove : Command
        {

            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// The unit which will follow the order
            /// </summary>
            private SpecialRobot _actor;

            private Vector3 _targetPosition;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// create the command with all the informations : 
            /// </summary>
            public CommandMove( SpecialRobot actor, Vector3 targetPosition )
            {
                _actor = actor;
                _targetPosition = targetPosition;
            }

            public override void Execute()
            {
                _actor.UnitBehaviour = UnitBehaviour.MOVE;

                _actor.move( _targetPosition );
            }

            /// <summary>
            /// Finished when the agent is near the targeted position
            /// </summary>
            public override bool IsFinished()
            {
                //if the agent is near the targeted position 
                if( Vector3.SqrMagnitude( _actor.transform.position - _targetPosition ) < 1 )
                    return true;
                else
                    return false;
            }

            public override void End()
            {
                //nothing
            }
        }


        /// <summary>
        /// Command implementation, so that the agent attack the target.
        /// </summary>
        public class CommandMoveAndAttack : Command
        {

            private SpecialRobot _actor;
            private Character _target;
            private bool _isFinished = false;
            //the delay between two update
            private float _aiDelay = 1;
            //ptr to coroutine to properly stop it
            private IEnumerator _moveAndAttackCoroutine;

            //create the command with all the informations : 
            public CommandMoveAndAttack( SpecialRobot actor, Character target, float aiDelay = 1 )
            {
                _actor = actor;
                _target = target;
                _aiDelay = aiDelay;
            }

            public override void Execute()
            {
                _actor.UnitBehaviour = UnitBehaviour.ATTACK;

                _moveAndAttackCoroutine = MoveAndAttackRoutine();
                _actor.StartCoroutine( _moveAndAttackCoroutine );
            }

            //finished when the target has been killed, or when the target has escaped
            public override bool IsFinished()
            {
                return _isFinished;
            }

            IEnumerator MoveAndAttackRoutine()
            {
                while( !_isFinished )
                {
                    if( !_target.IsAlive ) // is the target already dead ? 
                    {
                        _isFinished = true;
                    }
                    //Check if agent can attack
                    else if( _actor.canAttack( _target ) )
                    {
                        _actor.stopWalking();
                        _actor.attack( _target );
                    }
                    else
                    {
                        _actor.move( _target.transform.position );
                    }

                    yield return new WaitForSeconds( _aiDelay );
                }
            }

            public override void End()
            {
                _actor.StopCoroutine( _moveAndAttackCoroutine );
            }
        }
    }
}
