using JetBrains.Annotations;
using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    [SerializeField] protected Animator showAnimator;
    
    protected Game game;

    public void Init(Game game)
    {
        this.game = game;
        OnInit();
    }

    public void ShutDown()
    {
        OnShutDown();
        game = null;
    }

    public void Show()
    {
        showAnimator.Play("Show");
    }

    public void Hide()
    {
        showAnimator.Play("Hide");
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