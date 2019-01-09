using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using BA;

public class RadialBuildingUI : BA_BaseUIElement/*, IPointerDownHandler*/ {

    public BA_InputMapper InputMapper;


    public CanvasGroup ActionGroup;

    public ActionUI Action_1;
    public ActionUI Action_2;

    public EventCatcher BGCatcher;

    public Sprite BuildSprite;
    public Sprite AbortSprite;

    private Image _img;

    private BuildingInteraction _builder;
    private bool _active;
    private bool _selected;
    private ActionUI _currentSelectedAction;
    private int _buildingOp;

    // Use this for initialization
    void Start () {

        _img = GetComponent<Image>();
        _builder = FindObjectOfType<BuildingInteraction>();
        ActionGroup.transform.DOScale(0, 0);

        Action_1.Trigger = () => 
        {
            Action_2.ResetVisualFuntionality();
            _currentSelectedAction = Action_1;
            SelectAction();
        };
        Action_2.Trigger = () =>
        {
            Action_1.ResetVisualFuntionality();
            _currentSelectedAction = Action_2;
            SelectAction();           
        };

        BA_InputReceiverUI.Instance.ActionKey += BuildMenu;
        BA_InputReceiverUI.Instance.ActionKey2 += ReceiveBuildCommand;
        BA_InputReceiverUI.Instance.ActionDirectional += ReceiveDirectionalInput;
        InputMapper.MoveInputVector3 += ReceivePlatformCommand;

    }

    #region BA_Input

    public override void OnPointerClick(PointerEventData eventData)
    {
        BuildMenu();
    }

    public void BuildMenu()
    {
        
        if (!_active)
        {
            OnPointerDown();
            _currentSelectedAction = Action_1;
            SelectAction();
            _active = true;
        }
        else
        {
            _active = false;
            Build(false);
            ResetToNormal();
        }
    }

    public void ReceiveBuildCommand()
    {
        Build(true);
    }

    public void ReceivePlatformCommand(Vector3 input)
    {
        Ray ray = Camera.main.ScreenPointToRay(input);

        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 1 << 11))
        {
            if(hit.transform.GetComponent<Platform>() == _builder.GetCurrentPlatform() && _builder.CanBuild())
            {
                Build(true);
            }
        }
    }

    #endregion

    public void OnPointerDown(/*PointerEventData eventData*/)
    {
        if (_active)
            return;


        _active = true;
        _img.DOFade(0.5f, 0f);
        ActionGroup.transform.DOScale(1, 0.05f);
    }


    private void OnApplicationQuit()
    {
        BA_InputReceiverUI.Instance.ActionKey -= BuildMenu;
        BA_InputReceiverUI.Instance.ActionDirectional -= ReceiveDirectionalInput;
        InputMapper.MoveInputVector3 -= ReceivePlatformCommand;
    }



    public void Build(bool build)
    {        

        _builder.ReceiveBuildingOperation(build);

    }

    public void ReceiveDirectionalInput(Vector2 input)
    {
        if (!_active)
            return;

        float angle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;

        if (angle < 45 && angle > -45)
        {
            Action_1.OnPointerEnter(new PointerEventData(EventSystem.current));
            Action_2.OnPointerExit(new PointerEventData(EventSystem.current));
            _currentSelectedAction = Action_1;
            SelectAction();
            
        }
        else if (angle < -45 && angle > -145)
        {            
            Action_2.OnPointerEnter(new PointerEventData(EventSystem.current));
            Action_1.OnPointerExit(new PointerEventData(EventSystem.current));
            _currentSelectedAction = Action_2;
            SelectAction();
        }
        else
        {
            Action_2.OnPointerExit(new PointerEventData(EventSystem.current));
            Action_1.OnPointerExit(new PointerEventData(EventSystem.current));
        }
    }


    public void SelectAction()
    {
        _selected = true;

        if (_currentSelectedAction == Action_1)
        {
            _buildingOp = 0;
        }
        else if (_currentSelectedAction == Action_2)
        {
            _buildingOp = 1;
        }
        

        _builder.StartBuildingProcess(_buildingOp);
    }

    public void ResetToNormal()
    {
        ActionGroup.transform.DOScale(0, 0.1f);
        _img.sprite = BuildSprite;
        _img.DOFade(1, 0.05f);
        _selected = false;
        _active = false;

    }
}
