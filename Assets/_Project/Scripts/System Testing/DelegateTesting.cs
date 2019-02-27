using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateTesting : MonoBehaviour {

    public delegate void TestDelegate();

    TestDelegate Test;

	// Use this for initialization
	void Start () {


        Test += () => logN("1st");
        Test += () => logN("2nd");



        Test.Invoke();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void logN(string n)
    {
        Debug.Log(n);
    }
}
