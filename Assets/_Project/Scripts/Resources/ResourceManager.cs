using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ResourceManager : MonoBehaviour {

    public TextMeshPro ResourceText_0;
    public TextMeshPro ResourceText_1;
    public int ResourceCount_0;
    public int ResourceCount_1;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void IncrementResource(int resType, int count)
    {
        if(resType == 0)
        {
            ResourceCount_0 += count;
            ResourceText_0.text = "" + ResourceCount_0;
        }
        else if(resType == 1)
        {
            ResourceCount_1 += count;
            ResourceText_1.text = "" + ResourceCount_1;
        }

    }
}
