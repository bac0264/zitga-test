using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public enum TypeOfPopup
{
    ShopPopup,
    SpinPopup,
    StoryPopup,
    LanguagePopup,
    ProfilePopup,
    BaseStatPopup,
}
public class BasePopup<T> : BasePopupSimple
{ 
    public virtual void SetupData(T _data = default,List<T> data = null, string message = null, int VALUE = 0)
    {

    }
}
public class BasePopupSimple : MonoBehaviour
{
    public Animator animator;
    private void OnValidate()
    {
        if (animator == null) animator = GetComponent<Animator>();
    }
    public virtual void ShowPopup()
    {
        gameObject.SetActive(true);
    }
    // sau khi show se xu ly ....
    public virtual void ShowKey()
    {

    }
    public virtual void HidePopup()
    {
        animator.Play("HidePopup");
    }
    // sau khi hide se xu ly. ....
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
    public TypeOfPopup type;
}