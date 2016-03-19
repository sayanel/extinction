// @author : mehdi

using UnityEngine;
using System.Collections;
using Extinction.Enums;

namespace Extinction {
    namespace Characters {
        public class SurvivorAnimation : MonoBehaviour 
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            [SerializeField]
            private Survivor _survivor;

            [SerializeField]
            private Animator _bodyAnimator;

            [SerializeField]
            private Animator _handsAnimator;

            private CharacterState _currentState;
            private HandState _currentHandState;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            // Use this for initialization
	        void Start () {
	            
	        }

            // Update is called once per frame
	        void Update () {
                if ( _survivor.state != _currentState )
                {
                    _currentState = _survivor.state;
                    _bodyAnimator.SetInteger( "State", (int)_currentState );
                }

                if ( _survivor.handState != _currentHandState )
                {
                    _currentHandState = _survivor.handState;
                    _handsAnimator.SetInteger( "State", (int)_currentHandState );
                }
	        }
        }
    }
}
