// @author : Alex

using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;

namespace Extinction {
    namespace UI {
        public class NetworkPlayerSelectionUI : Photon.PunBehaviour {
            public NetworkPlayerSelectionData playerData;
            public Button toggleSelectPlayer;

            void Awake() {
                // Check if already selected by other player
                toggleSelectPlayer.interactable = (int) PhotonNetwork.room.customProperties[playerData.namePrefab] == 0;
            }

            public void changeSelection(bool isSelected) {
                string oldSelectedPrefab = PhotonNetwork.player.name;
                playerData.isSelected = isSelected;
                toggleSelectPlayer.interactable = !isSelected;
                Hashtable changedProperties = new Hashtable() { { playerData.namePrefab, isSelected ? 1 : 0 } };

                // Deselect previous prefab
                if (oldSelectedPrefab != null && oldSelectedPrefab != playerData.namePrefab)
                    changedProperties.Add(oldSelectedPrefab, 0);

                // Update over network
                PhotonNetwork.room.SetCustomProperties(changedProperties);

                // Store the prefab name as player name: used when PhotonNetwork.Instantiate
                PhotonNetwork.player.name = playerData.namePrefab;
            }

            public void clickButton() {
                changeSelection(true);
            }

            void changeActiveSkill1(string value) {
                playerData.activeSkill1 = value;
            }

            void changeActiveSkill2(string value) {
                playerData.activeSkill2 = value;
            }

            public override void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged) {
                base.OnPhotonCustomRoomPropertiesChanged(propertiesThatChanged);
                if(propertiesThatChanged.ContainsKey(playerData.namePrefab)) {
                    toggleSelectPlayer.interactable = (int)propertiesThatChanged[playerData.namePrefab] == 0;
                }
            }

            public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
                // need to be declared
            }
        }
    }
}