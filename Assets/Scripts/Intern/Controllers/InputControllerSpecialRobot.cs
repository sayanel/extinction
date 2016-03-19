using UnityEngine;
using System.Collections;

using Extinction.Characters;

public class InputControllerSpecialRobot : MonoBehaviour {

    protected SpecialRobot _specialRobot;

	void Awake ()
    {
        _specialRobot = GetComponent<SpecialRobot>();
    }
	
	
	void Update ()
    {
        _specialRobot.updateLocal();
    }
}
