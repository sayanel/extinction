// @author : florian

using UnityEngine;
using System.Collections;
using System;

namespace Extinction
{
    namespace Controllers
    {
        public class InputControllerherbie : InputController
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            private CameraMOBA _herbieCameraComponent;
            private Herbie _herbieComponent;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            void Awake()
            {

            }

            public override void processUserInputs()
            {
                //Camera Inputs : 

                //zoom
                if( !Mathf.Approximately( 0.0F, Input.GetAxis( "Mouse ScrollWheel" ) ) )
                {
                    _herbieCameraComponent.zoom( Input.GetAxis( "Mouse ScrollWheel" ) );
                }

                //position
                _herbieCameraComponent.setPosition( Input.mousePosition );

                //Selector Inputs

                //SpecialRobots Inputs
            }
        }
    }
}
