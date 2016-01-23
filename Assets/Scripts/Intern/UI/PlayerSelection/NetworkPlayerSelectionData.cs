// @author : Alex

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Extinction {
    namespace UI{

        public class NetworkPlayerSelectionData : MonoBehaviour {
            public string namePrefab; // same name as survivor/herbie prefab
            public bool isSelected;
            public string activeSkill1;
            public string activeSkill2;
            public List<string> activeSkillChoices1;
            public List<string> activeSkillChoices2;
        }
    }
}