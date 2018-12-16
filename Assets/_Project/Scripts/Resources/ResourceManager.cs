using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ResourceManager : MonoBehaviour {

    public TextMeshPro ResourceText;
    public int ResourceCount;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void IncrementResource(int count)
    {
        ResourceCount += count;
        ResourceText.text = "" + ResourceCount;

    }
}
