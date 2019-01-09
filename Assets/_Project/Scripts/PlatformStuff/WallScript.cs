using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour {

    public bool ActiveFromStart;

    public bool CurrentlyActive;

    private Collider _col;
    private Rigidbody _rigid;
    private Platform _selfPlatform;

	
	void Awake () {
        _selfPlatform = GetComponentInParent<Platform>();
        _col = GetComponent<Collider>();
        _rigid = GetComponent<Rigidbody>();

        if (!ActiveFromStart)
        {
            _col.enabled = false;
            _col.isTrigger = true;
        }
        else
        {
            CurrentlyActive = true;
        }

	}
	

    private void OnTriggerEnter(Collider other)
    {
        Platform p = other.GetComponentInParent<Platform>();

        if (p != null && p != _selfPlatform)
        {
            if (other.gameObject.GetComponentInParent<Platform>().Activated)
            {
                other.GetComponent<WallScript>().DeactivateWall();            
            }
        }
    }

    public void ActivateWall()
    {
        _col.enabled = true;
        Invoke("ChangeToNormalWall", 0.1f);
        CurrentlyActive = true;
    }

    public void ChangeToNormalWall()
    {
        _col.isTrigger = false;
    }

    public void DeactivateWall()
    {
        CurrentlyActive = false;
        gameObject.SetActive(false);
    }
}
