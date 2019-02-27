using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlatformData", menuName = "Data/PlatformData", order = 2)]
public class PlatformData : ScriptableObject {

    [Header("Materials")]
    public Material HighlightMat;    
    public Material NormalMat;

    [Header("Platform Costs")]
    public List<PlatformCost> Platform1_Cost;
    public List<PlatformCost> Platform2_Cost;


    [System.Serializable]
    public struct PlatformCost
    {
        public int ResourceType;
        public int ResourceCount;
    }
}
