// @author : florian

using UnityEngine;
using System.Collections;

using Extinction.Characters;
using Extinction.Enums;
using System;

namespace Extinction
{
    namespace Herbie
    {

        /// <summary>
        /// Command implementation, to move the agent to a position
        /// </summary>
        public class CommandMove : Command
        {

            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            private Vector3 _targetPosition;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// create the command with all the informations : 
            /// </summary>
            public CommandMove( Unit actor, Vector3 targetPosition )
            {
                _actor = actor;
                _targetPosition = targetPosition;
            }

            /// <summary>
            /// create the command with all the informations, except the actor of the command, which has to be set after : 
            /// </summary>
            public CommandMove( Vector3 targetPosition )
            {
                _targetPosition = targetPosition;
            }

            public override void Execute()
            {
                _actor.UnitBehaviour = UnitBehavior.Moving;

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

            public override Command Clone()
            {
                return new CommandMove( _actor, _targetPosition );
            }
        }
    }
}