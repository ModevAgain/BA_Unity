﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;


    void Update()
    {
        Vector3 goalPos = target.position - (new Vector3(0,15,13f));
        goalPos.y = transform.position.y;
        transform.position = goalPos;
        
    }
}
