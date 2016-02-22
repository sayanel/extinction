// @author : Alex

using UnityEngine;
using System.Collections.Generic;

namespace Extinction {
    namespace FX{
        public class FXManager: Utils.SingletonMonoBehavior<FXManager> {

            /// <summary>
            /// Represents which prefab is associated with FXType
            /// Simply fill it in unity inspector
            /// Usefull to fill the FXPools afterall 
            /// </summary>
            [System.Serializable]
            public class FXModelsEntry {
                public Enums.FXType type;
                public GameObject prefab;
            }

            [SerializeField]
            private int _poolsSize;

            [SerializeField]
            private List<FXModelsEntry> _FXModels;
            private Dictionary<Enums.FXType, FXPool> _FXPools;

            /// <summary>
            /// A PhotonView must be attached to the GameObject for RPC trigger.
            /// </summary>
            private PhotonView _photonView;

            public void Awake() {
                DontDestroyOnLoad(this);
            }
            public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            }

            public void Start() {
                _FXPools = new Dictionary<Enums.FXType, FXPool>();
                foreach (FXModelsEntry model in _FXModels) {
                    _FXPools[model.type] = ScriptableObject.CreateInstance<FXPool>();
                    _FXPools[model.type].Init(model.prefab, _poolsSize);
                }
            }

            /// <summary>
            /// Activate for each Computers over the network the FX of type FXType
            /// </summary>
            /// <param name="FXType"></param>
            /// <param name="pos"></param>
            /// <param name="rot"></param>
            public void Activate(int FXType, Vector3 pos, Quaternion rot) {
                FXManager.Instance.GetComponent<PhotonView>().RPC("ActivateRPC", PhotonTargets.All, FXType, pos, rot);
            }

            [PunRPC]
            public void ActivateRPC(int FXType, Vector3 pos, Quaternion rot) {
                GameObject go = _FXPools[(Enums.FXType)FXType].Get();
                go.transform.position = pos;
                go.transform.rotation = rot;
                go.SetActive(true);
                go.GetComponent<FXEvent>().Activate();
            }

        }
    }
}
