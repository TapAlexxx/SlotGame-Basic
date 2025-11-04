using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(CoroutineRunner), typeof(UIAdmin))]
public class Game : MonoBehaviour
{
    [field:SerializeField] public CoroutineRunner coroutineRunner { get; private set; }
    [field:SerializeField] public UIAdmin uiAdmin { get; private set; }
    
    [Header("Loading")]
    [SerializeField] private LoadingCurtain loadingCurtain;

    [Header("Configs")]
    [SerializeField] private SceneLoadConfig sceneLoadConfig;


    public static Game instance { get; private set; }
    public GameLoader gameLoader {get; private set;}
    public static FakeServer fakeServer { get; private set; }

    private void OnValidate()
    {
        coroutineRunner ??= GetComponent<CoroutineRunner>();
        uiAdmin ??= GetComponent<UIAdmin>();
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
        StartCoroutine(InitialLoad());
    }

    private void Init()
    {
        //Prewarm DoTween
        DOTween.Init();
        
        fakeServer ??= new FakeServer();
        
        gameLoader ??= new GameLoader();
        gameLoader.Init(this, sceneLoadConfig);
        
        uiAdmin.Init();
        
        loadingCurtain.Init(this);
    }

    private IEnumerator InitialLoad()
    {
        loadingCurtain.gameObject.SetActive(true);
        loadingCurtain.Show();
        
        yield return coroutineRunner.Execute(gameLoader.InitialLoad());
        
        uiAdmin.GrabUIBases();

        loadingCurtain.RefreshLoadingProgress(1.0f, ()=> loadingCurtain.Hide());
    }
}
