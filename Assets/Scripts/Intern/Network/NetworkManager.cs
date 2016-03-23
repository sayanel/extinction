// @author: Alex
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Extinction.Characters;
using Extinction.Controllers;
using Extinction.UI;
using Extinction.AI;

namespace Extinction {
    namespace Network {


        /// <summary>
        /// NetworkManager: Singleton
        /// Entry point for network initialisation: started at the beginning of the game.
        /// Handles also network transactions.
        /// </summary>
        public class NetworkManager: Photon.PunBehaviour { 
            private static NetworkManager _instance = null;
            private static object _lock = new object();

            // scenes index use with PhotonNetwork.LoadLevel => change levels
            // these index are mapped with current build settings
            //INDEX_PRELOAD == 0
            public int INDEX_SCENE_MAIN_MENU = 1;
            public int INDEX_SCENE_PLAYER_CHOICE = 2;
            public int INDEX_SCENE_GAME = 2;
            public int MAX_PLAYER = 5;

            public void Awake() {
                DontDestroyOnLoad(this);
            }

            public GameObject horde;

            public static NetworkManager Instance {
                get {
                    // lock if multithread context
                    lock (_lock) {
                        if (! _instance) {
                            _instance = (NetworkManager) GameObject.FindObjectOfType(typeof(NetworkManager));

                            if (! _instance) {
                                Debug.LogError("The script NetworkManager must be attached to one GameObject");
                            }
                        }
                        return _instance;
                    }
                }
            }

            /// <summary>
            /// Launch the connection to Photon Server (must be PhotonCloud on PhotonSettings).
            /// AppID must be provided into PhotonServerSettings
            /// </summary>
            public void Start() {
                if (!PhotonNetwork.ConnectUsingSettings("v0.1")) {
                    Debug.LogError("Connection to Photon has failed");
                    return;
                }

                PhotonNetwork.automaticallySyncScene = true;
                Debug.Log("Connection to Photon was initialized");
            }

            /// <summary>
            /// A player need to be inside Lobby before any network interaction: debug
            /// </summary>
            public override void OnJoinedLobby() {
                base.OnJoinedLobby();
                //PhotonNetwork.JoinOrCreateRoom(roomName_startMenu, new RoomOptions() { maxPlayers = 50 }, TypedLobby.Default);
                Debug.Log("Network: Lobby Was joined!");
            }

            /// <summary>
            /// Wrapper method of PhotonNetwork.JoinRoom: Use NetworkManagerAPI
            /// </summary>
            /// <param name="roomName"></param>
            public void JoinRoom(string roomName) {
                PhotonNetwork.JoinRoom(roomName);
            }


            public void CreateRoom() {
                if (!PhotonNetwork.insideLobby)
                    throw new System.Exception("Network: The client is not inside the lobby, fail to create room");


                ExitGames.Client.Photon.Hashtable customProp = new ExitGames.Client.Photon.Hashtable() {
                    {"anton", 0},
                    {"red", 0},
                    {"hal", 0},
                    {"herbie", 0},
                    {"malik", 0},
                };

                PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 5, customRoomProperties = customProp}, TypedLobby.Default);
            }

            public override void OnPhotonJoinRoomFailed(object[] codeAndMsg) {
                base.OnPhotonJoinRoomFailed(codeAndMsg);
                Debug.Log("The room cannot be joined: " + codeAndMsg.ToString());
            }

            public override void OnJoinedRoom() {
                base.OnJoinedRoom();
                Debug.Log(PhotonNetwork.player.name + " player rejoined the room " + PhotonNetwork.room.name);
            }

            public override void OnLeftRoom() {
                base.OnLeftRoom();

                if (PhotonNetwork.player.TagObject != null)
                    Destroy((GameObject)PhotonNetwork.player.TagObject);

                PhotonNetwork.player.TagObject = null;
                PhotonNetwork.LoadLevel(INDEX_SCENE_MAIN_MENU);
            }

            public override void OnCreatedRoom() {
                base.OnCreatedRoom();
                Debug.Log(PhotonNetwork.player.name + " player created the room " + PhotonNetwork.room.name);
                Application.LoadLevel(INDEX_SCENE_PLAYER_CHOICE);
            }

            public void LaunchGame(AsyncLoading asyncLoadingScript) {
                asyncLoadingScript.StartLoading(INDEX_SCENE_GAME);
            }

            public void CreateSurvivor(string prefabName, Vector3 pos, Quaternion rot) {
                GameObject go = PhotonNetwork.Instantiate(prefabName, pos, rot, 0);
                DontDestroyOnLoad(go);
                PhotonNetwork.player.TagObject = go;
                ((INetworkInitializerPrefab)(go.GetComponent<Survivor>())).Activate();
            }

            public void CreateHerbie(Vector3 posCamera, Quaternion rot) {
                GameObject herbieGO = Instantiate(Resources.Load<GameObject>("Characters/Herbie/Herbie"), posCamera, rot) as GameObject;
                if (herbieGO == null)
                    throw new System.Exception("Fail when creating Herbie prefab");

                Characters.Herbie herbie = herbieGO.GetComponent<Characters.Herbie>();
                herbie.initialize(new List<string>() { "Characters/RScout", "Characters/RTank", "Characters/RController" }, new List<Vector3>() { new Vector3(0, 0, 0), new Vector3(1, 0, 1), new Vector3(2, 0, 2) }, 0, true);
                PhotonNetwork.player.TagObject = herbieGO;

                CreateHorde();
            }

            public void CreateHorde() {
                // First add to scene on Herbie PC
                GameObject hordeLocal = PhotonNetwork.Instantiate("Horde", Vector3.zero, Quaternion.identity, 0);
                DontDestroyOnLoad(hordeLocal);
            }



            void OnLevelWasLoaded(int level) {
                if (level != INDEX_SCENE_GAME)
                    return;

                Debug.Log("LAUNCH GAME");

                if (PhotonNetwork.player.name != "herbie")
                    CreateSurvivor( "SurvivorAnton", new Vector3( 480, 60, 549 ), Quaternion.identity );
                else
                    CreateHerbie(new Vector3(470, 50, 530), Quaternion.identity);
            }
        }
    }
}
