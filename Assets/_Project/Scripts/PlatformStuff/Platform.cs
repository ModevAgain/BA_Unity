using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    [Header("Properties")]
    public bool Activated;


    private PlatformData _platformData;
    private Renderer _ren;
    private Collider _col;

    private void Awake()
    {
        _platformData = DataPipe.instance.PlatformData;
        _ren = GetComponent<MeshRenderer>();
        _col = GetComponent<Collider>();

        if(!Activated)
            _ren.material = _platformData.HighlightMat;

    }

    public void Highlight()
    {

        if (Activated)
        {
            _ren.material = _platformData.RedHighlightMat;
        }
        else
        {
            _col.enabled = false;   
            _ren.material = _platformData.HighlightMat;
        }
        _ren.enabled = true;
    }

    public void Activate()
    {
        _col.enabled = true;
        _ren.material = _platformData.NormalMat;
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

}
