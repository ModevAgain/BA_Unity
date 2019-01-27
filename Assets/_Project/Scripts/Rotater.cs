using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour {

    public int RotSpeed;
    private int rotCount;


    //// Use this for initialization
    //void Start () {

    //}

    // Update is called once per frame
    void FixedUpdate()
    {

        //transform.localRotation = Quaternion.Euler(0, rotCount, 0);

        transform.RotateAround(transform.parent.position, Vector3.up, RotSpeed);
        //transform.LookAt(transform.parent.position, Vector3.up);

        //rotCount += RotSpeed;

    }
}
