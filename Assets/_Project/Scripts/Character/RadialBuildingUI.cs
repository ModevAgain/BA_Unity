using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class RadialBuildingUI : MonoBehaviour, IPointerDownHandler {

    public CanvasGroup ActionGroup;

    public ActionUI Action_1;
    public ActionUI Action_2;

    public Sprite BuildSprite;
    public Sprite AbortSprite;

    private Image _img;

    private BuildingInteraction _builder;
    private bool _active;
    private bool _selected;
    private ActionUI _currentSelectedAction;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_active)
            return;
        _builder.StartBuildingProcess();

        _img.DOFade(0, 0.3f);
        ActionGroup.transform.DOScale(1, 0.3f);
    }

    // Use this for initialization
    void Start () {

        _img = GetComponent<Image>();
        _builder = FindObjectOfType<BuildingInteraction>();
        ActionGroup.transform.DOScale(0, 0);

        Action_1.Trigger = () => 
        {
            ActionGroup.transform.DOScale(0, 0.2f);
            _img.DOFade(1, 0.2f);
            _builder.ReceiveBuildingOperation(false);
        };
        Action_2.Trigger = () =>
        {
            ActionGroup.transform.DOScale(0, 0.2f);
            _img.DOFade(1, 0.2f);
            _builder.ReceiveBuildingOperation(true);
            _active = false;
        } ;

    }
	
	// Update is called once per frame
	void Update () {

        //If not mobile
        GamepadInteractionInUpdate();


        if(_active && _selected)
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

        _img.sprite = AbortSprite;
        _img.DOFade(1, 0.05f);

        ActionGroup.transform.DOScale(0, 0.1f);

        _builder.StartBuildingProcess();
    }
}
