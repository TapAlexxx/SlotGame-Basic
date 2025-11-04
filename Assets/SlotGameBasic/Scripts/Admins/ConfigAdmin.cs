using UnityEngine;

public class ConfigAdmin : BaseAdmin
{
    [field:SerializeField] public SceneLoadConfig sceneLoadConfig { get; private set; }
    [field:SerializeField] public IconConfigAssets symbolIcons { get; private set; }   
}