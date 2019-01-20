using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class ResourceManager : MonoBehaviour {

    public TextMeshPro ResourceText_0;
    public TextMeshPro ResourceText_1;
    public int ResourceCount_0;
    public int ResourceCount_1;

    public int Platform1Count;
    public int Platform2Count;

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

    public void DecrementResource(List<PlatformData.PlatformCost> cost)
    {
        if (cost.Where((c) => c.ResourceType == 0).Count() != 0)
        {
            ResourceCount_0 -= cost.Where((c) => c.ResourceType == 0).FirstOrDefault().ResourceCount;
            ResourceText_0.text = "" + ResourceCount_0;
            Platform1Count++;
        }
        if (cost.Where((c) => c.ResourceType == 1).Count() != 0)
        {
            ResourceCount_1 -= cost.Where((c) => c.ResourceType == 1).FirstOrDefault().ResourceCount;
            ResourceText_1.text = "" + ResourceCount_1;
            Platform2Count++;
        }
    }

    public bool HasEnoughResource(List<PlatformData.PlatformCost> cost)
    {
        bool enoughResources = true;

        if (cost.Where((c) => c.ResourceType == 0).Count() != 0)
        {
            if(! (ResourceCount_0 - cost.Where((c) => c.ResourceType == 0).FirstOrDefault().ResourceCount >= 0))
            {
                enoughResources = false;
            }
;
        }
        if (cost.Where((c) => c.ResourceType == 1).Count() != 0)
        {
            if (!(ResourceCount_1 - cost.Where((c) => c.ResourceType == 1).FirstOrDefault().ResourceCount >= 0))
                enoughResources = false;
        }

        return enoughResources;
    }
}
