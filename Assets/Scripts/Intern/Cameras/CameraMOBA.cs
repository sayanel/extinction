// @author : florian

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
            /// The height from which the icones will be visible
            /// </summary>
            [SerializeField]
            private float _heightIconeVisible = 30;

            /// <summary>
            /// if mouse pointer X position is greater than screenWidth - m_rightLimit, move the camera to the right
            /// </summary>
            [SerializeField]
            private int _rightLimit = 10;


            /// <summary>
            /// if mouse pointer X position is less than m_leftLimit, move the camera to the left
            /// </summary>
            [SerializeField]
            private int _leftLimit = 10;

            /// <summary>
            /// if mouse pointer Y position is less than m_topLimit, move the camera to the top
            /// </summary>
            [SerializeField]
            private int _topLimit = 10;

            /// <summary>
            /// if mouse pointer Y position is more than screenHeight - m_topLimit, move the camera to the bottom
            /// </summary>
            [SerializeField]
            private int _bottomLimit = 10;

            /// <summary>
            /// velocity of the camera
            /// </summary>
            [SerializeField]
            private float _velocity = 1.0F;
            public float Velocity{
                get{ return _velocity; }
                set{ _velocity = value;}
            }

            [SerializeField]
            private float _zoomSpeed = 0.1F;

            [SerializeField]
            private float _zoomMin = 10.0F;

            [SerializeField]
            private float _zoomMax = 90.0F;

            [SerializeField]
            private float _zoomStep = 0.1F;

            [SerializeField]
            private float _currentZoom = 60.0F;

            [SerializeField]
            private bool _hasChildCamera = true;

            private Camera _childCameraComponent;

            private Vector2 _direction;

            private Camera _cameraComponent;

            private Queue<float> _zoomQueue = new Queue<float>();

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            void Awake()
            {
                _cameraComponent = GetComponent<Camera>();
                _fieldOfView = _cameraComponent.fieldOfView;

                //search into child object to find a child camera.
                //if there is such a camera, store it in m_childCamera
                if( _hasChildCamera )
                {
                    Camera[] cams = GetComponentsInChildren<Camera>();
                    foreach( Camera cam in cams )
                    {
                        if( cam != _cameraComponent )
                        {
                            _childCameraComponent = cam;
                            //synchronize the field of view for the second camera : 
                            _childCameraComponent.fieldOfView = _fieldOfView;
                            break;
                        }
                    }
                }

                _currentZoom = _zoomMax;

                zoom( 0 );
            }

            public override void setFieldOfView( float fieldOfView )
            {
                //set fieldOfView member variable
                _fieldOfView = Mathf.Clamp( fieldOfView, _zoomMin, _zoomMax );

                //set fieldOfView of herbie's cameras
                _cameraComponent.fieldOfView = fieldOfView;

                if( _hasChildCamera )
                    _childCameraComponent.fieldOfView = fieldOfView;
            }

            /// <summary>
            /// Make a zoom in "time" second. 
            /// The longer the time is, the smaller the deltaTime of the coroutine is (ie : more steps in the coroutine), 
            /// with the following convention : One step each 0.1 second.
            /// </summary>
            /// <param name="targetFieldOfView"></param>
            /// <param name="time"></param>
            public override void zoom( float targetFieldOfView, float time )
            {
                //begin the coroutine, for a smooth zoom.
                zoom(targetFieldOfView);
            }

            /// <summary>
            /// Make a zoom in "time" second. 
            /// The longer the time is, the smaller the deltaTime of the coroutine is (ie : more steps in the coroutine), 
            /// with the following convention : One step each 0.1 second.
            /// </summary>
            /// <param name="targetFieldOfView"></param>
            public void zoomSmooth( float targetFieldOfView )
            {
                targetFieldOfView *= _zoomStep;
                //begin the coroutine, for a smooth zoom.
                if (_zoomQueue.Count == 0)
                {
                    _zoomQueue.Enqueue(targetFieldOfView);
                    StartCoroutine(zoomCoroutine(0.1f / _zoomSpeed));
                }
                else
                    _zoomQueue.Enqueue(targetFieldOfView);

            }

            /// <summary>
            /// A coroutine to perform a smooth zoom.
            /// </summary>
            /// <param name="targetFieldOfView"> The desired field of view at the end of zoom processing. </param>
            /// <param name="deltaTime"> The time between two calls of the routine. </param>
            /// <returns></returns>
            public IEnumerator zoomCoroutine( float deltaTime)
            {
                for(int i = 0; i < _zoomQueue.Count; i++)
                {
                    float targetFieldOfView = _zoomQueue.Dequeue() + _fieldOfView;

                    float time = 0;
                    float beginFieldOfView = _fieldOfView;
                    while (!Mathf.Approximately(_fieldOfView, targetFieldOfView))
                    {
                        _fieldOfView = Mathf.Lerp(beginFieldOfView, targetFieldOfView, time);

                        //keep field of view between zoom min and zoom max
                        if (_fieldOfView < _zoomMin)
                        {
                            _fieldOfView = _zoomMin;
                            yield return null;
                        }
                        else if (_fieldOfView > _zoomMax)
                        {
                            _fieldOfView = _zoomMax;
                            yield return null;
                        }

                        time += deltaTime;
                        yield return new WaitForSeconds(deltaTime);
                    }
                }
            }

            /// <summary>
            /// change the zoom of the camera, changing its field of view.
            /// This methode doesn't perform a smooth zoom. It directly set the new value for the field of view.
            /// For a smooth zoom, see the "public override void zoom( float targetFieldOfView, float time )" methode.
            /// </summary>
            /// <param name="delta"> camera's field of view will be incremented/decremented with this value. </param>
            public void zoom( float delta )
            {
                if( delta < 0 )
                {
                    _currentZoom += _zoomStep;
                }
                else if( delta > 0 )
                {
                    _currentZoom -= _zoomStep;
                }

                transform.Translate( 0, 0, delta * _zoomStep, transform );

                Ray ray = new Ray( transform.position, new Vector3( 0, -1, 0 ) );
                RaycastHit hitInfo = new RaycastHit();

                Terrain currentTerrain = Terrain.activeTerrain;
                if( currentTerrain != null )
                {
                    currentTerrain.GetComponent<TerrainCollider>().Raycast( ray, out hitInfo, 1000 );
                }
                else
                    hitInfo.point = new Vector3( transform.position.x, 0, transform.position.z );

                if( Vector3.Distance( hitInfo.point, transform.position ) >= _heightIconeVisible )
                    _cameraComponent.cullingMask |= ( 1 << LayerMask.NameToLayer( "RobotIcone" ) );
                else
                    _cameraComponent.cullingMask &= ~( 1 << LayerMask.NameToLayer( "RobotIcone" ) );
                //setFieldOfView( _currentZoom );
            }

            /// <summary>
            /// Directly changes the camera's field of view.
            /// For a smooth zoom, see the "public override void zoom( float targetFieldOfView, float time )" methode.
            /// </summary>
            /// <param name="zoomValue"></param>
            public void setZoom( float zoomValue )
            {
                _currentZoom = zoomValue;

                setFieldOfView( _currentZoom );
            }

            /// <summary>
            /// Update the new position of the camera, based on the mouse position on screen. 
            /// </summary>
            /// <param name="position"> The mouse position on screen. </param>
            public override void setPosition( Vector3 position )
            {
                //initialisation of the direction
                _direction = Vector2.zero;

                //update of the direction of the camera, based on the mouse position.
                //No use of esle, because more than on instructions may be valid (diagonal direction)
                if( position.x > _cameraComponent.pixelWidth - _rightLimit )
                {
                    _direction += new Vector2( _velocity, 0 );
                }
                if( position.x < _leftLimit )
                {
                    _direction += new Vector2( -_velocity, 0 );
                }
                if( position.y < _topLimit )
                {
                    _direction += new Vector2( 0, -_velocity );
                }
                if( position.y > _cameraComponent.pixelHeight - _bottomLimit )
                {
                    _direction += new Vector2( 0, _velocity );
                }

                //move the camera
                transform.position += new Vector3( _direction.x, 0, _direction.y );
            }

            public override void setRotation( Vector3 rotation )
            {
                //nothing 
                //no camera rotation for herbie's camera
            }           
        }
    }
}
