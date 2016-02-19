// Created by Clement & Maximilien
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Extinction.Characters;

namespace Extinction
{
    namespace AI
    {
        public class Horde : MonoBehaviour
        {

            private List<Creaker> _creakers = new List<Creaker>();
            private int _nbCreakersToUpdate;
            private static GameObject[] _waypoints;



            //Gestion de la horde
            private static List<int> _groups = new List<int>();
            private int _nbGroups = 1;
            [SerializeField] private static List<Transform> _groupTarget = new List<Transform>();
            private static List<Character> _groupCharacterTarget = new List<Character>();
            private static List<int> _targetLost = new List<int>();


            public void Awake()
            {
                _groups.Add(0);
                _waypoints = GameObject.FindGameObjectsWithTag("waypoint");
                _groupCharacterTarget.Add(null);
                _groupTarget.Add(_waypoints[0].transform);
                _targetLost.Add(0);

            }

            public void Update()
            {
                //foreach (Transform pos in _groupTarget){}

                for (int i = 1; i < _groupTarget.Count; ++i)
                {
                    if (_targetLost[i] <= 0 && _groupCharacterTarget[i] != null)
                    {
                        _groupCharacterTarget[i] = null;
                        _targetLost[i] = 0;
                        _groupTarget[i] = _waypoints[UnityEngine.Random.Range(0, _waypoints.Length)].transform;
                    }
                       
                    if (_groupCharacterTarget[i] != null) _groupTarget[i] = _groupCharacterTarget[i].transform;
                    //if (_groupCharacterTarget[i] == null) _groupTarget[i] = _waypoints[UnityEngine.Random.Range(0, _waypoints.Length)].transform;
                    //Debug.LogError("group n°: " + i + " nombre: " + _groups[i]);
                    //Debug.LogError("character target: " + i + " --> " + _groupCharacterTarget[i]);
                    Debug.LogError("wayPoint: " + i + " --> " + _groupTarget[i].position);
                    
        
                    
                }

            }

            static public int getGroupSize(int idGroup)
            {
                return _groups[idGroup];
            }

            static public void addOneCreaker(int idGroup)
            {
                _groups[idGroup]++;
            }

            static public void removeOneCreaker(int idGroup)
            {
                _groups[idGroup]--;
            }

            static public int createNewGroup()
            {
                _groups.Add(2);
                _groupTarget.Add(_waypoints[UnityEngine.Random.Range(0, _waypoints.Length)].transform);
                _groupCharacterTarget.Add(null);
                _targetLost.Add(0);
                return _groups.Count - 1;

            }

            static public int createNewGroup(Character c, int nb)
            {
                _groups.Add(nb);
                // _groupTarget.Add(_waypoints[UnityEngine.Random.Range(0, _waypoints.Length)].transform);
                _groupTarget.Add(null);
                _groupCharacterTarget.Add(c);
                _targetLost.Add(0);
                return _groups.Count - 1;   
            }


            static public Transform getGroupTarget(int idGroup)
            {
                return _groupTarget[idGroup];
            }

            static public void setCharacterTarget(Character c, int id)
            {
                _groupCharacterTarget[id] = c;
            }

            static public void addTargetLost(int idGroup)
            {
                
                _targetLost[idGroup]--;
                Debug.LogError("target lost " + idGroup + " --> " + _targetLost[idGroup]);
            }

            static public void targetFound(int idGroup)
            {
                _targetLost[idGroup]++;
            }

            static public bool targetIsSurvivor(int idGroup)
            {
                return _groupCharacterTarget == null;
            }

            static public void setNewWaypoint(int idGroup)
            {
                Debug.LogError("NEW WP");
                _groupTarget[idGroup] = _waypoints[UnityEngine.Random.Range(0, _waypoints.Length)].transform;
            }


        }
    }
}