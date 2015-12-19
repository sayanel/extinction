// @author : florian

using UnityEngine;
using System.Collections;

using Extinction.Characters;

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
    }
}