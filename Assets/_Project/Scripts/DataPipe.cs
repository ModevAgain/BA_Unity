using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class DataPipe : MonoBehaviour {

    public PlatformData PlatformData;
    public GridManager GridManager;
    public ResourceManager ResourceManager;
    public BuildingInteraction BuildingInteraction;

    public NavMeshSurface NavMeshSurface;
    public GameObject PlatformHolder;
    public PlayerReferences PlayerReferences;


    public static DataPipe instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        GridManager.Setup();
    }
}
