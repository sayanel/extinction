// @author : Alex

using UnityEngine;
using System.Collections.Generic;

namespace Extinction
{
    namespace Weapons
    {
        public class ProjectileManager : Utils.SingletonMonoBehavior<ProjectileManager>
        {

            /// <summary>
            /// Represents which prefab is associated with ProjectileType
            /// Simply fill it in unity inspector
            /// Usefull to fill the ProjectilePools afterall 
            /// </summary>
            [System.Serializable]
            public class ProjectileModelsEntry
            {
                public Enums.ProjectileType type;
                public GameObject prefab;
            }

            [SerializeField]
            private int _poolsSize;

            [SerializeField]
            private List<ProjectileModelsEntry> _projectileModels;
            private Dictionary<Enums.ProjectileType, ProjectilePool> _ProjectilePools;

            /// <summary>
            /// A PhotonView must be attached to the GameObject for RPC trigger.
            /// </summary>
            private PhotonView _photonView;

            public void Awake()
            {
                DontDestroyOnLoad( this );
            }

            public void Start()
            {
                _ProjectilePools = new Dictionary<Enums.ProjectileType, ProjectilePool>();
                foreach( ProjectileModelsEntry model in _projectileModels )
                {
                    _ProjectilePools[model.type] = ScriptableObject.CreateInstance<ProjectilePool>();
                    _ProjectilePools[model.type].Init( model.prefab, _poolsSize );
                }
            }

            /// <summary>
            /// Activate for each Computers over the network the projectile of type projectileType
            /// </summary>
            /// <param name="ProjectileType"></param>
            /// <param name="pos"></param>
            /// <param name="rot"></param>
            [PunRPC]
            public void Activate( int projectileType, Vector3 pos, Quaternion rot )
            {
                ProjectileManager.Instance.GetComponent<PhotonView>().RPC( "ActivateRPC", PhotonTargets.All, projectileType, pos, rot );
                ActivateLocal( projectileType, pos, rot );
            }

            // the idea is to have a local activation which activates damages on projectile, and a network activation which activates the gameObject.
            //there are multiple problèms with this approach : 
            //rpc should be called first to activate the gameObject before we can modify it.
            //ProjectileManager.get() get each time a different projectile...
            public void ActivateLocal( int projectileType, Vector3 pos, Quaternion rot )
            {
                GameObject go = _ProjectilePools[(Enums.ProjectileType)projectileType].Get(); //problem here
                go.GetComponent<Projectile>().ActivateLocal();
            }

            public void ActivateRPC( int projectileType, Vector3 pos, Quaternion rot )
            {
                GameObject go = _ProjectilePools[(Enums.ProjectileType)projectileType].Get();
                go.transform.position = pos;
                go.transform.rotation = rot;
                go.SetActive( true );
            }

        }
    }
}
