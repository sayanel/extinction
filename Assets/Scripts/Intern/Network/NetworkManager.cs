﻿// @author: Alex
using UnityEngine;
using System.Collections;
using Extinction.Characters;
using Extinction.Controllers;
using Extinction.UI;

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

            public AsyncLoading asyncLoadingScript;

            public void Awake() {
                DontDestroyOnLoad(this);
            }

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
                PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 5}, TypedLobby.Default);
            }

            public override void OnPhotonJoinRoomFailed(object[] codeAndMsg) {
                base.OnPhotonJoinRoomFailed(codeAndMsg);
                Debug.Log("The room cannot be joined: " + codeAndMsg.ToString());
            }

            public override void OnJoinedRoom() {
                base.OnJoinedRoom();
                Debug.Log(PhotonNetwork.player.name + " player rejoined the room " + PhotonNetwork.room.name);
                CreateCharacter("FPSPlayer", new Vector3(100,100,100), Quaternion.identity);
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
                //Application.LoadLevel(INDEX_SCENE_GAME);
                if(asyncLoadingScript != null)
                    asyncLoadingScript.StartLoading(INDEX_SCENE_GAME);
                //CreateCharacter("FPSPlayer", new Vector3(1000,1000,1000), Quaternion.identity);
            }

            public void CreateCharacter(string prefabName, Vector3 pos, Quaternion rot) {
                GameObject go = PhotonNetwork.Instantiate(prefabName, pos, rot, 0);
                DontDestroyOnLoad(go);
                PhotonNetwork.player.TagObject = go;
                ((INetworkInitializerPrefab)(go.GetComponent<Survivor>())).Activate();
            }
        }
    }
}
