using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class InputCommon : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Serializable]
    public class PointDownEvent : UnityEvent { }

    [SerializeField]
    private PointDownEvent m_OnPointDown = new PointDownEvent();

    [Serializable]
    public class PointUpEvent : UnityEvent { }

    [SerializeField]
    private PointUpEvent m_OnPointUp = new PointUpEvent();

    public void OnPointerDown(PointerEventData eventData)
    {
        m_OnPointDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_OnPointUp?.Invoke();
    }
}
