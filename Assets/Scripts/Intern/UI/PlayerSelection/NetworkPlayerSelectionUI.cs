// @author : Alex

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Extinction {
    namespace UI {
        public class NetworkPlayerSelectionUI : MonoBehaviour {
            public NetworkPlayerSelectionData playerData;
            public Button toggleSelectPlayer;
            public PhotonView pView;

            void Awake() {
                pView = GetComponent<PhotonView>();
            }

            [PunRPC]
            void changeSelection(bool isSelected) {
                playerData.isSelected = isSelected;
                toggleSelectPlayer.enabled = !isSelected;
                PhotonNetwork.player.name = playerData.namePrefab;
            }

            [PunRPC]
            void changeActiveSkill1(string value) {
                playerData.activeSkill1 = value;
            }

            [PunRPC]
            void changeActiveSkill2(string value) {
                playerData.activeSkill2 = value;
            }

            public void changeSelectionLocal() {
                Debug.Log("lol");
                pView.RPC("changeSelection", PhotonTargets.All, !toggleSelectPlayer.enabled);
                // deactivate all others buttons
            }
        }
    }
}