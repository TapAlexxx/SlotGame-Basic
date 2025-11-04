using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LoaderState
{
    NONE,
    RUNNING,
    SUCCESS,
    FAIL,
}

public abstract class BasicLoader<T>
{
    public LoaderState state { get; protected set; } = LoaderState.NONE;
    public abstract IEnumerator Load(T arg);
}

public class ScenesLoader : BasicLoader<List<string>>
{
    private List<string> scenesToLoad;
    private CoroutineRunner coroutineRunner;

    public ScenesLoader(CoroutineRunner coroutineRunner)
    {
        this.coroutineRunner = coroutineRunner;
    }
    
    public override IEnumerator Load(List<string> scenesToLoad)
    {
        SetState(LoaderState.RUNNING);

        this.scenesToLoad ??= new List<string>();
        this.scenesToLoad.AddRange(scenesToLoad);

        foreach (var sceneToLoad in scenesToLoad)
        {
            yield return coroutineRunner.Execute(LoadScene(sceneToLoad));
            
            if (state == LoaderState.FAIL)
            {
                Debug.LogError($"LoadSceneAsync returned null! Scene '{sceneToLoad}' not found!");
                yield break;
            }
        }
        
        this.scenesToLoad.Clear();
        
        SetState(LoaderState.SUCCESS);
    }
    
    private IEnumerator LoadScene(string name)
    {
        if (!SceneManager.GetSceneByName(name).isLoaded)
        {
            yield return new WaitForSeconds(0.1f);

            var operation = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            if (operation == null)
            {
                SetState(LoaderState.FAIL);
                yield break;
            }

            while (!operation.isDone)
            {
                yield return null;
            }
        }
    }

    private void SetState(LoaderState state)
    {
        base.state = state;
    }
}

public class GameLoader
{
    private ScenesLoader sceneLoader;

    private float currentLoadingProgress;
    private Game game;

    public event Action<float> onProgressChanged;

    public void Init(Game game)
    {
        this.game = game;

        sceneLoader = new ScenesLoader(this.game.coroutineRunner);

        currentLoadingProgress = 0.0f;
    }

    public IEnumerator InitialLoad()
    {
        var executionSteps = GetInitialLoadingSteps();

        int executedParts = 0;
        foreach (var executionStep in executionSteps)
        {
            yield return game.coroutineRunner.Execute(executionStep);
            executedParts++;
            RefreshLoadingProgress(executedParts, executionSteps.Count);
        }
    }

    private void RefreshLoadingProgress(int executedParts, int executionStepsCount)
    {
        currentLoadingProgress = executedParts / (float)executionStepsCount;
        onProgressChanged?.Invoke(currentLoadingProgress);
    }

    private List<IEnumerator> GetInitialLoadingSteps()
    {
        var executionSteps = new List<IEnumerator>();
        
        executionSteps.Add(sceneLoader.Load(game.configAdmin.sceneLoadConfig.scenesToLoad));

        return executionSteps;
    }
}