using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Basic Custom UI Element.
/// 
/// <para>
/// Inherit for functionality concerning the BA namespace.
/// </para>
/// </summary>
public class BA_BaseUIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// Override for visual implementation of PointerClick.  
    /// </summary>
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("OnClick " + gameObject.name);
    }

    /// <summary>
    /// Override for visual implementation of PointerEnter
    /// </summary>
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnEnter " + gameObject.name);
    }

    /// <summary>
    /// Override for visual implementation of PointerExit
    /// </summary>
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("OnExit" + gameObject.name);
    }
}
