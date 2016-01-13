//Author : Florian

using UnityEngine;
using System.Collections;


namespace Extinction
{
    namespace Utils
    {

        /// <summary>
        /// This detector will simply warn its _target when something enter or leave one of its collider.
        /// Its _target is a ITriggerable, a simple interface to handler these kind of events.
        /// If _target isn't set in the editor, the detector will try to fill this parameter searching a ITriggerable in its parent.
        /// </summary>
        public class Detector : MonoBehaviour
        {

            [SerializeField]
            private string _tag;

            [SerializeField]
            private GameObject _target;

            private ITriggerable _triggerableTarget;

            private Collider _collider;

            // Use this for initialization
            void Start()
            {
                if (_collider == null)
                    Debug.LogWarning("You have to place a collider on a GameObject which have a Detector Component ! ");
                if (_target == null)
                    _target = transform.parent.gameObject;

                _triggerableTarget = _target.GetComponent<ITriggerable>();
            }

            void OnTriggerEnter(Collider other)
            {
                if (_target != null)
                    _triggerableTarget.triggerEnter(other);
            }

            void OnTriggerExit(Collider other)
            {
                if (_target != null)
                    _triggerableTarget.triggerExit(other);
            }

        }
    }
}
