using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneLoadConfig", menuName = "SlotGame/Configs/SceneLoadConfig")]
public class SceneLoadConfig : ScriptableObject
{
    public List<string> scenesToLoad = new List<string>();
}