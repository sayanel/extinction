// @author : florian

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Extinction.HUD;
using Extinction.Enums;
using Extinction.Herbie;
using Extinction.Skills;

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
            /// GUI which will display the current selection
            /// </summary>
            [SerializeField]
            private HUDHerbie _hudHerbie;

            /// <summary>
            /// A list which contains the selected special robots.
            /// </summary>
            private List<SpecialRobot> _selected = new List<SpecialRobot>();

            /// <summary>
            /// This boolean is true if herbie is launching a active skill.
            /// ie : if herbie is waiting the player to click somewhere on the ground to trigger an active skill. 
            /// </summary>
            private bool _isCastingSkill = false;

            public bool IsCastingSkill{
                get{ return _isCastingSkill; }
            }

            private ActiveSkill _skillToCast;
            private SpecialRobot _skillCaster; 

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            public void prepareSkillCast(ActiveSkill skillToCast, SpecialRobot skillCaster)
            {
                // if the skill isn't ready to be used, do nothing
                if (!skillToCast.Activable)
                    return;

                // launch the beginActivation of the skill
                skillToCast.beginActivation();

                // set herbie to cast skill mode 
                _isCastingSkill = true;
                _skillToCast = skillToCast;
                _skillCaster = skillCaster;
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
        }
    }
}
