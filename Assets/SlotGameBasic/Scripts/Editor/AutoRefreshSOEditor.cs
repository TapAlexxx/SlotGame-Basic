using UnityEditor;
using UnityEngine;

namespace SlotGameBasic.Scripts.Editor 
{
    [CustomEditor(typeof(ScriptableObject), true)]
    public class AutoRefreshSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (target is IAutoRefreshable refreshable)
            {
                GUILayout.Space(10);
                if (GUILayout.Button("Refresh Data"))
                {
                    refreshable.RefreshData();
                }
            }
        }
    }
    
}