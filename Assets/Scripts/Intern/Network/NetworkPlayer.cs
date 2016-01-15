// @author: Alex

using UnityEngine;
using System.Collections;

namespace Extinction {
    namespace Network {

        /// <summary>
        /// Used for infos to send and to receive to others players: PhotonView observes on this script
        /// Attached on a player(prefab)
        /// </summary>
        public class NetworkPlayer: NetworkComponent{

            Vector3 m_correctPlayerPos; //< smooth
            Quaternion m_correctPlayerRot; //< smooth
            static float speedSmooth = 5;

            void Start() {
            }

            /// <summary>
            /// Called each frame: do smooth stuff for network player
            /// !isMine: do only correction for remote players
            /// </summary>
            void Update() {
                if (photonView.isMine)
                    return;

                transform.position = Vector3.Lerp(transform.position, m_correctPlayerPos, Time.deltaTime * speedSmooth);
                transform.rotation = Quaternion.Lerp(transform.rotation, m_correctPlayerRot, Time.deltaTime * speedSmooth);
            }

            /// <summary>
            /// While a script is observed, PhotonView calls regularly this method:
            /// Aim: create the info we want to pass to others and to handle incoming info (depending on who created PhotonView)
            /// </summary>
            /// <param name="stream">
            /// stream.isWriting tells us if we need to write or read info.
            /// First let's send the rotation and position info
            /// </param>
            /// <param name="info"></param>
            /// 
            public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
                if (stream.isWriting) {
                    // we handle this character: send to others the transform data
                    stream.SendNext(transform.position);
                    stream.SendNext(transform.rotation);
                    //stream.SendNext(m_playerController.state);
                }
                else {
                    // Network player, receive data (this object viewed into other windows game over network)
                    m_correctPlayerPos = (Vector3)stream.ReceiveNext();
                    m_correctPlayerRot = (Quaternion)stream.ReceiveNext();
                    //m_playerController.state = (CharacterState)stream.ReceiveNext();
                }
            }
        }
    }
}