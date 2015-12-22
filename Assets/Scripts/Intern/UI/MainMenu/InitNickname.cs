// @author : Alex

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Extinction {
    namespace UI {
        public class InitNickname : MonoBehaviour {

            public void Start() {
                GetComponent<InputField>().text = NicknameUtil.GetNamePersistant();
            }
        }
    }
}
