// @author : Alex
using UnityEngine;
using Extinction.Utils;


namespace Extinction {
    namespace UI {
        /// <summary>
        /// MultiLanguage Singleton.
        /// Default Language: English.
        /// Json files must be made like this: code.json (en.json, fr.json, ...)
        /// Check ELTK Extinction Language Tool Kit for easier insertion
        /// </summary>

        public class MultiLanguageManager : SingletonMonoBehavior<MultiLanguageManager> {
            [SerializeField]
            public string jsonDirPath = "./Assets/Scripts/Intern/UI/MultiLanguage";

            public string _code = "en";
            private JSONObject _elements;
            private static string KEY_ELEMENTS = "elements";

            new public void Awake() {
                base.Awake();
                ChangeLanguage(_code);
            }

            public void ChangeLanguage(string code) {
                _code = code;
                string path = System.IO.Path.Combine(jsonDirPath, code + ".json");
                JSONObject obj = new JSONObject(System.IO.File.ReadAllText(path));
                _elements = obj[MultiLanguageManager.KEY_ELEMENTS];
            }


            /// <summary>
            /// </summary>
            /// <param name="key"></param>
            /// <returns>The key associated value</returns>
            public string Get(string key){
                return _elements[key].str;
            }
        }
    }
}
