using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Platform : MonoBehaviour {

    [Header("Properties")]
    public bool Activated;
    public Mesh Mesh_0;
    public Mesh Mesh_1;
    public int GatherTime = 4;
    public bool ShouldGather;

    private PlatformData _platformData;
    private Renderer _ren;
    private MeshFilter _meshFilter;
    private Collider _col;
    private List<ResourceObject> _resourcesInRange;
    private int _currentType;
    private TextMeshPro _text;

    private void Awake()
    {
        _platformData = DataPipe.instance.PlatformData;
        _ren = GetComponent<MeshRenderer>();
        _col = GetComponent<Collider>();
        _meshFilter = GetComponent<MeshFilter>();

        if(!Activated)
            _ren.material = _platformData.HighlightMat;

        _resourcesInRange = new List<ResourceObject>();
        _text = GetComponentInChildren<TextMeshPro>();

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
            //_col.enabled = false;   
            //_meshFilter.sharedMesh = op == 0 ? Mesh_0 : Mesh_1;
            gameObject.name = gameObject.name.Substring(0, gameObject.name.Length - 1) + op;
            _text.text = op == 0 ? "I" : "II";  
            _ren.material = _platformData.HighlightMat;
        }
        _ren.enabled = true;
        _currentType = op;
        _text?.DOFade(1, 0);
    }

    public void Activate()
    {
        _col.enabled = true;
        _ren.material = _platformData.NormalMat;
        Activated = true;
        
        if (_currentType == 1)
        {
            ShouldGather = true;
            StartCoroutine(GatherRoutine());
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
            _text?.DOFade(0, 0);
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
}
