using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

public class ActionUI : MonoBehaviour, IPointerEnterHandler,IPointerUpHandler, IPointerExitHandler {


    public Action Trigger;
    private RectTransform _rect;


    private void Start()
    {
        _rect = GetComponent<RectTransform>();
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
}
