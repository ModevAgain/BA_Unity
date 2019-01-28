using BA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DirectionalArrowHandler : MonoBehaviour {

    public BA_InputMapper Mapper;

    public Vector3 targetDirection;

    private MeshRenderer _ren;

    private RaycastHit hit;
    private Ray ray;

    [SerializeField]
    private float tAngle;

    private bool _gamepad;
    private bool _fadeOut;
    private int _frameCount;
    private bool _blocked;

    private void Awake()
    {
        _ren = GetComponentInChildren<MeshRenderer>();

        if (DataPipe.instance.GameData.Touch)
        {
            _ren.enabled = false;
            return;
        }


        if (DataPipe.instance.GameData.MK)
            Mapper.DirectionalInputMousePos += UpdateTargetPos;
        if (DataPipe.instance.GameData.Gamepad)
        {
            Cursor.visible = false;
            _ren.enabled = false;
            Mapper.DirectionalInputRightStick += (v) => { _gamepad = true;  UpdateTargetPos(v); } ;
        }

    }
	
	// Update is called once per frame
	void Update () {

        if(_fadeOut)
        {
            _fadeOut = false;
            _ren.enabled = false;
            _gamepad = false;
        }

        if (_gamepad)
        {
            _frameCount++;


            if(_frameCount > 1)
                _fadeOut = true;
        }

	}

    public void UpdateTargetPos(Vector2 direction)
    {
        if (_blocked)
            return;

        _ren.enabled = true;

        ray = Camera.main.ScreenPointToRay(direction);        

        if (Physics.Raycast(ray, out hit, 1000, 1 << 17))
        {
            targetDirection = hit.point - transform.position;
            tAngle = Vector3.SignedAngle(transform.forward, targetDirection, transform.up);
            transform.Rotate(Vector3.up, tAngle, Space.World);
        }

        _frameCount = 0;
    }

    public void FadeOutAndBlock()
    {
        _fadeOut = true;
        _blocked = true;

    }

    public void Unblock()
    {
        _blocked = false;
    }
}
