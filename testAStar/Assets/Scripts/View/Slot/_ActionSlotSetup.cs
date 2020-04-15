using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;

[System.Serializable]
public class _ActionSlotSetup<T,T1> : EnhancedScrollerCellView, IPointerClickHandler
{
    private T1 data;
    public Text TXT_VALUE;
    public Image IMG_ICON;
    public Image IMG_BG;
    public Action<_ActionSlotSetup<T, T1>> OnRightClickEvent;
    public virtual T1 DATA
    {
        set
        {
            data = value;
        }
        get { return data; }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData != null && (eventData.button == PointerEventData.InputButton.Right || eventData.clickCount > 0))
        {
            if (OnRightClickEvent != null && this != null)
            {
                OnRightClickEvent(this);
            }
        }
    }
    //  public virtual AddData(T data)
    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    Debug.Log("run");
    //    if (eventData != null && (eventData.button == PointerEventData.InputButton.Right || eventData.clickCount > 0))
    //    {
    //        if (OnRightClickEvent != null && this != null)
    //        {
    //            OnRightClickEvent(this);
    //        }
    //    }

    //}
}