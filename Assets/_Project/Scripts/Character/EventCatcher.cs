using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventCatcher : MonoBehaviour, IPointerUpHandler/*, IPointerClickHandler*/ {

    public Action OnUpAction;

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    if (OnUpAction != null)
    //        OnUpAction.Invoke();
    //}

    public void OnPointerUp(PointerEventData eventData)
    {
        if (OnUpAction != null)
            OnUpAction.Invoke();
    }

    // Use this for initialization
    void Start () {
		
	}
	

}
