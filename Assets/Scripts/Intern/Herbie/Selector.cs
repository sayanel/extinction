// @author : 

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Extinction.Characters;
using Extinction.Utils;

namespace Extinction
{
    namespace Herbie
    {
        [RequireComponent( typeof( BoxCollider ), typeof( Rigidbody ) )]
        public class Selector : MonoBehaviour
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            [SerializeField]
            private float _triggerHeight = 200;

            private bool _selecting = false;

            public bool Selecting{
                get{ return _selecting; }
                set{ _selecting = value; }
            }

            private Vector2 _beginPoint;
            private Vector2 _endPoint;

            private BoxCollider _thisTrigger;

            private Vector3 _triggerAnchor;

            [SerializeField]
            private List<string> _selectableTags = new List<string>();

            [SerializeField]
            private List<SpecialRobot> _selected = new List<SpecialRobot>();

            //GUI which will display the current selection
            [SerializeField]
            private RectTransform _GUISelection;


            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            void Awake()
            {
                _thisTrigger = GetComponent<BoxCollider>();
                _thisTrigger.size = new Vector3( 1, 1, 1 );
                _thisTrigger.center = new Vector3( 0.5F, 0, 0.5F );
                _thisTrigger.isTrigger = true;

                Rigidbody thisRigidbody = GetComponent<Rigidbody>();
                thisRigidbody.useGravity = false;
                thisRigidbody.isKinematic = true;
            }

            void Start()
            {
                transform.rotation = Quaternion.identity;
            }

            //remove the controle we have on each agent of the previous selection. 
            public void clearSelection()
            {
                _selected.Clear();
            }

            public void BeginSelection()
            {
                _selecting = true;

                _beginPoint.x = Input.mousePosition.x;
                _beginPoint.y = Input.mousePosition.y;

                _endPoint.x = Input.mousePosition.x;
                _endPoint.y = Input.mousePosition.y;

                clearSelection();

                Ray selectionRay = Camera.main.ScreenPointToRay( Input.mousePosition );
                RaycastHit hitInfo;
                if( Physics.Raycast( selectionRay, out hitInfo, 10000, LayerMask.GetMask( "Terrain" ) ) )
                {
                    _triggerAnchor = hitInfo.point;

                    transform.position = _triggerAnchor;

                    transform.localScale = new Vector3( 1, _triggerHeight, 1 );
                }

                _thisTrigger.enabled = true;
            }

            public void UpdateSelection()
            {
                _endPoint.x = Input.mousePosition.x;
                _endPoint.y = Input.mousePosition.y;

                Ray selectionRay = Camera.main.ScreenPointToRay( Input.mousePosition );
                RaycastHit hitInfo;
                if( Physics.Raycast( selectionRay, out hitInfo, 10000, LayerMask.GetMask( "Terrain" ) ) )
                {
                    Vector3 diagVector = hitInfo.point - _triggerAnchor;

                    transform.position = _triggerAnchor;

                    transform.localScale = new Vector3( diagVector.x, _triggerHeight, diagVector.z );

                }
            }

            public void EndSelection()
            {
                _selecting = false;

                _endPoint.x = Input.mousePosition.x;
                _endPoint.y = Input.mousePosition.y;

                _thisTrigger.enabled = false;

                //updateGUI();
            }

            //update the visual of the gui with the new selection.
            public void updateGUI()
            {
                // TO COMPLETE

                //clear the gui 
                int childCount = _GUISelection.childCount;
                for( int i = 0; i < childCount; i++ )
                {
                    Destroy( _GUISelection.GetChild( 0 ).gameObject );
                }

                //repopulate gui with the first selected item
                if( _selected.Count > 0 )
                {
                    GameObject newIconeGameObject = new GameObject();
                    Image newIcone = newIconeGameObject.AddComponent<Image>();

                    newIcone.sprite = _selected[0].getHUDInfo().Visual;

                    newIconeGameObject.transform.SetParent( _GUISelection );
                }
            }

            void OnTriggerEnter( Collider other )
            {
                foreach( string tag in _selectableTags )
                {
                    if( other.CompareTag( tag ) )
                    {
                        SpecialRobot selectedAgent = other.GetComponent<SpecialRobot>();
                        if( selectedAgent != null && !_selected.Contains( selectedAgent ) )
                        {
                            _selected.Add( selectedAgent );
                        }
                    }
                }
            }

            void OnTriggerExit( Collider other )
            {
                foreach( string tag in _selectableTags )
                {
                    if( other.CompareTag( tag ) )
                    {
                        SpecialRobot selectedAgent = other.GetComponent<SpecialRobot>();
                        if( selectedAgent != null )
                        {
                            _selected.Remove( selectedAgent );
                        }
                    }
                }
            }

            void OnGUI()
            {
                if( _selecting )
                    GUIUtils.DrawScreenRectBorder( new Rect( _beginPoint.x, Camera.main.pixelHeight - _beginPoint.y, _endPoint.x - _beginPoint.x, _beginPoint.y - _endPoint.y ), 1, Color.red );
            }

            public void attachCommandToSelected( Command command, bool enqueue = false )
            {
                if(enqueue)
                {
                    foreach(SpecialRobot robot in _selected)
                    {
                        Command newRobotCommand = command.Clone();
                        newRobotCommand.setActor( robot );
                        robot.addCommand( newRobotCommand );
                    }
                }
                else
                {
                    foreach( SpecialRobot robot in _selected )
                    {
                        Command newRobotCommand = command.Clone();
                        newRobotCommand.setActor( robot );
                        robot.setDirectCommand( newRobotCommand );
                    }
                }
            }
        }
    }
}
