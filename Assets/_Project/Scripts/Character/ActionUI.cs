using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

public class ActionUI : MonoBehaviour, IPointerEnterHandler,IPointerUpHandler, IPointerExitHandler/*, IPointerClickHandler*/ {

    public Sprite Normal;
    public Sprite Confirm;

    public Action Trigger;
    private RectTransform _rect;
    private Image _img;
    private CanvasGroup _CG;

    private void Start()
    {
        _rect = GetComponent<RectTransform>();
        _img = GetComponent<Image>();
        _CG = GetComponent<CanvasGroup>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _rect.DOScale(1.15f, 0.15f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _rect.DOScale(1f, 0.15f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Trigger.Invoke();
    }

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    Trigger.Invoke();
    //}

    public void SetConfirmSprite()
    {
        _CG.DOFade(0, 0.05f).OnComplete(() =>
        {
            _img.sprite = Confirm;
            _CG.DOFade(1, 0.05f);
        });
    }

    public void Hide()
    {
        _CG.DOFade(0, 0.05f).OnComplete(() =>
        {
            _img.sprite = Normal;
        });
    }

    public void Reset()
    {
        _CG.DOFade(1, 0.0f).OnComplete(() =>
        {
            _img.sprite = Normal;
        });
    }
}
