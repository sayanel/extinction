﻿// @author : 

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
            private Vector2 _beginPointScreenSpace;
            private Vector2 _endPoint;
            private Vector2 _endPointScreenSpace;

            private BoxCollider _thisTrigger;

            private Vector3 _triggerAnchor;

            [SerializeField]
            private List<string> _selectableTags = new List<string>();

            [SerializeField]
            private List<SpecialRobot> _selected = new List<SpecialRobot>();

            [SerializeField]
            private Transform _cameraTransform;

            private Vector3 _cameraBeginPosition;

            [SerializeField]
            private Characters.Herbie _herbie;


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

                _cameraBeginPosition = _cameraTransform.position;

                _beginPointScreenSpace.x = Input.mousePosition.x;
                _beginPointScreenSpace.y = Input.mousePosition.y;

                _endPointScreenSpace.x = Input.mousePosition.x;
                _endPointScreenSpace.y = Input.mousePosition.y;

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
                Vector3 cameraPosBegin = Camera.main.WorldToScreenPoint(_cameraBeginPosition);
                Vector3 cameraPosEnd = Camera.main.WorldToScreenPoint(_cameraTransform.position);
                Vector3 cameraOffset = cameraPosEnd - cameraPosBegin;

                _beginPoint.x = _beginPointScreenSpace.x - cameraOffset.x;
                _beginPoint.y = _beginPointScreenSpace.y - cameraOffset.y;

                _endPointScreenSpace.x = Input.mousePosition.x;
                _endPointScreenSpace.y = Input.mousePosition.y;

                _endPoint.x = _endPointScreenSpace.x;
                _endPoint.y = _endPointScreenSpace.y;

                Debug.Log("beginPoint = "+_beginPoint);
                Debug.Log("endPoint = "+_endPoint);
                Debug.Log("beginPointScreenSpace = " + _beginPointScreenSpace);
                Debug.Log("endPointScreenSpace = " + _endPointScreenSpace);


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
                _thisTrigger.enabled = false;

                //send the new selection to herbie, and clear the selection of selector.
                _herbie.changeSelection(_selected);
                _selected.Clear();
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
                    GUIUtils.DrawScreenRectBorder( new Rect( _beginPoint.x, Camera.main.pixelHeight - _beginPoint.y, _endPoint.x - _beginPoint.x , _beginPoint.y - _endPoint.y ), 1, Color.red );
            }
        }
    }
}
