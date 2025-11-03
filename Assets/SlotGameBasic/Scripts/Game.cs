using UnityEngine;

[RequireComponent(typeof(CoroutineRunner))]
public class Game : MonoBehaviour
{
    [SerializeField] private CoroutineRunner coroutineRunner;
    [SerializeField] private LoadingCurtain loadingCurtain;

    [Header("Configs")]
    [SerializeField] private SceneLoadConfig sceneLoadConfig;
    
    
    private GameLoader gameLoader;
    
    public static Game instance { get; private set; }
    public static FakeServer fakeServer { get; private set; }

    private void OnValidate()
    {
        gameLoader ??= GetComponent<GameLoader>();
    }

    private void Awake()
    {
        instance = this;
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
        fakeServer = new FakeServer();
        
        gameLoader.Init(coroutineRunner, sceneLoadConfig);
    }
}
