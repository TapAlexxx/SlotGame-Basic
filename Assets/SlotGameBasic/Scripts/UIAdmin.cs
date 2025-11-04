using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIAdmin : MonoBehaviour
{
    private List<UIBase> uiBases;

    public void Init()
    {
    }

    public void GrabUIBases()
    {
        uiBases ??= new List<UIBase>();
        uiBases.Clear();

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

    public void OnGameLoaded()
    {
        throw new System.NotImplementedException();
    }
}