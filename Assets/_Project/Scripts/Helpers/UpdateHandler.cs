using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpdateHandler : MonoBehaviour {

    private IUpdatable[] _updatables;
    public bool Data;
    
    void Awake ()
    {

        List<ScriptableObject> objs = Resources.LoadAll<ScriptableObject>("").ToList();
        List<IUpdatable> tempUpdatables = new List<IUpdatable>();
        foreach (var item in objs)
        {
            if ((item is IUpdatable))
                tempUpdatables.Add(item as IUpdatable);
        }

        _updatables = tempUpdatables.ToArray();
    }
	
	
	void Update () {

        for (int i = 0; i < _updatables.Length; i++)
        {
            _updatables[i].Update();

        }

        //Data = (EventSystem.current.currentInputModule as BA.BA_InputModule).GetPointerData();
    }
}
