// @author : florian

using UnityEngine;
using System.Collections;
using System;

namespace Extinction
{
    namespace Cameras
    {
        public class CameraMOBA : CameraModule
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// if mouse pointer X position is greater than screenWidth - m_rightLimit, move the camera to the right
            /// </summary>
            [SerializeField]
            private int m_rightLimit = 10;


            /// <summary>
            /// if mouse pointer X position is less than m_leftLimit, move the camera to the left
            /// </summary>
            [SerializeField]
            private int m_leftLimit = 10;

            /// <summary>
            /// if mouse pointer Y position is less than m_topLimit, move the camera to the top
            /// </summary>
            [SerializeField]
            private int m_topLimit = 10;

            /// <summary>
            /// if mouse pointer Y position is more than screenHeight - m_topLimit, move the camera to the bottom
            /// </summary>
            [SerializeField]
            private int m_bottomLimit = 10;

            /// <summary>
            /// velocity of the camera
            /// </summary>
            [SerializeField]
            private float m_velocity = 1.0F;

            [SerializeField]
            private float m_zoomMin = 10.0F;

            [SerializeField]
            private float m_zoomMax = 90.0F;

            [SerializeField]
            private float m_zoomStep = 0.1F;

            [SerializeField]
            private float m_currentZoom = 60.0F;

            [SerializeField]
            private bool m_hasChildCamera = true;

            private Camera m_childCamera;

            private Vector2 m_direction;

            private Camera m_thisCamera;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            void Awake()
            {
                m_thisCamera = GetComponent<Camera>();
                _fieldOfView = m_thisCamera.fieldOfView;

                //search into child object to find a child camera.
                //if there is such a camera, store it in m_childCamera
                if( m_hasChildCamera )
                {
                    Camera[] cams = GetComponentsInChildren<Camera>();
                    foreach( Camera cam in cams )
                    {
                        if( cam != m_thisCamera )
                        {
                            m_childCamera = cam;
                            //synchronize the field of view for the second camera : 
                            m_childCamera.fieldOfView = _fieldOfView;
                            break;
                        }
                    }
                }

                m_currentZoom = m_zoomMax;

                zoom( 0 );
            }

            public override void setFieldOfView( float fieldOfView )
            {
                //set fieldOfView member variable
                _fieldOfView = Mathf.Clamp( fieldOfView, m_zoomMin, m_zoomMax );

                //set fieldOfView of herbie's cameras
                m_thisCamera.fieldOfView = fieldOfView;

                if( m_hasChildCamera )
                    m_childCamera.fieldOfView = fieldOfView;
            }

            /// <summary>
            /// Make a zoom in "time" second. 
            /// The longer the time is, the smaller the deltaTime of the coroutine is, 
            /// with the following convention : One step each 0.1 second.
            /// </summary>
            /// <param name="targetFieldOfView"></param>
            /// <param name="time"></param>
            public override void zoom( float targetFieldOfView, float time )
            {
                //begin the coroutine, for a smooth zoom.
                StartCoroutine( zoomCoroutine(targetFieldOfView, 0.1f/time) );
            }

            /// <summary>
            /// A coroutine to perform a smooth zoom.
            /// </summary>
            /// <param name="targetFieldOfView"> The desired field of view at the end of zoom processing. </param>
            /// <param name="deltaTime"> The time between two calls of the routine. </param>
            /// <returns></returns>
            public IEnumerator zoomCoroutine( float targetFieldOfView, float deltaTime)
            {
                float time = 0;
                float begineFieldOfView = _fieldOfView;
                while( Mathf.Approximately( _fieldOfView,  targetFieldOfView) 
                    && _fieldOfView > m_zoomMin && _fieldOfView < m_zoomMax ) //between zoomMin and zoomMax
                {
                    _fieldOfView = Mathf.Lerp( begineFieldOfView, targetFieldOfView, time );
                    yield return new WaitForSeconds( deltaTime );
                    time += deltaTime;
                }
            }

            /// <summary>
            /// change the zoom of the camera, changing its field of view.
            /// This methode doesn't perform a smooth zoom. It directly set the new value for the field of view.
            /// For a smooth zoom, see the "public override void zoom( float targetFieldOfView, float time )" methode.
            /// </summary>
            /// <param name="delta"> camera's field of view will be incremented/decremented with this value. </param>
            void zoom( float delta )
            {
                if( delta < 0 )
                {
                    m_currentZoom += m_zoomStep;
                }
                else if( delta > 0 )
                {
                    m_currentZoom -= m_zoomStep;
                }

                setFieldOfView( m_currentZoom );
            }

            /// <summary>
            /// Directly changes the camera's field of view.
            /// For a smooth zoom, see the "public override void zoom( float targetFieldOfView, float time )" methode.
            /// </summary>
            /// <param name="zoomValue"></param>
            public void setZoom( float zoomValue )
            {
                m_currentZoom = zoomValue;

                setFieldOfView( m_currentZoom );
            }

            /// <summary>
            /// Update the new position of the camera, based on the mouse position on screen. 
            /// </summary>
            /// <param name="position"> The mouse position on screen. </param>
            public override void setPosition( Vector3 position )
            {
                //initialisation of the direction
                m_direction = Vector2.zero;

                //update of the direction of the camera, based on the mouse position.
                //No use of esle, because more than on instructions may be valid (diagonal direction)
                if( position.x > m_thisCamera.pixelWidth - m_rightLimit )
                {
                    m_direction += new Vector2( m_velocity, 0 );
                }
                if( position.x < m_leftLimit )
                {
                    m_direction += new Vector2( -m_velocity, 0 );
                }
                if( position.y < m_topLimit )
                {
                    m_direction += new Vector2( 0, -m_velocity );
                }
                if( position.y > m_thisCamera.pixelHeight - m_bottomLimit )
                {
                    m_direction += new Vector2( 0, m_velocity );
                }

                //move the camera
                transform.position += new Vector3( m_direction.x, 0, m_direction.y );
            }

            public override void setRotation( Vector3 rotation )
            {
                //nothing 
                //no camera rotation for herbie's camera
            }           
        }
    }
}
