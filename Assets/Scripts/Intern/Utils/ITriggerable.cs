using UnityEngine;
using System.Collections;

/// <summary>
/// Symple interface to trigegr some functions. It is used by the Detector to call function when an object enter or leave one of its colliders.
/// </summary>
public interface ITriggerable {

    void triggerEnter(Collider other);
    void triggerExit(Collider other);
}
