// @author : florian

using UnityEngine;
using System.Collections;

using Extinction.Characters;
using Extinction.Enums;
using Extinction.Skills;

namespace Extinction
{
    namespace Herbie
    {

        /// <summary>
        /// Command implementation, so that the agent attack the target.
        /// </summary>
        public class CommandSkillCast : Command
        {

            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            private Vector3 _targetPosition;
            private ActiveSkill _skillToCast;
            private bool _isFinished = false;
            //the delay between two update
            private float _aiDelay = 1;
            //ptr to coroutine to properly stop it
            private IEnumerator _moveAndCastCoroutine;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// create the command with all the informations : 
            /// </summary>
            public CommandSkillCast(Unit actor, ActiveSkill skillToCast, Vector3 targetPosition, float aiDelay = 1)
            {
                _actor = actor;
                _targetPosition = targetPosition;
                _aiDelay = aiDelay;
                _skillToCast = skillToCast;
            }

            /// <summary>
            /// create the command with all the informations, except the actor, which has to be set after : 
            /// </summary>
            public CommandSkillCast(ActiveSkill skillToCast, Vector3 targetPosition, float aiDelay = 1)
            {
                _targetPosition = targetPosition;
                _aiDelay = aiDelay;
                _skillToCast = skillToCast;
            }

            public override void Execute()
            {
                _actor.UnitBehaviour = UnitBehavior.Attacking;

                _moveAndCastCoroutine = MoveAndCastRoutine();
                _actor.StartCoroutine(_moveAndCastCoroutine);
            }

            //finished when the target has been killed, or when the target has escaped
            public override bool IsFinished()
            {
                return _isFinished;
            }

            IEnumerator MoveAndCastRoutine()
            {
                while (!_isFinished)
                {
                    //Check if agent can cast the active skill
                    if ( Vector3.Distance(_actor.transform.position, _targetPosition) <= _skillToCast.ActivationDistance )
                    {
                        _actor.stopWalking();
                        _skillToCast.activate(_targetPosition);
                        _isFinished = true;
                    }
                    else
                    {
                        //mose toward the target position
                        _actor.move(_targetPosition);
                    }

                    yield return new WaitForSeconds(_aiDelay);
                }
            }

            public override void End()
            {
                _actor.StopCoroutine(_moveAndCastCoroutine);
            }

            public override Command Clone()
            {
                return new CommandSkillCast(_actor, _skillToCast, _targetPosition, _aiDelay);
            }
        }
    }
}