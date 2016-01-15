// @author : Alex

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Extinction {
    namespace UI {
        /// <summary>
        /// Give feedback to player about level loading.
        /// Slider progress according the level loading.
        /// </summary>

        public class AsyncLoading : MonoBehaviour {
            public GameObject loadingImage;
            public Slider loadingBar;
            private AsyncOperation _loadingProgression;

            public void StartLoading(string levelName) {
                loadingImage.SetActive(true);
                StartCoroutine(UpdateLoadingBar(levelName));
            }

            IEnumerator UpdateLoadingBar(string levelName) {
                _loadingProgression = Application.LoadLevelAsync(levelName);

                while (! _loadingProgression.isDone ) {
                    loadingBar.value = _loadingProgression.progress;
                    yield return null;
                }
                loadingImage.SetActive(false);
            }
        }
    }
}
