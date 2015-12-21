// @author : florian

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Extinction
{
    namespace Characters
    {
        public class Herbie : MonoBehaviour
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// A list which contains the selected special robots.
            /// </summary>
            private List<SpecialRobot> _selected = new List<SpecialRobot>();

            /// <summary>
            /// This boolean is true if herbie is launching a active skill.
            /// ie : if herbie is waiting the player to click somewhere on the ground to trigger an active skill. 
            /// </summary>
            private bool _isCastingSpell = false;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            public void clearSelection()
            {

            }

            public void updateSelectionGUI()
            {

            }

            public void setDirectCommand(Command command)
            {

            }

            public void addCommand(Command command)
            {

            }
        }
    }
}
