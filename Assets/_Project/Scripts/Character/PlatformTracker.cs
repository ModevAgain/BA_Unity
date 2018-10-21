using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTracker : MonoBehaviour {

    public Platform CurrentPlatform;

    private void OnTriggerEnter(Collider other)
    {
        CurrentPlatform = other.gameObject.GetComponent<Platform>();
    }
}
