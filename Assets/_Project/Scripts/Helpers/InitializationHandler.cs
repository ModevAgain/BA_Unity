using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InitializationHandler : MonoBehaviour {


    private List<IInitializable> _initializables;


    void Awake () {

        List<ScriptableObject> objs = Resources.LoadAll<ScriptableObject>("").ToList();
        _initializables = new List<IInitializable>();
        foreach (var item in objs)
        {
            if ((item is IInitializable))
                _initializables.Add(item as IInitializable);
        }

        foreach (var item in _initializables)
        {
            item.Initialize();
        }

    }

    private void OnDestroy()
    {
        foreach (var item in _initializables)
        {
            item.Deinitialize();
        }
    }

    private void OnApplicationQuit()
    {
        foreach (var item in _initializables)
        {
            item.Deinitialize();
        }
    }
}
