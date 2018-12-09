using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using BA;

public class RadialBuildingUI : BA_BaseUIElement/*, IPointerDownHandler*/ {

    public bool MOBILE;

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


    #region BA_Input

    public override void OnPointerClick(PointerEventData eventData)
    {
        BuildMenu();
    }

    public void BuildMenu()
    {
        Debug.Log("BuildMenu | active =  " + _active);

        if (!_active)
            OnPointerDown();
        else ResetToNormal();
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

    // Use this for initialization
    void Start () {

        _img = GetComponent<Image>();
        _builder = FindObjectOfType<BuildingInteraction>();
        ActionGroup.transform.DOScale(0, 0);

        Action_1.Trigger = () => 
        {
            if (_selected)
                Build(true);
            else
            {
                _currentSelectedAction = Action_1;
                SelectAction();
            }
        };
        Action_2.Trigger = () =>
        {
            if (_selected)
                Build(true);
            else
            {
                _currentSelectedAction = Action_2;
                SelectAction();
            }
        };

        //BGCatcher.OnUpAction = () =>
        //{
        //    if (!_selected)
        //        ResetToNormal();
        //};

        BA_InputReceiverUI.Instance.BuildingMenu += BuildMenu;

    }

    private void OnApplicationQuit()
    {
        BA_InputReceiverUI.Instance.BuildingMenu -= BuildMenu;
    }

    // Update is called once per frame
    void Update () {

        if (MOBILE)
            return;

        //GamepadInteractionInUpdate();


        if (_active && _selected)
        {
            if (Input.GetButtonDown("A"))
            {
                Build(true);
            }
            if (Input.GetButtonDown("B"))
            {
                Build(false);
            }
        }
    }

    public void Build(bool build)
    {
        _builder.ReceiveBuildingOperation(build);

        
        _img.DOFade(0, 0.15f).OnComplete(() =>
        {
            _img.sprite = BuildSprite;
            _img.DOFade(1, 0.15f).OnComplete(() =>
            {
                _active = false;
                _selected = false;
            });
        });
    }

    public void GamepadInteractionInUpdate()
    {
        if (_selected)
            return;

        if (Input.GetAxis("Horizontal_2") != 0 && !_active)
        {
            _active = true;

            _img.DOFade(0, 0f);
            ActionGroup.transform.DOScale(1, 0.05f);
        }
        else if (_active)
        {
            float x = Input.GetAxis("Vertical_2");
            float y = Input.GetAxis("Horizontal_2");

            float angle = Mathf.Atan2(x,y) * Mathf.Rad2Deg;

            if (angle < 180 && angle > 135)
            {
                Action_2.OnPointerEnter(new PointerEventData(EventSystem.current));
                Action_1.OnPointerExit(new PointerEventData(EventSystem.current));
                _currentSelectedAction = Action_2;
            }
            else if (angle < 135 && angle > 90)
            {
                Action_1.OnPointerEnter(new PointerEventData(EventSystem.current));
                Action_2.OnPointerExit(new PointerEventData(EventSystem.current));
                _currentSelectedAction = Action_1;
            }
            else
            {
                Action_2.OnPointerExit(new PointerEventData(EventSystem.current));
                Action_1.OnPointerExit(new PointerEventData(EventSystem.current));
            }

            if(x == 0 && y == 0)
            {                
                _currentSelectedAction.OnPointerUp(new PointerEventData(EventSystem.current));

                SelectAction();
            }

        }
    }

    public void SelectAction()
    {
        _selected = true;

        if (_currentSelectedAction == Action_1)
        {
            Action_1.SetConfirmSprite();
            Action_2.Hide();
        }
        else
        {
            Action_2.SetConfirmSprite();
            Action_1.Hide();
        }

        _img.sprite = AbortSprite;
        _img.DOFade(1, 0.05f);

        //ActionGroup.transform.DOScale(0, 0.1f);

        _builder.StartBuildingProcess();
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
