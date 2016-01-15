// @author : mehdi-antoine

using UnityEngine;
using System.Collections;
using Extinction.Characters;

namespace Extinction
{
    namespace HUD
    {
        public class HUDWeaponMarker : MonoBehaviour
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            [SerializeField]
            private Texture2D _accurateMarker;
            [SerializeField]
            private Texture2D _normalMarker;
            [SerializeField]
            private Survivor _survivor;

            private Rect _position;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            

            // Use this for initialization
            void Start()
            {
                _position = new Rect( Screen.width / 2 - 16, Screen.height / 2 - 16, 32, 32 );
            }

            // Update is called once per frame
            void Update()
            {

            }

            void OnGUI()
            {
                GUI.DrawTexture( _position, _survivor.isAiming ? _accurateMarker : _normalMarker );
            }
        }
    }
}
