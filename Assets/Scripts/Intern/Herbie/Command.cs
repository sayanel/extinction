// @author : florian

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
    }
}
