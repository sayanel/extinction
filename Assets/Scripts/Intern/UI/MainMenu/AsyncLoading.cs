// @author : Alex

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Extinction.Network;

namespace Extinction {
    namespace UI {
        /// <summary>
        /// Give feedback to player about level loading.
        /// Slider progress according the level loading.
        /// </summary>

        public class AsyncLoading : MonoBehaviour {
            public GameObject wrapperBackground;
            public Slider loadingBar;
            private AsyncOperation _loadingProgression;

            public void StartLoading(int levelIndex) {
                wrapperBackground.SetActive(true);
                StartCoroutine(UpdateLoadingBar(levelIndex));
            }

            IEnumerator UpdateLoadingBar(int levelIndex) {
                // TODO: see with photon equivalent Application.LoadLevelAsync
                PhotonNetwork.LoadLevel(levelIndex);
                yield return true;
                while (! _loadingProgression.isDone ) {
                    loadingBar.value = _loadingProgression.progress;
                    yield return null;
                }
                wrapperBackground.SetActive(false);
            }
        }
    }
}
