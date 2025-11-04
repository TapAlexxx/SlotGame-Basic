using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIAdmin : BaseAdmin
{
    private List<UIBase> uiBases;

    public void GrabUIBases()
    {
        uiBases ??= new List<UIBase>();
        uiBases.Clear();
        
        var grabbedUIBases = GameObject.Find("HUD")?.GetComponentsInChildren<UIBase>(true);
        if (grabbedUIBases == null) return;
        
        foreach (var uiBase in grabbedUIBases)
        {
            uiBase.Init(this);
            uiBases.Add(uiBase);
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