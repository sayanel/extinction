// @author : florian

using UnityEngine;
using System.Collections;

using Extinction.Characters;
using Extinction.Enums;

namespace Extinction
{
    namespace Herbie
    {

        /// <summary>
        /// Command implementation, so that the agent attack the target.
        /// </summary>
        public class CommandMoveAndAttack : Command
        {

            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            private Character _target;
            private bool _isFinished = false;
            //the delay between two update
            private float _aiDelay = 1;
            //ptr to coroutine to properly stop it
            private IEnumerator _moveAndAttackCoroutine;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// create the command with all the informations : 
            /// </summary>
            public CommandMoveAndAttack( Unit actor, Character target, float aiDelay = 1 )
            {
                _actor = actor;
                _target = target;
                _aiDelay = aiDelay;
            }

            /// <summary>
            /// create the command with all the informations, except the actor, which has to be set after : 
            /// </summary>
            public CommandMoveAndAttack( Character target, float aiDelay = 1 )
            {
                _target = target;
                _aiDelay = aiDelay;
            }

            public override void Execute()
            {
                _actor.UnitBehaviour = UnitBehavior.Attacking;

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
                    if( _target.Health <= 0 ) // is the target already dead ? 
                    {
                        _isFinished = true;
                    }
                    //Check if agent can attack
                    else if( _actor.canAttack( _target ) )
                    {
                        _actor.stopWalking();

                        //float angle = Vector3.Angle( _actor.transform.forward, _target.transform.position - _actor.transform.position );
                        //_actor.turn(angle);
                        _actor.transform.LookAt( _target.transform );

                        _actor.attack();
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

            public override Command Clone()
            {
                return new CommandMoveAndAttack( _actor, _target, _aiDelay );
            }
        }
    }
}