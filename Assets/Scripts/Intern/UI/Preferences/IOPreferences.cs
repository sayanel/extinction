// @author : Alex

using UnityEngine;
using System.Collections;

namespace Extinction {
    namespace UI {
        /// <summary>
        /// In Out game players preferences: persistant way using Unity PlayerPrefs - see PlayerPrefs doc for more informations.
        /// </summary>

        public class IOPreferences : MonoBehaviour {
            readonly public static string KEY_NICKNAME = "extinction_nickname";
            readonly public static string KEY_GAME_SOUND_LEVEL = "extinction_sound_level";
            readonly public static string KEY_MICROPHONE_SOUND_LEVEL = "extinction_microphone_sound_level";

            // resolution already set when launching unity app: might be useless
            readonly public static string KEY_RESOLUTION_WIDTH = "extinction_resolution_width";
            readonly public static string KEY_RESOLUTION_HEIGHT = "extinction_resolution_height";


            public static void SavePreferences() {
                PlayerPrefs.Save();
            }

            public static string GetValue(string prefKey, string defaultValue) {
                return PlayerPrefs.GetString(prefKey, defaultValue);
            }

            public static void SetValue(string prefKey, string value) {
                PlayerPrefs.SetString(prefKey, value);
            }

            /// <summary>
            /// From unity the method must be attached to an instance
            /// </summary>
            public static string GetValueScript(string prefKey, string defaultValue) {
                return IOPreferences.GetValue(prefKey, defaultValue);
            }

            public static void SetValueScript(string prefKey, string value) {
                SetValue(prefKey, value);
            }
        }
    }
}
