using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReferences : MonoBehaviour {

    public Collider MainCollider;
    public Renderer MainRenderer;
    public Transform MainTransform;
    public Platform CurrentPlatform
    {
        get
        {
            return _platformTracker.CurrentPlatform;
        }
    }
    public Vector3 LookDirection
    {
        get
        {                        
            return MainTransform.forward;
        }
    }


    private PlatformTracker _platformTracker;

    private void Awake()
    {
        _platformTracker = GetComponentInChildren<PlatformTracker>();

        
    }
}
