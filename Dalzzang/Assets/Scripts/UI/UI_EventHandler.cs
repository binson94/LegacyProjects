/*
작성자 : 이우열
작성일 : 23.03.31
최근 수정 일자 : 23.03.31
최근 수정 사항 : 기본 UI 시스템 구현
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler
{ 
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData, int> OnClickHandler_idx = null;
    public int idx = -1;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(idx >= 0)
            OnClickHandler_idx?.Invoke(eventData, idx);
        else
            OnClickHandler?.Invoke(eventData);
    }
}

