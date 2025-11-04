using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "IconConfigAsset", menuName = "SlotGame/Configs/IconConfigAsset")]
public class IconConfigAssets : ScriptableObject, IAutoRefreshable
{
    public string iconsFolder = "Icons";
    public Dictionary<string, Sprite> icons = new();
    
#if UNITY_EDITOR
    public void RefreshData()
    {
        icons.Clear();

        var guids = AssetDatabase.FindAssets("t:Sprite", new[] { iconsFolder });

        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            icons.Add(guid, sprite);
        }

        EditorUtility.SetDirty(this);
    }
#endif

    public Sprite Get(string iconID)
    {
        return icons[iconID];
    }
}