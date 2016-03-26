// @author : Alex

using UnityEngine;
using System.Collections.Generic;
using ExitGames.Client.Photon;

namespace Extinction {
    namespace Game {
        public class SpawnPositionSurvivor : MonoBehaviour {
            private Dictionary<int, int> _spawnTaken = new Dictionary<int, int>();
            private string _keyPositions = "spawnPos";
            private PhotonView _view;

            void Start() {
                // Use rpc to stack (as a lock system)
                _view = GetComponent<PhotonView>();
                _view.RPC("updatePosition", PhotonTargets.All);
            }

            [PunRPC]
            void updatePosition() {
                if (PhotonNetwork.player.name == "herbie")
                    return;

                if (!_view.isMine)
                    return;

                if (PhotonNetwork.room.customProperties[_keyPositions] == null) {
                    Hashtable spawnTaken = new Hashtable() { };
                    PhotonNetwork.room.customProperties[_keyPositions] = spawnTaken;
                }

                Hashtable photonSpawnTaken = (Hashtable)PhotonNetwork.room.customProperties[_keyPositions];
                int nbSpawns = transform.childCount;
                int index;

                while(true) {
                    index = Random.Range(0, nbSpawns - 1);
                    if (photonSpawnTaken[index] == null)
                        break;
                }
                GameObject goSurvivor = (GameObject) PhotonNetwork.player.TagObject;
                goSurvivor.transform.position = transform.GetChild(index).transform.position;

                photonSpawnTaken.Add(index, index);
                PhotonNetwork.room.customProperties[_keyPositions] = photonSpawnTaken;
                Debug.Log(photonSpawnTaken.ToString());
            }
        }
    }
}