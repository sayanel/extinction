// @author : Florian

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Extinction
{
    namespace Weapons
    {

        /// <summary>
        /// Pool of Projectiles, usefull for frequent GameObject usage. 
        /// Avoid Instancition and Destroy frequently, only once instanciation
        /// </summary>
        public class ProjectilePool : ScriptableObject{

            private List<GameObject> _projectileObjects;
            private int _poolSize;
            private int _poolCurrentIndex;

            private GameObject _projectileObjectType;

            /// <summary>
            /// Constructs the Pool object
            /// </summary>
            public void Init( GameObject projectileObjectType, int poolSize )
            {
                if( poolSize < 0 )
                    throw new System.ArgumentOutOfRangeException();

                _poolSize = poolSize;
                _projectileObjectType = projectileObjectType;
                _poolCurrentIndex = -1;
                _projectileObjects = new List<GameObject>( _poolSize );


                // fill the GameObject pool list according the projectileObjectType 
                for( int i = 0; i < _poolSize; ++i )
                {
                    GameObject go = (GameObject)Instantiate( _projectileObjectType );
                    go.SetActive( false );
                    _projectileObjects.Add( go );
                }
            }

            /// <summary>
            /// Return an object from the pool. Does its own process pooling 
            /// TODO: See if other implementations are better such as:
            /// 1: foreach while GO is !activeInHierarchy
            /// 2: List growable with network context ?
            /// </summary>
            /// <returns> An Projectile object from the pool </returns>
            public GameObject Get()
            {
                _poolCurrentIndex = ( _poolCurrentIndex + 1 ) % _poolSize;
                return _projectileObjects[_poolCurrentIndex];
            }
        }
    }
}