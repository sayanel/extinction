// @author : florian

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Extinction.HUD;
using Extinction.Enums;
using Extinction.Herbie;
using Extinction.Skills;
using Extinction.Cameras;

namespace Extinction
{
    namespace Characters
    {
        public class Herbie : MonoBehaviour
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// GUI which will display the current selection.
            /// Is automatically find at runtime in the parameter isn't set.
            /// </summary>
            [SerializeField]
            private HUDHerbie _hudHerbie;

            /// <summary>
            /// Herbie's camera.
            /// Is automatically find at runtime in the parameter isn't set.
            /// </summary>
            [SerializeField]
            private CameraMOBA _cameraComponent;

            /// <summary>
            /// A list containing all CharacterType which could be considered as targets.
            /// </summary>
            [SerializeField]
            private List<CharacterType> _validTargetTypes = new List<CharacterType>();

            public List<CharacterType> ValidTargetTypes{
                get { return _validTargetTypes; }
            }

            /// <summary>
            /// A list which contains the selected special robots.
            /// </summary>
            private List<SpecialRobot> _selected = new List<SpecialRobot>();

            /// <summary>
            /// This boolean is true if herbie is launching a active skill.
            /// ie : if herbie is waiting the player to click somewhere on the ground to trigger an active skill. 
            /// </summary>
            private bool _isCastingSkill = false;

            public bool IsCastingSkill
            {
                get { return _isCastingSkill; }
            }


            private ActiveSkill _skillToCast;
            private SpecialRobot _skillCaster;
            private HUDSkillButton _skillButton;

            [Header("Cast skill cursor")]

            [SerializeField]
            private Texture2D _castSkillCursorVisual;
            [SerializeField]
            private Vector2 _castSkillCursorHotSpot = Vector2.zero;
            [SerializeField]
            private CursorMode _castSkillCursorMode = CursorMode.Auto;


            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            void Start()
            {
                //try to fill missing parameters : 
                if (_hudHerbie == null)
                {
                    _hudHerbie = FindObjectOfType<HUDHerbie>();
                }
                if(_cameraComponent == null)
                {
                    _cameraComponent = GetComponent<CameraMOBA>();
                }
            }

            public void prepareSkillCast(ActiveSkill skillToCast, SpecialRobot skillCaster, HUDSkillButton skillButton)
            {
                // if the skill isn't ready to be used, do nothing
                if (!skillToCast.Activable)
                    return;

                // set herbie to cast skill mode 
                _isCastingSkill = true;
                _skillToCast = skillToCast;
                _skillCaster = skillCaster;
                _skillButton = skillButton;

                //directly cast the skill if the skill applies on the robot
                if (skillToCast.SkillOnSelf)
                {
                    castSkill(_skillCaster.transform.position, Input.GetKeyDown(KeyCode.LeftShift));
                    return;
                }

                // change cursor visual
                Cursor.SetCursor(_castSkillCursorVisual, _castSkillCursorHotSpot, _castSkillCursorMode);

                // launch the beginActivation of the skill
                skillToCast.beginActivation();
            }

            public void castSkill(Vector3 targetPosition, bool queued = false)
            {
                // Give the unit the order to cast the active skill to the given position
                attachCommandToSingleUnit(_skillCaster, new CommandSkillCast(_skillToCast, targetPosition), queued);

                //callback on the button, to display the coolDown effect on HUD.
                _skillButton.OnActiveSkill();

                cancelSkillCast();
            }

            public void cancelSkillCast()
            {
                // set default cursor visual
                Cursor.SetCursor(null, _castSkillCursorHotSpot, _castSkillCursorMode);

                // set herbie to normal mode 
                _isCastingSkill = false;
                _skillToCast = null;
                _skillCaster = null;
                _skillButton = null;
            }

            /// <summary>
            /// Update the selection.
            /// </summary>
            /// <param name="newSelection"> The new selection of units. </param>
            public void changeSelection(List<SpecialRobot> newSelection)
            {
                _selected.Clear();

                _selected.AddRange(newSelection);

                updateSelectionGUI();
            }

            /// <summary>
            /// Give an order to units which are selected.
            /// </summary>
            /// <param name="command"> The order. </param>
            /// <param name="enqueue"> If true the order will be placed on the order queue. </param>
            public void attachCommandToSelected(Command command, bool enqueue = false)
            {
                if (enqueue)
                {
                    foreach (SpecialRobot robot in _selected)
                    {
                        Command newRobotCommand = command.Clone();
                        newRobotCommand.setActor(robot);
                        robot.addCommand(newRobotCommand);
                    }
                }
                else
                {
                    foreach (SpecialRobot robot in _selected)
                    {
                        Command newRobotCommand = command.Clone();
                        newRobotCommand.setActor(robot);
                        robot.setDirectCommand(newRobotCommand);
                    }
                }
            }

            /// <summary>
            /// Give an order to a single among selected units.
            /// </summary>
            /// <param name="actor"> The unit which will apply the order. </param>
            /// <param name="command"> The order. </param>
            /// <param name="enqueue"> If true the order will be placed on the order queue. </param>
            public void attachCommandToSingleUnit(SpecialRobot actor, Command command, bool enqueue = false)
            {
                Command newRobotCommand = command.Clone();
                newRobotCommand.setActor(actor);
                if (enqueue)
                    actor.addCommand(newRobotCommand);
                else
                    actor.setDirectCommand(newRobotCommand);
            }

            /// <summary>
            /// update the visual of the gui with the new selection.
            /// </summary>
            public void updateSelectionGUI()
            {
                List<CharacterName> selectedNames = new List<CharacterName>();
                foreach (SpecialRobot robot in _selected)
                {
                    selectedNames.Add(robot.getCharacterName());
                }

                _hudHerbie.changeSelection(selectedNames);

                ////clear the gui 
                //int childCount = _GUISelection.childCount;
                //for( int i = 0; i < childCount; i++ )
                //{
                //    Destroy( _GUISelection.GetChild( 0 ).gameObject );
                //}

                ////repopulate gui with the first selected item
                //if( _selected.Count > 0 )
                //{
                //    GameObject newIconeGameObject = new GameObject();
                //    Image newIcone = newIconeGameObject.AddComponent<Image>();

                //    newIcone.sprite = _selected[0].getHUDInfo().Visual;

                //    newIconeGameObject.transform.SetParent( _GUISelection );
                //}
            }

            /// <summary>
            /// Move herbie's camera to the position given in parameters, without changing the y component.
            /// </summary>
            /// <param name="position">The new position of the camera, without taking y coordinate into account.</param>
            public void translateCameraOnYPlane(Vector3 position)
            {
                float yComponent = _cameraComponent.transform.position.y;
                _cameraComponent.transform.position = new Vector3( position.x, yComponent, position.z );
            }

            /// <summary>
            /// return true if herbie has the robot given in parameter in its selection.
            /// </summary>
            /// <param name="robot"></param>
            /// <returns></returns>
            public bool isSelecting(SpecialRobot robot)
            {
                return _selected.Contains( robot );
            }
        }
    }
}
