using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : MonoBehaviour {

    public GameObject Resource1Object;

    private bool _shouldSpawn;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator SpawnResource()
    {

        

        while (_shouldSpawn)
        {
            GameObject temp = Instantiate(Resource1Object);
            //temp.transform.position = 
            yield return null;
        }


    }
}
