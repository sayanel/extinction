// @author : Alex
using UnityEngine;
using Extinction.Utils;


namespace Extinction {
    namespace MyNamespace {
        /// <summary>
        /// MultiLanguage Singleton.
        /// Default Language: English.
        /// Json files must be made like this: code.json (en.json, fr.json, ...)
        /// Check ELTK Extinction Language Tool Kit for easier insertion
        /// </summary>

        public class MultiLanguage : SingletonMonoBehavior<MultiLanguage> {
            [SerializeField]
            public string jsonDirPath = "./Assets/Scripts/Intern/UI/MultiLanguage";

            private string _code = "en";
            private JSONObject _elements;
            private static string KEY_ELEMENTS = "elements";

            public void Start() {
                ChangeLanguage(_code);
            }

            public void ChangeLanguage(string code) {
                _code = code;
                string path = System.IO.Path.Combine(jsonDirPath, code + ".json");
                JSONObject obj = new JSONObject(System.IO.File.ReadAllText(path));
                _elements = obj[MultiLanguage.KEY_ELEMENTS];
            }


            /// <summary>
            /// </summary>
            /// <param name="key"></param>
            /// <returns>The key associated value</returns>
            public string Get(string key){
                return _elements[key].ToString();
            }
        }
    }
}
