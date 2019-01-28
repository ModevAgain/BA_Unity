using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Plattform2Gathering : MonoBehaviour {

    public float gatherTime = 6f;
    float activeGatherDuration = 0f;
    Image progressionImage;
    bool platformFinished = false;
    private RessourceSpawner _resSpawner;
    private Platform _platform;

    private void Awake()
    {
        progressionImage = GetComponentInChildren<Image>();
        _resSpawner = FindObjectOfType<RessourceSpawner>();
        _platform = GetComponentInParent<Platform>();

        FindObjectOfType<NavMeshSurface>().BuildNavMesh();
    }
	
	// Update is called once per frame
	void Update () {

        if (platformFinished)
            return;

		if(gatherTime >= activeGatherDuration)
        {
            activeGatherDuration += Time.deltaTime;

            var currentFillAmount = activeGatherDuration / gatherTime;

            progressionImage.fillAmount = currentFillAmount;
        }
        else if(gatherTime < activeGatherDuration)
        {
            progressionImage.fillAmount = 1f;
            platformFinished = true;
            StartCoroutine(_resSpawner.Ressource2_SpawnOnPlatform(_platform));

        }
	}

    public void ResetPlatform()
    {
        activeGatherDuration = 0f;
        platformFinished = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.name == "PlatformTracker")
        {
            if (platformFinished)
            {
                ResetPlatform();                
            }
        }
    }
}
