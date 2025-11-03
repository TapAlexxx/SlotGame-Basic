using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIAdmin : MonoBehaviour
{
    private Game game;
    private List<UIBase> uiBases;

    public void Init(Game game)
    {
        this.game = game;
    }

    public IEnumerator GrabUIBases()
    {
        yield return null;
        uiBases = new List<UIBase>();

        int count = SceneManager.sceneCount;
        for (int i = 0; i < count; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            var roots = scene.GetRootGameObjects();
            foreach (var r in roots)
            {
                uiBases.AddRange(r.GetComponentsInChildren<UIBase>(true));
            }
        }
    }
    
    public void Show<T>() where T : UIBase
    {
        if (GetUIBase<T>(out var ui))
        {
            ui.gameObject.SetActive(true);
            ui.Show();
        }
        if (!ui)
        {
            Debug.LogWarning($"Missing ui '{typeof(T)}'!");
        }
    }

    private bool GetUIBase<T>(out T uiBase) where T : UIBase
    {
        uiBase = uiBases.FirstOrDefault(x => x is T) as T;
        return uiBase != null;
    }
}

[RequireComponent(typeof(CoroutineRunner))]
public class Game : MonoBehaviour
{
    [SerializeField] private CoroutineRunner coroutineRunner;
    [SerializeField] private LoadingCurtain loadingCurtain;

    [Header("Configs")]
    [SerializeField] private SceneLoadConfig sceneLoadConfig;


    public static Game instance { get; private set; }
    public GameLoader gameLoader {get; private set;}
    public static FakeServer fakeServer { get; private set; }

    private void OnValidate()
    {
        gameLoader ??= GetComponent<GameLoader>();
    }

    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        loadingCurtain.ShutDown();
    }

    private void Start()
    {
        BootstrapGame();
    }

    private void BootstrapGame()
    {
        Init();
        LoadInitial();
    }

    private void LoadInitial()
    {
        
    }

    private void Init()
    {
        //Prewarm DoTween
        DOTween.Init();
        
        fakeServer = new FakeServer();
        
        gameLoader.Init(coroutineRunner, sceneLoadConfig);
    }
}
