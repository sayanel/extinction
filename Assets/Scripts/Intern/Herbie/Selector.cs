// @author : florian

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
            public Transform CameraTransform { get { return _cameraTransform; } set { _cameraTransform = value; } }

            private Vector3 _cameraBeginPosition;

            [SerializeField]
            private Characters.Herbie _herbie;
            public Characters.Herbie Herbie { get { return _herbie; } set { _herbie = value; } }


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

                //try to fill missing parameters : 
                if(_herbie == null)
                {
                    _herbie = FindObjectOfType<Characters.Herbie>();
                }
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

                _beginPoint.x = _beginPointScreenSpace.x;
                _beginPoint.y = _beginPointScreenSpace.y;
                _endPoint.x = _endPointScreenSpace.x;
                _endPoint.y = _endPointScreenSpace.y;

                clearSelection();

                Ray selectionRay = Camera.main.ScreenPointToRay( Input.mousePosition );
                RaycastHit hitInfo;
                if( Physics.Raycast( selectionRay, out hitInfo, 10000, LayerMask.GetMask( "Terrain" ) ) )
                {
                    _triggerAnchor = hitInfo.point;

                    transform.position = _triggerAnchor;

                    transform.localScale = new Vector3( 0.01f, _triggerHeight, 1.01f );
                }

                _thisTrigger.enabled = true;
            }

            public void UpdateSelection()
            {
                Vector3 a = Camera.main.ScreenToWorldPoint( new Vector3( 1, 1, 1 ) );

                Vector3 cameraPosBegin = _cameraBeginPosition;
                Vector3 cameraPosEnd = _cameraTransform.position;
                Debug.Log( "cameraPosBegin : " + cameraPosBegin );
                Debug.Log( "cameraPosEnd : " + cameraPosEnd );
                Vector3 cameraOffset = Vector3.Scale( cameraPosEnd - cameraPosBegin, a);

                _beginPoint.x = Mathf.Abs(-_beginPointScreenSpace.x + cameraOffset.x);
                _beginPoint.y = Mathf.Abs(-_beginPointScreenSpace.y + cameraOffset.z);

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
                    if( other.CompareTag( tag ) && !other.isTrigger )
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
                    if( other.CompareTag( tag ) && !other.isTrigger )
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
