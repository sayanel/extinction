// @author: Alex
using UnityEngine;
using System.Collections;
using Extinction.Characters;
using Extinction.Controllers;

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

            public override void OnJoinedLobby() {
                base.OnJoinedLobby();
                //PhotonNetwork.JoinOrCreateRoom(roomName_startMenu, new RoomOptions() { maxPlayers = 50 }, TypedLobby.Default);
                Debug.Log("Network: Lobby Was joined!");
            }

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
                PhotonNetwork.LoadLevel(2);
                CreateCharacter("FPSPlayer", Vector3.zero, Quaternion.identity);
            }

            public override void OnLeftRoom() {
                base.OnLeftRoom();

                if (PhotonNetwork.player.TagObject != null)
                    Destroy((GameObject)PhotonNetwork.player.TagObject);

                PhotonNetwork.player.TagObject = null;
                Application.LoadLevel(1);
            }

            public override void OnCreatedRoom() {
                base.OnCreatedRoom();
                Debug.Log(PhotonNetwork.player.name + " player created the room " + PhotonNetwork.room.name);
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
