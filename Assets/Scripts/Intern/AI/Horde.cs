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
            private int nbCreaker = 500;
            static protected NavMeshAgent _nav; // Reference to the nav mesh agent.

            private List<Creaker> _creakers = new List<Creaker>();
            private int _nbCreakersToUpdate = 500;
            private int creakerIndex = 0;
            [SerializeField]
            private static GameObject[] _waypoints;         

            [SerializeField] public GameObject creakerPrefab;

            //Gestion de la horde
            private static List<int> _groups = new List<int>();
            private int _nbGroups = 1;
            [SerializeField] private static List<Transform> _groupTarget = new List<Transform>();
            private static List<Character> _groupCharacterTarget = new List<Character>();
            private static List<int> _targetLost = new List<int>();

            private GameObject creakerGO;




            public void Start()
            {
                _groups.Add(0);
                _waypoints = GameObject.FindGameObjectsWithTag("waypoint");
                _groupCharacterTarget.Add(null);
                _groupTarget.Add(_waypoints[UnityEngine.Random.Range(0, _waypoints.Length)].transform);
                _targetLost.Add(0);
                _nav = GetComponent<NavMeshAgent>();

                createHorde(nbCreaker);

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
                    
                }
                int j;
                for(j = 0; j < creakerIndex + _nbCreakersToUpdate; ++j)
                {
                    if (_creakers[(j + creakerIndex) % nbCreaker]._isDead)
                    {
                        _creakers.RemoveAt(j + creakerIndex);
                        --j;
                        continue;
                    }

                    _creakers[(j+creakerIndex)%nbCreaker].UpdateCreaker();
                }
                creakerIndex = (creakerIndex + j)%nbCreaker;
                //Debug.LogError("Index: " + creakerIndex);
                //seeGroups();
            }

            public void seeGroups()
            {
                for(int i=0; i<_groups.Count; ++i)
                {
                    if (_groups[i] > 1)
                        Debug.LogError("Groupe n° " + i + " with " + _groups[i] + " creakers");
                 }
               
            }

            public bool getSpawnPos(out Vector3 pos)
            {
                int layerMask = (1 << NavMesh.GetAreaFromName("Walkable"));
                var position = new Vector3(Random.Range(340, 650), 20, Random.Range(350, 700));

            
                NavMeshHit hit;
                bool isHit = NavMesh.SamplePosition(position, out hit, 50f, layerMask);
                pos = hit.position;
                return isHit;
            }

            public Creaker createCreaker(Vector3 position)
            {
                creakerGO = PhotonNetwork.Instantiate("Creaker", position, Quaternion.identity, 0) as GameObject;
                DontDestroyOnLoad(creakerGO);

                Creaker creaker = creakerGO.GetComponent<Creaker>();
                creaker.init();
                return creaker;
            }

            public void createHorde(int nbCreakers)
            {
                Vector3 pos;

                for(int i=0; i<nbCreakers; ++i)
                {   
                    while(!getSpawnPos(out pos)) { }
                    _creakers.Add(createCreaker(pos));
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

            static public Transform getWayPoint()
            {
                return _waypoints[UnityEngine.Random.Range(0, _waypoints.Length)].transform;
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
                //Debug.LogError("target lost " + idGroup + " --> " + _targetLost[idGroup]);
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
                //Debug.LogError("NEW WP");
                _groupTarget[idGroup] = _waypoints[UnityEngine.Random.Range(0, _waypoints.Length)].transform;
            }


        }
    }
}