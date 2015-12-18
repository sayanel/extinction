// @author : Alex

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Extinction {
    namespace FX {

        /// <summary>
        /// Pool of FX, usefull for frequent GameObject usage. 
        /// Avoid Instancition and Destroy frequently, only once instanciation
        /// </summary>
        public class FXPool: ScriptableObject {
            private List<GameObject> _FXObjects;
            private int _poolSize;
            private int _poolCurrentIndex;

            private GameObject _fxObjectType;

            public void Init(GameObject fxObjectType, int poolSize) {
                if (poolSize < 0)
                    throw new System.ArgumentOutOfRangeException();

                _poolSize = poolSize;
                _fxObjectType = fxObjectType;
            }

            /// <summary>
            /// Constructs the Pool object
            /// </summary>
            public void Start() {
                _poolCurrentIndex = -1;
                _FXObjects = new List<GameObject>(_poolSize);
                for(int i = 0; i< _poolSize; ++i) {
                    GameObject go = (GameObject)Instantiate(_fxObjectType);
                    go.SetActive(false);
                    _FXObjects.Add(go);
                }
            }

            /// <summary>
            /// Return an object from the pool. Does its own process pooling 
            /// TODO: See if other implementations are better such as:
            /// 1: foreach while GO is !activeInHierarchy
            /// 2: List growable with network context ?
            /// </summary>
            /// <returns> An FX object from the pool </returns>
            public GameObject Get(){
                _poolCurrentIndex = (_poolCurrentIndex + 1) % _poolSize;
                return _FXObjects[_poolCurrentIndex];
            }
        }
    }
}
