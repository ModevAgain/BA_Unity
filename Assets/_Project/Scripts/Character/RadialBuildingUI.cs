using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class RadialBuildingUI : MonoBehaviour, IPointerDownHandler {

    public CanvasGroup ActionGroup;

    public ActionUI RightAction;
    public ActionUI WrongAction;

    private Image _img;

    private BuildingInteraction _builder;
    private bool _active;

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

        RightAction.Trigger = () => 
        {
            ActionGroup.transform.DOScale(0, 0.2f);
            _img.DOFade(1, 0.2f);
            _builder.ReceiveBuildingOperation(false);
        };
        WrongAction.Trigger = () =>
        {
            ActionGroup.transform.DOScale(0, 0.2f);
            _img.DOFade(1, 0.2f);
            _builder.ReceiveBuildingOperation(true);
            _active = false;
        } ;

    }
	
	// Update is called once per frame
	void Update () {
		


        if(Input.GetAxis("Horizontal_2") != 0 && !_active)
        {
            _active = true;
            _builder.StartBuildingProcess();

            _img.DOFade(0, 0.3f);
            ActionGroup.transform.DOScale(1, 0.3f);
        }
        else if (_active)
        {
            float angle = Mathf.Atan2(Input.GetAxis("Vertical_2"), Input.GetAxis("Horizontal_2")) * Mathf.Rad2Deg;

            if(angle < 180 && angle > 135)
            {
                WrongAction.OnPointerEnter(new PointerEventData(EventSystem.current));
                RightAction.OnPointerExit(new PointerEventData(EventSystem.current));
            }
            else if (angle < 135 && angle > 90)
            {
                RightAction.OnPointerEnter(new PointerEventData(EventSystem.current));
                WrongAction.OnPointerExit(new PointerEventData(EventSystem.current));
            }
            else
            {
                WrongAction.OnPointerExit(new PointerEventData(EventSystem.current));
                RightAction.OnPointerExit(new PointerEventData(EventSystem.current));
            }

        }

	}
}
