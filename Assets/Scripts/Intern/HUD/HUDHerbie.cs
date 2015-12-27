using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Extinction
{
    namespace HUD
    {
        public class HUDHerbie : MonoBehaviour
        {
            /// <summary>
            /// Reference to Herbie's miniMap on the HUD.
            /// </summary>
            [SerializeField]
            Transform _miniMap;

            /// <summary>
            /// Reference to Herbie's selection widget on the HUD.
            /// </summary>
            [SerializeField]
            Transform _selectionHUD;

            [SerializeField]
            Color _selectedColor = new Color(0, 1, 0, 0.7f);
            [SerializeField]
            Color _unselectedColor = new Color(1, 1, 1, 0.5f);

            void Awake()
            {
                if(_miniMap == null)
                {
                    _miniMap =  transform.Find("/HerbieMap");
                }
                if(_selectionHUD == null)
                {
                    _selectionHUD = transform.Find("/RobotSelection");
                }
            }

            void changeSelection(List<string> selectedNames)
            {

            }

            void clearSelection()
            {
                for(int i = 0; i < _selectionHUD.childCount; ++i)
                {
                    Transform currentChild = _selectionHUD.GetChild(i);

                    currentChild.GetComponent<Image>().color = _unselectedColor;
                }
            }


        }
    }
}
