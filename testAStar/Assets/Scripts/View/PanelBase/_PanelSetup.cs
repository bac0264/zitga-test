using UnityEngine;
using System.Collections;

public class _PanelSetup<T, T1> : BasePanel
{
    public _SlotListSetup<T, T1> SlotListManager;

    public virtual void Setup(T1[] dataBase = null)
    {
        if (SlotListManager == null) SlotListManager = GetComponentInChildren<_SlotListSetup<T, T1>>();
        if (SlotListManager != null) SlotListManager.Setup(dataBase);
    }
    public override void OnValidate()
    {
        base.OnValidate();
    }
    public override void HidePanel()
    {
        gameObject.SetActive(false);
    }
    public override void HideKey()
    {
        gameObject.SetActive(false);
    }
    public override void ShowPanel()
    {
        gameObject.SetActive(true);
    }
    public override void ShowKey()
    {

    }
}
