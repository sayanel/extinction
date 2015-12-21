// @author : florian

using UnityEngine;
using System.Collections;
using System;

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
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// The unit which will follow the order
            /// </summary>
            protected SpecialRobot _actor;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            //execute the command
            public abstract void Execute();

            //the agent has finished the command
            public abstract bool IsFinished();

            //properly end the command
            public abstract void End();

            public virtual void setActor(SpecialRobot specialRobot)
            {
                _actor = specialRobot;
            }

            /// <summary>
            /// Clone design patern, to be able to make copy of an command.
            /// </summary>
            /// <returns></returns>
            public abstract Command Clone();
        }
    }
}
