using JetBrains.Annotations;
using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    [SerializeField] protected Animator showAnimator;
    
    private bool inited;
    
    protected UIAdmin admin;

    public void Init(UIAdmin uiAdmin)
    {
        admin = uiAdmin;
        OnInit();
        
        inited = true;
    }

    public void ShutDown()
    {
        if(!inited)
            return;
        
        OnShutDown();
    }

    public void Show()
    {
        if(showAnimator != null)
            showAnimator.Play("Show");
        OnUIShow();
    }

    public void Hide()
    {
        if(showAnimator != null)
            showAnimator.Play("Hide");
        OnUIHide();
    }
    
    //Used from animation event
    [UsedImplicitly]
    public void OnHideCompleteHandler()
    {
        gameObject.SetActive(false);
    }
    
    protected virtual void OnInit(){}
    protected virtual void OnShutDown(){}
    
    protected virtual void OnUIShow(){}
    protected virtual void OnUIHide(){}
}