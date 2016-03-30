//@author : florian.
using UnityEngine;
using System.Collections;

public class PlayerSelectionCamera : MonoBehaviour {

    [SerializeField]
    float _startZOffset;
    [SerializeField]
    float _startScreenWidth = 1;

    [SerializeField]
    float _amplitude = 10; 

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        int screenWidth = Screen.width;
        Debug.Log("screen width : " + screenWidth);
        float currentZOffset = Mathf.Lerp(_startZOffset, _startZOffset * _amplitude, ((_startScreenWidth - screenWidth) / (float)_startScreenWidth));// - (screenWidth * _startZOffset) / _startScreenWidth;
        transform.localPosition = new Vector3(7.0f, 1.0f, currentZOffset);
    }
}
