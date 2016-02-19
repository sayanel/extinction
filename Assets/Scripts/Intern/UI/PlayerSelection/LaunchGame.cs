using UnityEngine;
using System.Collections;
using Extinction.UI;
using Extinction.Network;

public class LaunchGame : MonoBehaviour {
    PhotonView pView;

    void Awake() {
        pView = GetComponent<PhotonView>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
    }

    [PunRPC]
    public void launchGame() {
        NetworkManager.Instance.LaunchGame(GetComponent<AsyncLoading>());
    }   

    public void click() {
        pView.RPC("launchGame", PhotonTargets.AllBuffered);
    }
}
