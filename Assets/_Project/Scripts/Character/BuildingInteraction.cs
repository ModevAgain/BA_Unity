using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildingInteraction : MonoBehaviour {

    public GridManager _gridManager;
    private PlayerReferences _playerRefs;

    public Platform _highlightedPlatform;

    private bool _buildAction;
    [SerializeField]
    private bool _waitingForAction;
    [SerializeField]
    private bool _buildingMenuActive;

    private Coroutine _buildingRoutine;
    private NavMeshSurface _navSurface;
    private int _currentBuildingOp;
    private bool _canBuild;

    private void Start()
    {
        _playerRefs = GetComponent<PlayerReferences>();
        _gridManager = DataPipe.instance.GridManager;

        _navSurface = FindObjectOfType<NavMeshSurface>();
        _navSurface.BuildNavMesh();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(HighlightNewPlatform());
        }
    }

    public Platform GetCurrentPlatform()
    {
        return _highlightedPlatform;
    }

    public void ReceiveBuildingOperation(bool build)
    {
        if (!_canBuild)
            return;

        if (build)
        {
            
            _buildAction = build;
            _waitingForAction = false;
        }
        else
        {
            _highlightedPlatform.Deactivate();
            _buildAction = build;
            _waitingForAction = false;
            _buildingMenuActive = false;

        }
    }

    public void StartBuildingProcess(int op)
    {
        _currentBuildingOp = op;
        Debug.Log("op: " + op);
        _buildingMenuActive = true;

        if (_buildingRoutine != null)
            StopCoroutine(_buildingRoutine);
        _buildingRoutine = StartCoroutine(HighlightNewPlatform());
    }

    public IEnumerator HighlightNewPlatform()
    {

        _waitingForAction = true;


        Vector3 lastPos = Vector3.zero;
        Vector3 lastDir = Vector3.zero;
        while (_buildingMenuActive)
        {
            while (_waitingForAction)
            {            
                if(lastPos != _playerRefs.CurrentPlatform.transform.localPosition ||lastDir != _playerRefs.LookDirection)
                {

                    _highlightedPlatform?.Deactivate();            
                    _highlightedPlatform = _gridManager.GetPlatformForHighlight(_playerRefs.CurrentPlatform.transform.localPosition, _playerRefs.LookDirection, _currentBuildingOp);
                    _highlightedPlatform.Highlight(_currentBuildingOp);

                    if (!_highlightedPlatform.Activated)
                        _canBuild = true;
                    else _canBuild = false;
                }

                lastPos = _playerRefs.CurrentPlatform.transform.localPosition;
                lastDir = _playerRefs.LookDirection;

                yield return new WaitForSeconds(0.2f);


            }

            if (_buildAction)
            {

                _highlightedPlatform?.Activate();
                _waitingForAction = true;
                _navSurface.BuildNavMesh();
            }

            _highlightedPlatform = null;

            yield return null;
        }

        
    }
}
