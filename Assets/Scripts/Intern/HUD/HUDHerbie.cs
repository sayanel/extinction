using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Extinction.Enums;

namespace Extinction
{
    namespace HUD
    {
        public class HUDHerbie : MonoBehaviour
        {
            [System.Serializable]
            private class RobotHUDInfo {
                public CharacterName _name;
                public Transform _hudRef;
            }

            [SerializeField]
            private List<RobotHUDInfo> _robotHudInfoList = new List<RobotHUDInfo>();

            private Dictionary<CharacterName, Transform> _robotHudInfos = new Dictionary<CharacterName, Transform>();


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

            [SerializeField]
            int nbRobotEmplacement = 3;

            void Awake()
            {
                if(_miniMap == null)
                {
                    _miniMap =  transform.Find("HerbieMap");
                }
                if(_selectionHUD == null)
                {
                    _selectionHUD = transform.Find("RobotSelection");
                }

                //fill dictionary : 
                _robotHudInfos.Clear();
                for( int emplacementIndex = 0; emplacementIndex < 3; emplacementIndex++ )
                {
                    if( _robotHudInfoList.Count <= emplacementIndex || _robotHudInfoList[emplacementIndex]._hudRef == null )
                    {
                        _selectionHUD.Find( "RobotEmplacement" + emplacementIndex.ToString() );
                    }
                    else
                        _robotHudInfos.Add( _robotHudInfoList[emplacementIndex]._name, _robotHudInfoList[emplacementIndex]._hudRef );

                    emplacementIndex++;
                }

                //fill button for casting active skills : 
                ActiveSkill[] allActiveSkill = GetComponentsInChildren<ActiveSkill>();
                foreach(ActiveSkill skill in allActiveSkill)
                {
                    Button skillButton = skill.GetComponent<Button>();

                    skillButton.onClick.AddListener([]=>{
                        skill.BeginExecution();
                    });
                }
            }

            void changeSelection(List<CharacterName> selectedNames)
            {
                foreach(KeyValuePair<CharacterName, Transform> robotInfo in _robotHudInfos)
                {
                    if( selectedNames.Contains( robotInfo.Key ) )
                    {
                        //change the visual to show a selected color on the robot hud info
                        robotInfo.Value.GetComponent<Image>().color = _unselectedColor;
                    }
                    else
                    {
                        //change the visual to show an unselected color on the robot hud info
                        robotInfo.Value.GetComponent<Image>().color = _unselectedColor;
                    }
                }
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
