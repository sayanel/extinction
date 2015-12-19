// @author : florian

using UnityEngine;
using System.Collections;
using System;

using Extinction.Cameras;


namespace Extinction
{
    namespace Controllers
    {
        public class InputControllerherbie : InputController
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            [SerializeField]
            private CameraMOBA _herbieCameraComponent = null;

            [SerializeField]
            private Characters.Herbie _herbieComponent = null;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            void Awake()
            {
                if( _herbieCameraComponent == null )
                    _herbieCameraComponent = GetComponent<CameraMOBA>();

                if( _herbieComponent == null )
                    _herbieComponent = GetComponent<Characters.Herbie>();
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

                //Selector Inputs

                //SpecialRobots Inputs
            }
        }
    }
}
