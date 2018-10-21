using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInteraction : MonoBehaviour {

    public GridManager _gridManager;
    private PlayerReferences _playerRefs;

    public Platform _highlightedPlatform;

    private bool _buildAction;
    private bool waitingForAction;

    private void Start()
    {
        _playerRefs = GetComponent<PlayerReferences>();
        _gridManager = DataPipe.instance.GridManager;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(HighlightNewPlatform());
        }

        //else if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    _abortBuildAction = false;
        //    waitingForAction = false;
        //}
    }

    public void ReceiveBuildingOperation(bool build)
    {
        _buildAction = build;
        waitingForAction = false;
    }

    public void StartBuildingProcess()
    {
        StartCoroutine(HighlightNewPlatform());
    }


    public IEnumerator HighlightNewPlatform()
    {

        waitingForAction = true;


        Vector3 lastPos = Vector3.zero;
        Vector3 lastDir = Vector3.zero;

        while (waitingForAction)
        {
            

            if(lastPos != _playerRefs.CurrentPlatform.transform.localPosition ||lastDir != _playerRefs.LookDirection)
            {

                _highlightedPlatform?.Deactivate();            
                _highlightedPlatform = _gridManager.GetPlatformForHighlight(_playerRefs.CurrentPlatform.transform.localPosition, _playerRefs.LookDirection);
                _highlightedPlatform.Highlight();
            }

            lastPos = _playerRefs.CurrentPlatform.transform.localPosition;
            lastDir = _playerRefs.LookDirection;

            yield return new WaitForSeconds(0.2f);


        }

        if (!_buildAction)
        {
            _highlightedPlatform.Deactivate();
        }
        else
        {
            _highlightedPlatform.Activate();
        }

        _highlightedPlatform = null;
    }
}
