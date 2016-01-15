// @author : Alex

using UnityEngine;
using System.Collections;
using Extinction.Network;

namespace Extinction {
    namespace UI {
        /// <summary>
        /// Basic click handler for main menu
        /// </summary>
        public class ClickHandler : MonoBehaviour {

            public LoadRoomList loadRoomScript;

            public void ExitGame() {
                Application.Quit();
            }

            public void CreateGame() {
                if (PhotonNetwork.inRoom)
                    throw new System.Exception("Player already in room: cannot create a new Room!");
                NetworkManager.Instance.CreateRoom();
                loadRoomScript.Load();
            }

            public void JoinGame(string roomName) {
                PhotonNetwork.LeaveRoom();
                NetworkManager.Instance.JoinRoom(roomName);
                loadRoomScript.Load();
            }
        }
    }
}
