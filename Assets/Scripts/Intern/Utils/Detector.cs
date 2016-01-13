//Author : Florian

using UnityEngine;
using System.Collections;

/// <summary>
/// This detector will simply warn its _target when something enter or leave one of its collider.
/// Its _target is a ITriggerable, a simple interface to handler these kind of events.
/// </summary>
public class Detector : MonoBehaviour {

    [SerializeField]
    private string _tag;

    [SerializeField]
    private ITriggerable _target;

    private Collider _collider;

    // Use this for initialization
    void Start ()
    {
        if (_collider == null)
            Debug.LogWarning("You have to place a collider on a GameObject which have a Detector Component ! ");
	}

    void OnTriggerEnter(Collider other)
    {
        if(_target != null)
            _target.triggerEnter(other);
    }

    void OnTriggerExit(Collider other)
    {
        if (_target != null)
            _target.triggerExit(other);
    }
	
}
