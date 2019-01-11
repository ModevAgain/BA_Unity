using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlatformData", menuName = "Data/PlatformData", order = 2)]
public class PlatformData : ScriptableObject {

    public Material HighlightMat;    
    public Material NormalMat;

    public PlatformCost Platform1_Cost;
    public PlatformCost Platform2_Cost;


    [System.Serializable]
    public struct PlatformCost
    {
        public int ResourceType;
        public int ResourceCount;
    }
}
