using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    [Header("Properties")]
    public bool Acivated;


    private PlatformData _platformData;
    private Renderer _ren;
    private Collider _col;

    private void Awake()
    {
        _platformData = DataPipe.instance.PlatformData;
        _ren = GetComponent<MeshRenderer>();
        _col = GetComponent<Collider>();

        if(!Acivated)
            _ren.material = _platformData.HighlightMat;

    }

    public void Highlight()
    {
        _col.enabled = false;
        _ren.material = _platformData.HighlightMat;
        _ren.enabled = true;
    }

    public void Activate()
    {
        _col.enabled = true;
        _ren.material = _platformData.NormalMat;
    }

    public void Deactivate()
    {
        _col.enabled = false;
        _ren.enabled = false;
        _ren.material = _platformData.HighlightMat;
    }

}
