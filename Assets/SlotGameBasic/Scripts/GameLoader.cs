using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        
        this.scenesToLoad.AddRange(scenesToLoad);

        foreach (var sceneToLoad in scenesToLoad)
        {
            yield return coroutineRunner.Execute(LoadScene(sceneToLoad));
        }
        
        scenesToLoad.Clear();
        
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
                Debug.LogError($"LoadSceneAsync returned null! Scene '{name}' not found!");
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
    private CoroutineRunner coroutineRunner;
    private ScenesLoader sceneLoader;
    private SceneLoadConfig sceneLoadConfig;

    private float currentLoadingProgress;
    
    public event Action<float> onProgressChanged;

    public void Init(CoroutineRunner coroutineRunner, SceneLoadConfig sceneLoadConfig)
    {
        this.sceneLoadConfig = sceneLoadConfig;
        this.coroutineRunner = coroutineRunner;

        sceneLoader = new ScenesLoader(this.coroutineRunner);

        currentLoadingProgress = 0.0f;
    }

    public IEnumerator InitialLoad()
    {
        var executionSteps = GetInitialLoadingSteps();

        int executedParts = 0;
        foreach (var executionStep in executionSteps)
        {
            yield return coroutineRunner.Execute(executionStep);
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
        
        executionSteps.Add(sceneLoader.Load(sceneLoadConfig.scenesToLoad));

        return executionSteps;
    }
}