using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleWolf : MonoBehaviour {

    public GameObject Wolf;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void Toggle()
    {
        if (Wolf.activeSelf)
            Wolf.SetActive(false);
        else Wolf.SetActive(true);
    }

}
