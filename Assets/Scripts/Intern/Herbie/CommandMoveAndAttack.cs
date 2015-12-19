// @author : florian

using UnityEngine;
using System.Collections;

using Extinction.Characters;

namespace Extinction
{
    namespace Herbie
    {

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