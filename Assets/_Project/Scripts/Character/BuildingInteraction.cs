using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInteraction : MonoBehaviour {

    public GridManager _gridManager;
    private PlayerReferences _playerRefs;

    public Platform _highlightedPlatform;

    private bool _abortBuildAction;
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

    public void ReceiveBuildingOperation(bool abort)
    {
        _abortBuildAction = abort;
        waitingForAction = false;
    }

    public void StartBuildingProcess()
    {
        StartCoroutine(HighlightNewPlatform());
    }


    public IEnumerator HighlightNewPlatform()
    {

        waitingForAction = true;
        

        _highlightedPlatform = _gridManager.GetPlatformForHighlight(_playerRefs.CurrentPlatform.transform.localPosition, _playerRefs.LookDirection);
        _highlightedPlatform.Highlight();

        while (waitingForAction)
        {
            yield return null;
        }

        if (_abortBuildAction)
        {
            _highlightedPlatform.Deactivate();
        }
        else
        {
            _highlightedPlatform.Activate();
        }
    }
}
