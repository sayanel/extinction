using UnityEngine;
using System.Collections;

namespace Extinction
{
    namespace Skills
    {
        public class CreateRCamera : ActiveSkill
        {
            [SerializeField]
            private Texture2D _cursorTexture;

            [SerializeField]
            private Vector2 _cursorHotSpot = Vector2.zero;

            [SerializeField]
            private CursorMode _cursorLockMode = CursorMode.Auto;

            [SerializeField]
            private GameObject _rCameraModel;

            public override void beginActivation()
            {
                Cursor.SetCursor(_cursorTexture, _cursorHotSpot, _cursorLockMode);
            }

            public override void activate(Vector3 position)
            {
                Instantiate(_rCameraModel, position, Quaternion.identity);
                //TODO : make the instantiation synchnized on the network

                StartCoroutine(handleCooldown());
                Cursor.SetCursor(null, _cursorHotSpot, _cursorLockMode);
            }
        }
    }
}
