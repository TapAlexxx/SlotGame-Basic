using System.Collections;
using DG.Tweening;
using UnityEngine;


[RequireComponent(typeof(CoroutineRunner), typeof(UIAdmin))]
public class Game : MonoBehaviour
{
    [field:SerializeField] public CoroutineRunner coroutineRunner { get; private set; }
    [field:SerializeField] public UIAdmin uiAdmin { get; private set; }
    [field:SerializeField] public ServerAdmin serverAdmin { get; private set; }
    [field:SerializeField] public ConfigAdmin configAdmin { get; private set; }
    
    [Header("Loading")]
    [SerializeField] private LoadingCurtain loadingCurtain;


    public static Game instance { get; private set; }
    public GameLoader gameLoader {get; private set;}
    public IServerAPI serverAPI { get; private set; }

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
        DOTween.Init();
        
        //serverAPI ??= new ServerAPI();
        serverAPI ??= new FakeServerAPI();
        
        gameLoader ??= new GameLoader();
        gameLoader.Init(this);
        
        uiAdmin.Init(this);
        serverAdmin.Init(this);
        configAdmin.Init(this);
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
