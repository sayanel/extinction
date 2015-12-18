// @author : Alex

using UnityEngine;
using System.Collections;

namespace Extinction {
    namespace Utils {
        public class SingletonMonoBehavior<T> where T : MonoBehaviour {
            protected T _instance = null;
            private object _lock = new object();
            private static bool applicationIsQuitting = false;

            public T Instance {
                get {
                    if (applicationIsQuitting) {
                        Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed on application quit. Won't create again - returning null.");
                        return null;
                    }

                    // lock if multithread context
                    lock (_lock) {
                        if (!_instance) {
                            _instance = (T)GameObject.FindObjectOfType(typeof(T));

                            if (!_instance) {
                                Debug.LogError("The script SingletonMonoBehavior<" + typeof(T) + "> must be attached to one GameObject");
                            }
                        }
                        return _instance;
                    }
                }
            }

            /// <summary>
            /// When Unity quits, it destroys objects in a random order.
            /// In principle, a Singleton is only destroyed when application quits.
            /// 
            /// If any script calls Instance after it have been destroyed, 
            /// it will create a buggy ghost object that will stay on the Editor scene
            /// even after stopping playing the Application. Really bad!
            /// 
            /// So, this was made to be sure we're not creating that buggy ghost object.
            /// </summary>
            public void OnDestroy() {
                applicationIsQuitting = true;
            }
        }
    }
}