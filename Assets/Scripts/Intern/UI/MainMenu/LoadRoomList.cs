// @author : Alex
using UnityEngine;
using System.Collections;
using Extinction.Network;


namespace Extinction {
    namespace UI {
        /// <summary>
        /// Util script for updating games list
        /// </summary>
        public class LoadRoomList : MonoBehaviour {
            public Transform roomListContentPanel;
            public GameObject roomUIModel;

            public void Load() {
                RoomInfo[] roomList = PhotonNetwork.GetRoomList();

                foreach (Transform child in roomListContentPanel.transform) {
                    GameObject.Destroy(child.gameObject);
                }

                foreach (RoomInfo room in roomList) {
                    GameObject roomUI = Instantiate(roomUIModel);
                    RoomInfoUI infosUI = roomUI.GetComponent<RoomInfoUI>();
                    infosUI.gameName.text = room.name;
                    infosUI.playerNumbers.text = room.playerCount.ToString() + " / " + room.maxPlayers.ToString();
                    infosUI.startTime.text = ""; // TODO
                    infosUI.transform.SetParent(roomListContentPanel);
                }
            }

        }   
    }
}
