using System.IO;
using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "IconConfigAsset", menuName = "SlotGame/Configs/IconConfigAsset")]
public class IconConfigAssets : ScriptableObject, IAutoRefreshable
{
    public string targetFolder = "Icons";
    public SerializedDictionary<string, Sprite> icons = new();
    
#if UNITY_EDITOR
    public void RefreshData()
    {
        icons.Clear();
        var pathToThis = AssetDatabase.GetAssetPath(this);     
        var folderPath = Path.GetDirectoryName(pathToThis);    
        var iconsFolder = folderPath + $"/{targetFolder}"; 
        
        var guids = AssetDatabase.FindAssets("t:Sprite", new[] { iconsFolder });
        
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            var spriteName = Path.GetFileNameWithoutExtension(path);
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            
            icons.Add(spriteName, sprite);
        }

        EditorUtility.SetDirty(this);
    }
#endif

    public Sprite Get(string iconID)
    {
        return icons[iconID];
    }
}