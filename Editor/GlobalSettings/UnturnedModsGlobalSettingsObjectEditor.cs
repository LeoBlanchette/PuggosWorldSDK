using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace LB3D.PuggosWorld.Unturned
{
    [CustomEditor(typeof(UnturnedModsGlobalSettingsObject))]
    public class UnturnedModsGlobalSettingsObjectEditor : Editor
    {
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var script = (UnturnedModsGlobalSettingsObject)target;

            if (GUILayout.Button("Set Unturned Installation Path", GUILayout.Height(40)))
            {
                string path = GetInstallDirectoryWindow.GetUnturnedInstallationPath();
                script.SetGlobalUnturnedInstallationDirectoryPath(path);
                EditorUtility.SetDirty(script);
            }
            if (GUILayout.Button("Generate .dat File Database.", GUILayout.Height(40)))
            {
                script.GenerateDatFileDataBase();
            }
        }
    }
}