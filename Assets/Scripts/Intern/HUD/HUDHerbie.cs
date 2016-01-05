using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Extinction.Enums;
using Extinction.Characters;
using Extinction.Skills;

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

            /// <summary>
            /// A dictionary to store references to hud widgets representing each robot. 
            /// </summary>
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

            /// <summary>
            /// Tint color when a robot is selected.
            /// </summary>
            [SerializeField]
            Color _selectedColor = new Color(0, 1, 0, 0.7f);

            /// <summary>
            /// Tint color when a robot is selected.
            /// </summary>
            [SerializeField]
            Color _unselectedColor = new Color(1, 1, 1, 0.5f);

            /// <summary>
            /// Number of emplacements, on HUD, for robots.
            /// </summary>
            [SerializeField]
            int nbRobotEmplacement = 3;

            /// <summary>
            /// The model for robot widget.
            /// </summary>
            [SerializeField]
            private GameObject _robotWidgetModel;

            /// <summary>
            /// the path to find the robots visual widget from the robot widget
            /// </summary>
            [SerializeField]
            private string _robotsVisualWidgetPath = "RobotVisual";

            /// <summary>
            /// the path to find the robots active skills widget from the robot widget
            /// </summary>
            [SerializeField]
            private string _robotsActiveSkillsWidgetPath = "RobotActiveSkills";

            /// <summary>
            /// the path to find the robots active skills widget from the robot widget
            /// </summary>
            [SerializeField]
            private string _robotsLifeBarPath = "RobotLifeBar";

            /// <summary>
            /// A model of floating widget which contains text. 
            /// This widget is displayed on top of other widgets if the mouse ramains over the widget for a few seconds. 
            /// </summary>
            [SerializeField]
            private GameObject _floatingInfoModel;

            /// <summary>
            /// The floating info instance.
            /// </summary>
            [SerializeField]
            private GameObject _floatingInfoHUD;

            /// <summary>
            /// The delay before infos are displayed on top of HUD (in second).
            /// </summary>
            [SerializeField]
            private float _delayBeforeDisplayInfo = 1;

            //TEMPORARY
            [SerializeField]
            private List<SpecialRobot> _robotList = new List<SpecialRobot>();
            //TEMPORARY
            [SerializeField]
            private Characters.Herbie _herbie;


            void Awake()
            {
                if (_miniMap == null)
                {
                    _miniMap = transform.Find("HerbieMap");
                }
                if (_selectionHUD == null)
                {
                    _selectionHUD = transform.Find("RobotSelection");
                }

                /*
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

                    skillButton.onClick.AddListener(()=>{
                        skill.BeginExecution();
                    });
                }
                */
            }


            void Start()
            {
                //intantiate the floating info hud and attach it to herbie's hud.
                _floatingInfoHUD = Instantiate(_floatingInfoModel);
                _floatingInfoHUD.transform.SetParent(transform);

                //TEMPORARY
                initUI(_robotList, _herbie);
            }

            public void initUI(List<SpecialRobot> robots, Characters.Herbie herbie)
            {
                foreach (SpecialRobot robot in robots)
                {
                    GameObject newRobotWidget = Instantiate(_robotWidgetModel);

                    Transform robotVisual = newRobotWidget.transform.Find(_robotsVisualWidgetPath);

                    Transform robotActiveSkills = newRobotWidget.transform.Find(_robotsActiveSkillsWidgetPath);

                    //find the life bar, and give a reference to the robot.
                    Transform robotLifeBarTransform = newRobotWidget.transform.Find(_robotsLifeBarPath);
                    HUDProgressBar robotLifeBar = robotLifeBarTransform.GetComponent<HUDProgressBar>();
                    robot.setLifeBar( robotLifeBar );

                    //add tmpValues to avoid errors in lambdas : 
                    Characters.Herbie tmpHerbie = herbie;
                    SpecialRobot tmpRobot = robot;

                    //set the visual of the robot
                    robotVisual.GetComponent<Image>().sprite = robot.Visual;
                    robotVisual.GetComponent<Button>().onClick.AddListener( () => {
                        if( tmpHerbie.isSelecting( tmpRobot ) )
                            tmpHerbie.translateCameraOnYPlane( tmpRobot.transform.position );
                        else
                        {
                            if(Input.GetKeyDown(KeyCode.LeftShift))
                            {
                                if( tmpHerbie.isSelecting( tmpRobot ) )
                                    tmpHerbie.removeFromSelection( tmpRobot );
                                else
                                    tmpHerbie.addToSelection( tmpRobot );
                            }
                            else
                                tmpHerbie.changeSelection( new List<SpecialRobot> { tmpRobot } );
                        }
                    } );

                    //set the skills buttons : 
                    for(int i = 0; i < Mathf.Min(robot.getActiveSkillCount(), 2); ++i)
                    {
                        Transform skillEmplacement = robotActiveSkills.GetChild(i);

                        skillEmplacement.GetComponent<Image>().sprite = robot.getActiveSkill(i).Visual;

                        HUDSkillButton hudSkillButton = skillEmplacement.GetComponent<HUDSkillButton>();
                        //find cool down image in the child of skillEmplacement.
                        hudSkillButton.setCoolDownImage(skillEmplacement.GetChild(0).GetComponent<Image>());
                        //the description is the same as the skill description
                        hudSkillButton.setDescription(robot.getActiveSkill(i).Description);
                        //same floating info model as HUDHerbie's.
                        hudSkillButton.setFloatingInfoModel(_floatingInfoHUD);
                        //set a reference to the skill handled by this button
                        hudSkillButton.setSkill(robot.getActiveSkill(i));
                        //set the delay before displaying hud infos : 
                        hudSkillButton.setDelayBeforeDisplayingInfo(_delayBeforeDisplayInfo);

                        int tmpIndex = i;
                        HUDSkillButton tmpSkillButton = hudSkillButton;
                        skillEmplacement.GetComponent<Button>().onClick.AddListener(() => {

                            ActiveSkill tmpSkill = tmpRobot.getActiveSkill(tmpIndex);

                            if (tmpSkill != null)
                                tmpHerbie.prepareSkillCast(tmpSkill, tmpRobot, tmpSkillButton);
                        });
                    }

                    newRobotWidget.transform.SetParent(_selectionHUD);

                    _robotHudInfos.Add( robot.getCharacterName(), newRobotWidget.transform );
                }
            }

            public void changeSelection(List<CharacterName> selectedNames)
            {
                foreach(KeyValuePair<CharacterName, Transform> robotInfo in _robotHudInfos)
                {
                    bool interactivity = selectedNames.Contains( robotInfo.Key );
                    Color widgetColor = interactivity ? _selectedColor : _unselectedColor;

                    //change the visual to show a selected color on the robot hud info : 
                    robotInfo.Value.GetComponent<Image>().color = widgetColor;

                    //find the widget which contains the skill buttons : 
                    Transform robotActiveSkillsWidget = robotInfo.Value.Find( _robotsActiveSkillsWidgetPath );
                    //change skill button interactivity : 
                    Button[] skillButtons = robotActiveSkillsWidget.GetComponentsInChildren<Button>();
                    foreach( Button button in skillButtons )
                    {
                        button.interactable = interactivity;
                    }
                }
            }

            public void clearSelection()
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
