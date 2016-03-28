// @author : mehdi-antoine

using UnityEngine;
using System.Collections;
using Extinction.Characters;
using Extinction.Enums;

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

            private int targetSize = 128;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            

            // Use this for initialization
            void Start()
            {
                _position = new Rect( Screen.width / 2 - targetSize / 2, Screen.height / 2 - targetSize / 2, targetSize, targetSize );
            }

            // Update is called once per frame
            void Update()
            {

            }

            void OnGUI()
            {
                GUI.DrawTexture( _position, _survivor.isAiming  ? _accurateMarker : _normalMarker );
            }
        }
    }
}
