using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    protected Game game;

    public virtual void Init(Game game)
    {
        this.game = game;
    }
    
    public abstract void Show();
    public abstract void Hide();
}