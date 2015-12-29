// @author: Alex
using UnityEngine;
using System.Collections;

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

            private static string roomName_startMenu = "StartMenuRoom";
            private static string roomName_mapAPO = "MapAPO";

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

                Debug.Log("Connection to Photon was initialized");
            }

            public void JoinRoom(string roomName) {
                PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions() { maxPlayers = 5 }, TypedLobby.Default);
            }

            public override void OnPhotonJoinRoomFailed(object[] codeAndMsg) {
                base.OnPhotonJoinRoomFailed(codeAndMsg);
                Debug.Log("The room cannot be joined: " + codeAndMsg.ToString());
            }

            public override void OnJoinedRoom() {
                base.OnJoinedRoom();
                Debug.Log(PhotonNetwork.player.name + " rejoined the room " + PhotonNetwork.room.name);
            }

            public void CreateCharacter(string prefabName, Vector3 pos, Quaternion rot) {
                GameObject go = PhotonNetwork.Instantiate(prefabName, pos, rot, 0);
                //((INetworkInitializerPrefab)(go.GetComponent<Character>())).Activate();
            }
        }
    }
}
