using UnityEngine;
using System.Collections;

namespace Extinction
{
    namespace HUD
    {
        /// <summary>
        /// Store informations which can be displayed on a HUD. 
        /// </summary>
        public class HUDInfoDisplayer : MonoBehaviour
        {
            /// <summary>
            /// An image which can be displayed on the HUD.
            /// </summary>
            [SerializeField]
            private Sprite _visual;

            public Sprite Visual
            {
                get{ return _visual; }
                set{ _visual = value; }
            }

            /// <summary>
            /// A label which can be displayed on the HUD.
            /// </summary>
            [SerializeField]
            private string _label = "label";

            public string Label
            {
                get{ return _label; }
                set{ _label = value; }
            }

            /// <summary>
            /// A description which can be displayed on the HUD.
            /// </summary>
            [SerializeField]
            private string _description = "a description...";

            public string Description
            {
                get{ return _description; }
                set{ _description = value; }
            }

        }
    }
}
