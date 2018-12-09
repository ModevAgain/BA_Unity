using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPipe : MonoBehaviour {

    public PlatformData PlatformData;
    public GridManager GridManager;
    public GameObject PlatformHolder;


    public static DataPipe instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        GridManager.Setup();
    }
}
