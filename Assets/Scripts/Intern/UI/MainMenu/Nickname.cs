// @author : Alex

using UnityEngine;
using System.Collections;

namespace Extinction {
    namespace UI {
        public class NicknameUtil : MonoBehaviour {
            private static string NICKNAME_KEY_PREFERENCE = "ExtinctionNickname";

            public static string GetNamePersistant() {
                Debug.Log(PlayerPrefs.GetString(NICKNAME_KEY_PREFERENCE).ToString());
                return PlayerPrefs.GetString(NICKNAME_KEY_PREFERENCE, "Extinction Guest " + (int)(Random.value * 100));
            }

            public static void SetNamePersistant(string nickname) {
                PlayerPrefs.SetString(NICKNAME_KEY_PREFERENCE, nickname);
            }

            public void SetNamePersistantScript(string nickname) {
                Debug.Log(nickname);
                SetNamePersistant(nickname);
            }
        }
    }
}
