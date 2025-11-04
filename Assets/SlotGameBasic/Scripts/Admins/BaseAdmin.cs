using UnityEngine;

public abstract class BaseAdmin : MonoBehaviour
{
    public Game game { get; private set; }

    public void Init(Game game)
    {
        this.game = game;
    }
}