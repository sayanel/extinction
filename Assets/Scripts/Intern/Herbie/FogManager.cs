// @author : florian

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Extinction.Characters;

namespace Extinction
{
    namespace Herbie
    {
        public class FogManager : MonoBehaviour
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// A dictionnary which store each unit which where revealed, and the number of unit actualy seeing the revealed unit. 
            /// If this count decrease to zero, the unit is hidden. 
            /// </summary>
            private Dictionary<GameObject, int> _observedUnits = new Dictionary<GameObject, int>();

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// Function to call when a unit enter into the field of view of an other unit.
            /// It will reveal the unit, if it is not revealed yet. Otherwise, it will increase the viewer counter into the dictionary.
            /// </summary>
            /// <param name="gameObject"></param>
            public void gameObjectEnterFieldOfView(GameObject gameObject)
            {
                int objectReferences = 0;
                if(_observedUnits.TryGetValue(gameObject, out objectReferences))
                {
                    if(objectReferences == 0)
                    {
                        revealGameObject(gameObject);
                    }

                    objectReferences++;
                }
                else
                {
                    _observedUnits.Add(gameObject, 1);
                }
            }

            /// <summary>
            /// Function to call when a unit enter into the field of view of an other unit.
            /// It will hide the unit, if it is no one is seing it. Otherwise, it will decrease the viewer counter into the dictionary.
            /// </summary>
            /// <param name="gameObject"></param>
            public void gameObjectLeaveFieldOfView(GameObject gameObject)
            {
                int objectReferences = 0;
                if (_observedUnits.TryGetValue(gameObject, out objectReferences))
                {
                    objectReferences--;
                    if(objectReferences == 0)
                    {
                        hideGameObject(gameObject);
                        _observedUnits.Remove(gameObject);
                    }
                }
            }

            /// <summary>
            /// reveal the game object (turn on render components)
            /// </summary>
            /// <param name="gameObject"></param>
            public void revealGameObject(GameObject gameObject)
            {
                Renderer[] meshRenderer = gameObject.GetComponentsInChildren<Renderer>();

                if(meshRenderer != null)
                {
                    for(int i = 0; i < meshRenderer.Length; i++)
                    {
                        meshRenderer[i].enabled = true;
                    }
                }
            }

            /// <summary>
            /// hide the game object (turn off render components)
            /// </summary>
            /// <param name="gameObject"></param>
            public void hideGameObject(GameObject gameObject)
            {
                Renderer[] meshRenderer = gameObject.GetComponentsInChildren<Renderer>();

                if (meshRenderer != null)
                {
                    for (int i = 0; i < meshRenderer.Length; i++)
                    {
                        meshRenderer[i].enabled = false;
                    }
                }
            }
        }
    }
}
