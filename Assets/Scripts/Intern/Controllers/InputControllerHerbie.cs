﻿// @author : florian

using UnityEngine;
using System.Collections;
using System;

using Extinction.Cameras;
using Extinction.Herbie;
using Extinction.Characters;


namespace Extinction
{
    namespace Controllers
    {
        public class InputControllerHerbie : InputController
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            //small struct to retrieve the information of the entity the mouse has clicked on
            struct MouseTargetInfo
            {
                public Vector3 position;
                public GameObject gameObject;
                public string tag;
            }

            //used to store information on the entity the mouse has clicked on (if any)
            MouseTargetInfo m_mouseTargetInfo;

            [SerializeField]
            private CameraMOBA _herbieCameraComponent = null;

            [SerializeField]
            private Characters.Herbie _herbieComponent = null;

            [SerializeField]
            private Selector _selectorComponent = null;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            void Awake()
            {
                if( _herbieCameraComponent == null )
                    _herbieCameraComponent = GetComponent<CameraMOBA>();

                if( _herbieComponent == null )
                    _herbieComponent = GetComponent<Characters.Herbie>();

                if( _selectorComponent == null )
                {
                    _selectorComponent = GetComponentInChildren<Selector>();
                }
            }

            void Update()
            {
                processUserInputs();
            }

            public override void processUserInputs()
            {
                //Camera Inputs : 

                //zoom
                if( !Mathf.Approximately( 0.0F, Input.GetAxis( "Mouse ScrollWheel" ) ) )
                {
                    _herbieCameraComponent.zoomSmooth( Input.GetAxis( "Mouse ScrollWheel" ) );
                }

                //position
                _herbieCameraComponent.setPosition( Input.mousePosition );

                //Selector Inputs : 
                if( Input.GetMouseButtonDown( 0 ) )
                {
                    _selectorComponent.BeginSelection();
                }
                else if( _selectorComponent.Selecting )
                {
                    if( Input.GetMouseButtonUp( 0 ) )
                    {
                        _selectorComponent.EndSelection();
                    }
                    else
                    {
                        _selectorComponent.UpdateSelection();
                    }
                }

                //SpecialRobots Inputs
                if( Input.GetMouseButtonDown( 1 ) )
                {
                    if( checkMouseTarget( out m_mouseTargetInfo ) ) // first step : rayCast and store information on the mouseTargetInfo
                    {
                        Debug.Log( "mouse encounter a target with tag : " + m_mouseTargetInfo.tag.ToString() );

                        if( m_mouseTargetInfo.tag == "Target" )
                        {
                            //store a pointer to the current target
                            Character currentTarget = m_mouseTargetInfo.gameObject.GetComponent<Character>();

                            //launch a coroutine for attack behaviour
                            if( currentTarget != null )
                            {
                                //MoveAndAttack();
                                _selectorComponent.attachCommandToSelected( new CommandMoveAndAttack( currentTarget, 0.5f ), Input.GetKey(KeyCode.LeftShift) );
                                //AttachNewCommand( new MoveAndAttackCommand( _agent, currentTarget, 0.5f ) );
                            }
                        }
                        else
                        {
                            //Move( m_mouseTargetInfo.position );
                            _selectorComponent.attachCommandToSelected( new CommandMove( m_mouseTargetInfo.position ), Input.GetKey( KeyCode.LeftShift ) );
                        }
                    }
                }
            }


            /// <summary>
            /// fill the struct passed as parameter with informations concerning the mouse click :
            /// click position, click on an entity ? , ...  
            /// </summary>
            bool checkMouseTarget( out MouseTargetInfo info )
            {
                Ray selectionRay = Camera.main.ScreenPointToRay( Input.mousePosition );
                RaycastHit hitInfo;
                if( Physics.Raycast( selectionRay, out hitInfo ) )
                {
                    info.gameObject = hitInfo.collider.gameObject;
                    info.position = hitInfo.point;
                    info.tag = hitInfo.collider.tag;
                    return true;
                }
                else
                {
                    info.gameObject = null;
                    info.position = Vector3.zero;
                    info.tag = "none";
                    return false;
                }
            }



        }
    }
}
