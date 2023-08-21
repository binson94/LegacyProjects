using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_PointHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Action<PointerEventData> OnEnterHandler = null;
    public Action<PointerEventData> OnExitHandler = null;
    public Action<PointerEventData, int> OnEnterHandler_idx = null;
    public Action<PointerEventData, int> OnExitHandler_idx = null;
    public int idx = -1;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (idx >= 0)
            OnEnterHandler_idx?.Invoke(eventData, idx);
        else
            OnEnterHandler?.Invoke(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (idx >= 0)
            OnExitHandler_idx?.Invoke(eventData, idx);
        else
            OnExitHandler?.Invoke(eventData);
    }
}