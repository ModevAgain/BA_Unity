using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class DataPipe : MonoBehaviour {

    public GameData GameData;
    public PlatformData PlatformData;
    public GridManager GridManager;
    public ResourceManager ResourceManager;
    public RessourceSpawner RessourceSpawner;
    public BuildingInteraction BuildingInteraction;
    public RadialBuildingUI BuildingUI;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            DOTween.KillAll();
            SceneManager.LoadScene("Start");
        }
    }
}
