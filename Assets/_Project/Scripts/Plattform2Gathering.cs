using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plattform2Gathering : MonoBehaviour {

    public float gatherTime = 6f;
    float activeGatherDuration = 0f;
    Image progressionImage;
    bool platformFinished = false;

    private void Awake()
    {
        progressionImage = GetComponentInChildren<Image>();
    }
	
	// Update is called once per frame
	void Update () {
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
