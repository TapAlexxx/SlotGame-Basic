using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    [SerializeField] protected Animation showAnimator;
    
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
    
    protected virtual void OnInit(){}
    protected virtual void OnShutDown(){}
    
    protected virtual void OnUIShow(){}
    protected virtual void OnUIHide(){}
}