using UnityEngine;
using System.Collections;

namespace Extinction
{
    namespace Skills
    {
        public abstract class ActiveSkill : MonoBehaviour
        {
            /// <summary>
            /// A visual for the skill. 
            /// </summary>
            [SerializeField]
            protected Sprite _visual;

            public Sprite Visual{
                get{ return _visual; }
            }

            /// <summary>
            /// A description for this skill.
            /// </summary>
            [SerializeField]
            protected string _description;

            public string Description {
                get { return _description; }
            }

            /// <summary>
            /// Is the active skill apply on the character which has launched the skill ?
            /// </summary>
            [SerializeField]
            protected bool _skillOnSelf = false;

            public bool SkillOnSelf {
                get { return _skillOnSelf; }
            }

            /// <summary>
            /// In case where the skill is launched on a position of battleground, this is the maximal distance from where the player can launch the skill.
            /// </summary>
            [SerializeField]
            protected float _activationDistance = 0;

            public float ActivationDistance {
                get { return _activationDistance; }
            }

            /// <summary>
            /// The time (in second) we have to wait befor we can launch this skill.
            /// </summary>
            [SerializeField]
            protected float _coolDown = 5;

            public float CoolDown {
                get { return _coolDown; }
            }

            protected float _currentCoolDown = 5;

            public float CurrentCoolDown {
                get { return _currentCoolDown; }
            }

            /// <summary>
            /// Set it to true if we can launch this skill (ie : the skill is available, and the cooldown is over).
            /// </summary>
            protected bool _activable = true;

            public bool Activable {
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
