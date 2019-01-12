using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Linq;

public class Platform : MonoBehaviour {

    [Header("Properties")]
    public bool Activated;
    public Mesh Mesh_0;
    public Mesh Mesh_1;
    public int GatherTime = 4;
    public bool ShouldGather;
    public bool IsHQ;
    public GameObject Platform2Object;

    private PlatformData _platformData;
    private Renderer _ren;
    private MeshFilter _meshFilter;
    private Collider _col;
    private List<ResourceObject> _resourcesInRange;
    private int _currentType;
    private WallScript[] _walls;
    private List<WallScript> _activeWalls; 
    private System.Random _rnd;

    private void Awake()
    {
        _platformData = DataPipe.instance.PlatformData;
        _ren = GetComponent<MeshRenderer>();
        _col = GetComponent<Collider>();
        _meshFilter = GetComponent<MeshFilter>();

        if(!Activated)
            _ren.material = _platformData.HighlightMat;

        _resourcesInRange = new List<ResourceObject>();

        _walls = GetComponentsInChildren<WallScript>();
        _activeWalls = new List<WallScript>();
         _rnd = new System.Random();
    }

    public void Highlight(int op)
    {
        _col.enabled = true;
        if (Activated)
        {
            //_ren.material = _platformData.RedHighlightMat;
        }
        else
        {
            _col.isTrigger = true;  
            //_col.enabled = false;   
            _meshFilter.sharedMesh = op == 0 ? Mesh_0 : Mesh_1;
            gameObject.name = gameObject.name.Substring(0, gameObject.name.Length - 1) + op;
            _ren.material = _platformData.HighlightMat;
        }
        _ren.enabled = true;
        _currentType = op;
    }

    public void Activate()
    {
        _col.enabled = true;
        _col.isTrigger = false;
        _ren.material = _platformData.NormalMat;
        Activated = true;
        
        if (_currentType == 1)
        {
            //ShouldGather = true;
            //StartCoroutine(GatherRoutine());

            Platform2Object.SetActive(true);
        }

        foreach (var wall in _walls)
        {
            wall.ActivateWall();
        }

    }

    public void Deactivate()
    {
        if (Activated)
        {
            _ren.material = _platformData.NormalMat;
        }
        else
        {
            _col.enabled = false;
            _ren.enabled = false;
            _ren.material = _platformData.HighlightMat;
        }
    }


    public IEnumerator GatherRoutine()
    {
        while (ShouldGather)
        {
            foreach (var item in _resourcesInRange)
            {
                item.GetPickedUp(this.transform);
            }

            _resourcesInRange.Clear();
            yield return new WaitForSeconds(GatherTime);
        }

    }

    public void OnTriggerEnter(Collider col)
    {
        if (!ShouldGather)
            return;

        ResourceObject tempResource = col.GetComponentInParent<ResourceObject>();
        if (tempResource != null )
            _resourcesInRange.Add(tempResource);
    }

    public void OnTriggerExit(Collider col)
    {
        if (!ShouldGather)
            return;

        ResourceObject tempResource = col.GetComponentInParent<ResourceObject>();
        if (tempResource != null)
        {
            if(_resourcesInRange.Contains(tempResource))
                _resourcesInRange.Remove(tempResource);
        }
    }

    public Vector3 GetRandomActiveWall()
    {

        _activeWalls =  _walls.Where((w) => w.CurrentlyActive).ToList();

        if (_activeWalls.Count != 0)
            return _activeWalls[_rnd.Next(_activeWalls.Count)].transform.position;
        else return Vector3.zero;
        
    }
}
