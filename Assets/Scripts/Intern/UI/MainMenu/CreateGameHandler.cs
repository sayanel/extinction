// @author : Alex

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Extinction {
    namespace UI {
        public class CreateGameHandler : Photon.PunBehaviour{

            public Button buttonCreateGame;
            public Button buttonLoadRooms;

            public override void OnReceivedRoomListUpdate() {
                base.OnReceivedRoomListUpdate();
                buttonLoadRooms.onClick.Invoke();
            }

            public override void OnJoinedLobby() {
                base.OnJoinedLobby();
                buttonCreateGame.interactable = true;
                buttonLoadRooms.onClick.Invoke();
            }
        }
    }
}