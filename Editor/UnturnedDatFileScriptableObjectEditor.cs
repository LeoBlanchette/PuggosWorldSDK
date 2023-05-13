using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LB3D.PuggosWorld.Unturned
{

    [CustomEditor(typeof(UnturnedDatFileScriptableObject))]
    public class UnturnedDatFileScriptableObjectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var script = (UnturnedDatFileScriptableObject)target;
            
            if (GUILayout.Button("Clone From .dat File", GUILayout.Height(40)))
            {
                script.CloneFromDatFileObject();
            }
            if (GUILayout.Button("Clear", GUILayout.Height(20)))
            {
                script.Clear();
            }
        }
    }
}