using UnityEngine;
using System.Collections;
public enum PanelType
{
    MapPanel,
}
public class BasePanel : MonoBehaviour
{
    public Animator animator;
    public PanelType panelType;
    public virtual void OnValidate()
    {
        if (animator == null) animator = GetComponent<Animator>();
    }
    public virtual void HidePanel()
    {
        gameObject.SetActive(false);
    }
    public virtual void HideKey()
    {
        gameObject.SetActive(false);
    }
    public virtual void ShowPanel()
    {
        gameObject.SetActive(false);
    }
    public virtual void ShowKey()
    {

    }
}
