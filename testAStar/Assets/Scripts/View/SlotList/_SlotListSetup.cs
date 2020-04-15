using UnityEngine;
using System.Collections;
using System;

// T is BoosterSlot, T1 is BoosterStat
[System.Serializable]
public class _SlotListSetup<T, T1> : MonoBehaviour
{
    public _ActionSlotSetup<T, T1>[] slotList;

    public Action<_ActionSlotSetup<T, T1>> OnRightClick;
    public virtual void Start()
    {
        SetupEvent();
    }

    public T2 GetType<T2>()
    {
        return (T2)(object)this;
    }
    public virtual void OnValidate()
    {
        if (slotList == null || slotList.Length == 0) slotList = GetComponentsInChildren<_ActionSlotSetup<T, T1>>();
    }
    public virtual void Setup(T1[] dataBase = null)
    {
        // Debug.Log("run");
        if (slotList == null || slotList.Length == 0)
        {
            slotList = GetComponentsInChildren<_ActionSlotSetup<T, T1>>();
        }
        //  Debug.Log("run");
        if (dataBase != null) SetupSlotList(dataBase);
    }
    void SetupSlotList(T1[] dataBase)
    {
        //  Debug.Log("run");
        if (dataBase == null) return;
        int i = 0;
        for (; i < dataBase.Length && i < slotList.Length; i++)
        {
            slotList[i].DATA = dataBase[i];
            //Debug.Log(JsonUtility.ToJson(slotList[i].DATA));
            slotList[i].gameObject.SetActive(true);
        }
        for (; i < slotList.Length; i++)
        {
            slotList[i].gameObject.SetActive(false);
        }
    }
    public virtual void SetupEvent()
    {
        int i = 0;
        if (slotList == null || slotList.Length == 0)
            slotList = GetComponentsInChildren<_ActionSlotSetup<T, T1>>();
        for (; i < slotList.Length; i++)
        {
            slotList[i].OnRightClickEvent += OnRightClick;
        }
    }
}
