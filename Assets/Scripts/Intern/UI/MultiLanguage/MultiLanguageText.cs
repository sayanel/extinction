// @author : Alex

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Extinction {
    namespace UI {
        /// <summary>
        /// MultiLanguage util script: must be attached to a Text.
        /// Ensure the multi language process. The Text component text must provide the Key associated of json language file.
        /// </summary>
        public class MultiLanguageText : MonoBehaviour {

            /// <summary>
            /// Multilanguage key corresponding into json language file.
            /// </summary>
            private string _keyLanguage;
            private Text _textComponent;

            public void Awake() {
                _textComponent = GetComponent<Text>();
                _keyLanguage = _textComponent.text;
            }

            public void Start() {
                Refresh();
            }

            public void Refresh() {
                _textComponent.text = MultiLanguageManager.Instance.Get(_keyLanguage);
            }
        }
    }
}

