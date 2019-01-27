using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using BA;
using System;

public class RadialBuildingUI : BA_BaseUIElement/*, IPointerDownHandler*/ {

    [Header("Input")]
    public BA_InputMapper InputMapper;
    public BA_ContextRegulator ContextRegulator;

    [Header("References")]
    public CanvasGroup ActionGroup;

    public ActionUI Action_1;
    public ActionUI Action_2;

    public EventCatcher BGCatcher;

    public Sprite BuildSprite;
    public Sprite AbortSprite;

    public ParticleSystem Particles;

    private Image _img;

    private BuildingInteraction _builder;
    [SerializeField]
    private bool _active;
    private bool _selected;
    private ActionUI _currentSelectedAction;
    private int _buildingOp;

    private ResourceManager _resourceMan;
    private PlatformData _platformData;

   

    // Use this for initialization
    void Start () {

        _img = GetComponent<Image>();
        _builder = DataPipe.instance.BuildingInteraction;
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

        _resourceMan = DataPipe.instance.ResourceManager;
        _platformData = DataPipe.instance.PlatformData;

        BA_InputReceiverUI.Instance.ActionKey += BuildMenu;
        BA_InputReceiverUI.Instance.ActionKey2 += ReceiveBuildCommand;
        BA_InputReceiverUI.Instance.ActionDirectional += ReceiveDirectionalInput;
        InputMapper.MoveInputVector3 += ReceivePlatformCommand;
        _builder.RessourceCheck = () => RessourceCheck();
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
            Particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            ContextRegulator.Building_Activated();
            OnPointerDown();
            //_currentSelectedAction = Action_1;
            //SelectAction();
            _active = true;

        }
        else
        {
            Particles.Play();
            _active = false;
            Build(false);
            ResetToNormal();
            ContextRegulator.Building_Deactivated();
        }
    }

    public void ReceiveBuildCommand()
    {
        if(_active)
            Build(true);
    }

    public void ReceivePlatformCommand(Vector3 input)
    {
        if (!_builder.CanBuild() || !_active || !_selected)
        {
            //Debug.Log("Cannot Build");
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(input);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000, 1 << 11))
        {
            if (hit.transform.GetComponent<Platform>() == _builder.GetCurrentPlatform())
            {
                Build(true);
            }
            else Debug.Log(hit.transform.name);
        }
        else Debug.Log("Didnt hit platform");
    }

    #endregion

    public void OnPointerDown(/*PointerEventData eventData*/)
    {
        if (_active)
            return;


        _active = true;
        //_img.DOFade(0.5f, 0f);
        _img.DOFade(0f, 0.1f).OnComplete(() =>
        {
            _img.sprite = AbortSprite;
            _img.DOFade(1, 0.1f);
        });
        ActionGroup.alpha = 0;
        ActionGroup.DOFade(1, 0.1f);
        ActionGroup.transform.DOScale(1, 0.1f);
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
    public void RessourceCheck()
    {
        if(_currentSelectedAction == Action_1 || _currentSelectedAction == null)
        {
            if (!_resourceMan.HasEnoughResource(_platformData.Platform1_Cost))
            {
                Action_1.Hide();
                _currentSelectedAction = null;
                Build(false);
            }
        }
        if(_currentSelectedAction == Action_2 || _currentSelectedAction == null)
        {
            if (!_resourceMan.HasEnoughResource(_platformData.Platform2_Cost))
            {
                Action_2.Hide();
                _currentSelectedAction = null;
                Build(false);
            }
        }
    }

    public void ReceiveDirectionalInput(Vector2 input)
    {
        if (!_active)
            return;

        float angle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;

        //Debug.Log(angle);

        if (angle < 45 && angle > -90)
        {
            //Action_1.OnPointerEnter(new PointerEventData(EventSystem.current));
            //Action_2.OnPointerExit(new PointerEventData(EventSystem.current));
            _currentSelectedAction = Action_1;
            SelectAction();
            
        }
        else if (angle < -90 || Mathf.Abs(angle) > (90))
        {            
            //Action_2.OnPointerEnter(new PointerEventData(EventSystem.current));
            //Action_1.OnPointerExit(new PointerEventData(EventSystem.current));
            _currentSelectedAction = Action_2;
            SelectAction();
        }
        else
        {
            //Action_2.OnPointerExit(new PointerEventData(EventSystem.current));
            //Action_1.OnPointerExit(new PointerEventData(EventSystem.current));
            _currentSelectedAction = null;
        }
    }


    public void SelectAction()
    {
        _selected = false;

        if (_currentSelectedAction == Action_1)
        {
            if (_resourceMan.HasEnoughResource(_platformData.Platform1_Cost))
            {
                _buildingOp = 0;
                Action_2.Hide();
                Action_1.SetConfirmSprite();
                _selected = true;
            }
            else
            {
                Action_1.Error();
            }
        }
        else if (_currentSelectedAction == Action_2)
        {
            if (_resourceMan.HasEnoughResource(_platformData.Platform2_Cost))
            {
                _buildingOp = 1;
                Action_1.Hide();
                Action_2.SetConfirmSprite();
                _selected = true;
            }
            else
            {
                Action_2.Error();
            }
        }

        if (_selected)
        {
            _builder.StartBuildingProcess(_buildingOp);
        }
    }

    public void ResetToNormal()
    {
        Action_1.Hide();
        Action_2.Hide();
        ActionGroup.alpha = 1;
        ActionGroup.DOFade(0, 0.2f);
        ActionGroup.transform.DOScale(0, 0.1f);
        _img.DOFade(0f, 0.1f).OnComplete(() =>
        {
            _img.sprite = BuildSprite;
            _img.DOFade(1, 0.1f);
        });
        _selected = false;
        _active = false;

    }

    public bool IsActive()
    {
        return _active;
    }
}
