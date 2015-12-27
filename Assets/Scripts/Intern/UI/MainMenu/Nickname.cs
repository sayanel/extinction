// @author : Alex

using UnityEngine;
using System.Collections;

namespace Extinction {
    namespace UI {
        /// <summary>
        /// Nickname util: save in a persistant way into Windows registry by using PlayerPrefs
        /// </summary>

        public class NicknameUtil : MonoBehaviour {
            private static string NICKNAME_KEY_PREFERENCE = "ExtinctionNickname";

            public static string GetNamePersistant() {
                return PlayerPrefs.GetString(NICKNAME_KEY_PREFERENCE, "Extinction Guest " + (int)(Random.value * 100));
            }

            public static void SetNamePersistant(string nickname) {
                PlayerPrefs.SetString(NICKNAME_KEY_PREFERENCE, nickname);
            }

            /// <summary>
            /// From unity the method must be attached to an instance
            /// </summary>
            public void SetNamePersistantScript(string nickname) {
                SetNamePersistant(nickname);
            }
        }
    }
}
