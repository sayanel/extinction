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
            [SerializeField]
            private int nbCreaker = 20;
            //static protected NavMeshAgent _nav; // Reference to the nav mesh agent.
             
            private List<Creaker> _creakers = new List<Creaker>();
            [SerializeField]
            private int _nbCreakersToUpdate = 20;
            private int creakerIndex = 0;
            [SerializeField]
            private static GameObject[] _waypoints;      

            [SerializeField]
            public GameObject creakerPrefab;

            //Gestion de la horde
            private static List<int> _groups = new List<int>();
            private int _nbGroups = 1;
            [SerializeField] private static List<Transform> _groupTarget = new List<Transform>();
            private static List<Character> _groupCharacterTarget = new List<Character>();
            private static List<int> _targetLost = new List<int>();

            private GameObject creakerGO;

            //the dimension of the terrain, use to find spawn positions of creakers : 
            [SerializeField]
            Vector2 terrainRangeX = new Vector2( 340, 650 );
            [SerializeField]
            Vector2 terrainRangeY = new Vector2( 350, 700 );
            [SerializeField] private static int _counterSetNewWP;
            [SerializeField] private static int _counterMaxSetNewWP;
            [SerializeField] private static bool _setNewWP;


            public void Start()
            {
                _groups.Add(0);
                _waypoints = GameObject.FindGameObjectsWithTag("waypoint");
                _groupCharacterTarget.Add(null);
                _groupTarget.Add(_waypoints[UnityEngine.Random.Range(0, _waypoints.Length)].transform);
                _targetLost.Add(0);
                //_nav = GetComponent<NavMeshAgent>();
                _counterSetNewWP = 0;
                _counterMaxSetNewWP = 500;
                _setNewWP = false;

                createHorde(nbCreaker);

                StartCoroutine(delayedUpdate());
            }

            public void Update()
            {
                //foreach (Transform pos in _groupTarget){}
                //_nbCreakersToUpdate = nbCreaker;
                //Lost survivor?
                /*for (int i = 1; i < _groupTarget.Count; ++i)
                {
                    if (_targetLost[i] <= 0 && _groupCharacterTarget[i] != null)
                    {
                        _groupCharacterTarget[i] = null;
                        _targetLost[i] = 0;
                        _groupTarget[i] = getWayPoint();
                    } 
                       
                    if (_groupCharacterTarget[i] != null)
                        _groupTarget[i] = _groupCharacterTarget[i].transform;
                    
                }*/

                for (int j = 0; j < nbCreaker; ++j)
                {
                    //play creaker death :
                    if (_creakers[(j) % nbCreaker].Health <= 0.0001)
                    {
                        //TODO : supprimer les creakers proprement
                        Creaker c = _creakers[(j) % nbCreaker];
                        nbCreaker--;
                        removeOneCreaker(c.getIdGroup());
                        _creakers.RemoveAt((j) % nbCreaker);
                        --j;

                        c.die();
                    }
                }

                    //move fnctionnalities to delayedUpdate() coroutine.

                    //seeGroups();

                    //counterTimeWP(0);
            }

            IEnumerator delayedUpdate()
            {
                while (true)
                {
                    for (int j = 0; j < /*creakerIndex +*/ _nbCreakersToUpdate; ++j)
                    {
                        if(_creakers[(j + creakerIndex) % nbCreaker].Health > 0.0001)
                        {
                            //update creaker if he is not dead : 
                            _creakers[(j + creakerIndex) % nbCreaker].UpdateCreaker();
                        }
                    }
                    creakerIndex = (creakerIndex + _nbCreakersToUpdate) % nbCreaker;

                    yield return new WaitForSeconds(0.5f);
                }
            }

            //for debuging.
            public void seeGroups()
            {
                for (int i = 0; i < _groups.Count; ++i)
                {
                    if (_groups[i] > 1)
                    {
                        Debug.LogError("Groupe n° " + i + " with " + _groups[i] + " creakers" + " target: " + _groupTarget[i] );
                    }
                }
               
            }

            public bool getSpawnPos(out Vector3 pos)
            {
                int layerMask = (1 << NavMesh.GetAreaFromName("Walkable"));
                var position = new Vector3( Random.Range( terrainRangeX.x, terrainRangeX.y ), 1/*20*/, Random.Range( terrainRangeY.x, terrainRangeY.y ) );//new Vector3(Random.Range(340, 650), 20, Random.Range(350, 700));

            
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
                //creakerGO = PhotonNetwork.Instantiate("Creaker", position, Quaternion.identity, 0) as GameObject;
                //DontDestroyOnLoad(creakerGO);
                return creaker;

                //Creaker creaker = creakerGO.GetComponent<Creaker>();
                //creaker.init();
                //return creaker;
            }

            public void localInitialization()
            {
                StartCoroutine(localInitializationRoutine());
            }

            IEnumerator localInitializationRoutine()
            {
                int testNb = 0;

                while(_creakers.Count < nbCreaker && testNb < 4)
                {
                    yield return new WaitForSeconds(0.5f);
                    testNb++;
                }

                foreach (Creaker creaker in _creakers)
                {
                    creaker.detectorCollider.SetActive(true);
                    creaker.rangeCollider.SetActive(true);
                }
            }

            public void createHorde(int nbCreakers)
            {
                Vector3 pos = new Vector3(0,0,0);

                for(int i=0; i<nbCreakers; ++i)
                {
                    bool foundPos = false;
                    for(int j = 0; (j < 100 && !foundPos); j++ ) {
                        getSpawnPos( out pos );
                    }

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
                _groupTarget.Add(getWayPoint());
                _groupCharacterTarget.Add(null);
                _targetLost.Add(0);
                return _groups.Count - 1;

            }

            static public Transform getRandomWaypoint()
            {
               int id = Random.Range(0, _waypoints.Length);
               return _waypoints[id].transform;
            }

            //Si un creaker du groupe 0 croise un survivant
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

            static public void counterTimeWP(int go)
            {

                if (Horde._counterSetNewWP != 0 || _setNewWP == true)
                {
                    _counterSetNewWP++;
                    if (_counterSetNewWP >= _counterMaxSetNewWP)
                    {
                        _counterSetNewWP = 0;
                    }
                    _setNewWP = false;
                }

            }

            static public void setNewWaypoint(int idGroup)
            {

                //if (_counterSetNewWP == 0)
                {
                    _groupTarget[idGroup] = getWayPoint();
                    _setNewWP = true;
                }
                    //Debug.LogError("idGroup: " + idGroup + " -> " + _groups[idGroup] + " new waypoint: " + _groupTarget[idGroup]);
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

            


        }
    }
}