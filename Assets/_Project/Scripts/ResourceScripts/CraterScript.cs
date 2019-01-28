using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CraterScript : MonoBehaviour {

    private MeshRenderer _ren;
    public bool _active;
    private Tween _tweener;
    private ResourceObject _resObj;

	// Use this for initialization
	void Start () {
        _ren = GetComponent<MeshRenderer>();
        _resObj = GetComponentInParent<ResourceObject>();

    }

    private void OnTriggerEnter(Collider other)
    {        
        if (other.gameObject.layer ==  11 && !_active)
        {

            _active = true;
            _ren.enabled = true;
            _tweener = _ren.material.DOFloat(0,"_Cutout", 0.7f).SetDelay(0.5f).OnComplete(() => _active = false);
        }
        else if(other.gameObject.layer == 2)
        {
            _active = false;
            StartCoroutine(_resObj.HitLava());
        }

    }

    public void StopCrater()
    {
        _tweener.Kill();
        _ren.enabled = false;
    }
}
