using UnityEngine;
using System.Collections;

namespace Extinction
{
    namespace Utils
    {

        /// <summary>
        /// Symple interface to trigegr some functions. It is used by the Detector to call function when an object enter or leave one of its colliders.
        /// </summary>
        public interface ITriggerable
        {
            //void triggerEnter(Collider other);
            //void triggerExit(Collider other);

            void triggerEnter(Collider other, string tag);
            void triggerExit(Collider other, string tag);
        }
    }
}
