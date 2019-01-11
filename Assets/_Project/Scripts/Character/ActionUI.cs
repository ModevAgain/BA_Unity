using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

public class ActionUI : BA_BaseUIElement{

    public Sprite Normal;
    public Sprite Confirm;

    public ActionUI ActionUIOther;

    public Action Trigger;
    private RectTransform _rect;
    private Image _img;
    private CanvasGroup _CG;
    private bool _toggled;

    private void Start()
    {
        _rect = GetComponent<RectTransform>();
        _img = GetComponent<Image>();
        _CG = GetComponent<CanvasGroup>();
    }

    
    public override void OnPointerEnter(PointerEventData eventData)
    {
        // base.OnPointerEnter(eventData);
        if (!_toggled) { }
            //_rect.DOScale(1.15f, 0.15f);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        //base.OnPointerExit(eventData);

        if (!_toggled) { }
            //_rect.DOScale(1f, 0.15f);
    }


    public override void OnPointerClick(PointerEventData eventData)
    {
        //base.OnPointerClick(eventData);
        Trigger.Invoke();
        _toggled = true;
    }

    public void SetConfirmSprite()
    {
        _img.sprite = Confirm;
        _img.transform.DOScale(1f, 0.1f);
        //_CG.DOFade(0, 0.05f).OnComplete(() =>
        //{
        //    _img.sprite = Confirm;
        //    _CG.DOFade(1, 0.05f);
        //});
    }

    public void Hide()
    {
        _img.sprite = Normal;
        _img.transform.DOScale(0.96f, 0.1f);
        //_CG.DOFade(0, 0.05f).OnComplete(() =>
        //{
        //    _img.sprite = Normal;
        //});
    }

    public void ResetVisualFuntionality()
    {
        _toggled = false;
        OnPointerExit(new PointerEventData(EventSystem.current));
    }

}
