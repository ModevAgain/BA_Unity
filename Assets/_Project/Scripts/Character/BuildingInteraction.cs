using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class BuildingInteraction : MonoBehaviour {

    public bool BUILDING_IN_PROGESS;
    
    public Platform _highlightedPlatform;

    public Action RessourceCheck;

    private GridManager _gridManager;
    private PlayerReferences _playerRefs;
    private PlatformData _platformData;


    private bool _buildAction;
    [SerializeField]
    private bool _waitingForAction;   
    public bool BuildingMenuActive;

    private Coroutine _buildingRoutine;
    private NavMeshSurface _navSurface;
    private int _currentBuildingOp;
    private bool _canBuild;
    private ResourceManager _resourceMan;

    private RadialBuildingUI _radialBuildingUI;

    private WaitForSeconds _waiter;

    private void Start()
    {
        _playerRefs = DataPipe.instance.PlayerReferences;
        _gridManager = DataPipe.instance.GridManager;
        _platformData = DataPipe.instance.PlatformData;
        _resourceMan = DataPipe.instance.ResourceManager;
        _radialBuildingUI = DataPipe.instance.BuildingUI;

        _navSurface = DataPipe.instance.NavMeshSurface;
        _navSurface.BuildNavMesh();

        _waiter = new WaitForSeconds(0.2f);
    }

    public Platform GetCurrentPlatform()
    {
        return _highlightedPlatform;
    }

    public void ReceiveBuildingOperation(bool build)
    {
        if (!_canBuild)
        {
            _buildAction = false;
            _waitingForAction = false;
            BUILDING_IN_PROGESS = false;
            return;
        }

        if (build)
        {
            bool enoughRessources = false;

            if(_currentBuildingOp == 0)
            {
                enoughRessources = _resourceMan.HasEnoughResource(_platformData.Platform1_Cost);
            }
            else if(_currentBuildingOp == 1)
            {
                enoughRessources = _resourceMan.HasEnoughResource(_platformData.Platform2_Cost);
            }

            if (!enoughRessources)
            {
                return;
            }


            _buildAction = true;
            _waitingForAction = false;
            BUILDING_IN_PROGESS = true;
        }
        else
        {
            if(_highlightedPlatform != null)
                _highlightedPlatform.Deactivate();

            _buildAction = false;
            _waitingForAction = false;
            BuildingMenuActive = false;

            _highlightedPlatform = null;
            StopCoroutine(_buildingRoutine);

            BUILDING_IN_PROGESS = false;

            _navSurface.BuildNavMesh();

        }
    }

    public void StartBuildingProcess(int op)
    {
        _currentBuildingOp = op;
        //Debug.Log("op: " + op);
        BuildingMenuActive = true;

        if (_buildingRoutine != null)
            StopCoroutine(_buildingRoutine);
        _buildingRoutine = StartCoroutine(HighlightNewPlatform());
    }

    public IEnumerator HighlightNewPlatform()
    {

        _waitingForAction = true;


        Vector3 lastPos = Vector3.zero;
        Vector3 lastDir = Vector3.zero;
        while (BuildingMenuActive)
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

                yield return _waiter;


            }

            if (_buildAction)
            {

                _highlightedPlatform?.Activate();

                if(_currentBuildingOp == 0)
                    _resourceMan.DecrementResource(_platformData.Platform1_Cost);
                else _resourceMan.DecrementResource(_platformData.Platform2_Cost);

                //Wait a frame for Wall Deactivation
                for (int i = 0; i < 3; i++)
                {
                    yield return null;
                }

                _waitingForAction = true;
                //_navSurface.BuildNavMesh();
                _navSurface.UpdateNavMesh(_navSurface.navMeshData);

                BUILDING_IN_PROGESS = false;
                RessourceCheck();
            }

            _highlightedPlatform = null;

            yield return null;
        }
    }

    public bool CanBuild()
    {
        return _canBuild;
    }

}
