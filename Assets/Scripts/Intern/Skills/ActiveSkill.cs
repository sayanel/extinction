using UnityEngine;
using System.Collections;

namespace Extinction
{
    namespace Skills
    {
        public abstract class ActiveSkill : MonoBehaviour
        {
            /// <summary>
            /// Is the active skill apply on the character which has launched the skill ?
            /// </summary>
            [SerializeField]
            private bool _skillOnSelf = false;

            /// <summary>
            /// In case where the skill is launched on a position of battleground, this is the maximal distance from where the player can launch the skill.
            /// </summary>
            [SerializeField]
            private float _activationDistance = 0;

            /// <summary>
            /// The time (in second) we have to wait befor we can launch this skill.
            /// </summary>
            [SerializeField]
            private float _coolDown = 5;

            private float _currentCoolDown = 5;

            /// <summary>
            /// Set it to true if we can launch this skill (ie : the skill is available, and the cooldown is over).
            /// </summary>
            private bool _activable = true;

            public bool Activable{
                get { return _activable; }
            }

            /// <summary>
            /// Prepare the active skill to be launched (change mouse cursor,...).
            /// </summary>
            public abstract void beginActivation();

            /// <summary>
            /// Activate the active skill at position "position"
            /// </summary>
            /// <param name="position"> the position of the active skill when it is launched</param>
            public abstract void activate(Vector3 position);

            /// <summary>
            /// Handle the cooldown. Change the activable property, based on the current cooldown of this skill.
            /// </summary>
            /// <returns></returns>
            public virtual IEnumerator handleCooldown()
            {
                _activable = false;
                _currentCoolDown = _coolDown;

                while (_currentCoolDown > 0)
                {
                    _currentCoolDown-=0.5f;
                    yield return new WaitForSeconds(0.5f);
                }

                _activable = true;
            }
        }
    }
}
