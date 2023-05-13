using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SDG.Unturned.Tools;
using System.IO;

namespace LB3D.PuggosWorld.Unturned
{

    [CustomEditor(typeof(UnturnedModScriptableObject))]
    public class UnturnedModScriptableObjectEditor : Editor
    {
        public override void OnInspectorGUI()
        {     
            base.OnInspectorGUI();
                
            //Process Actions
            EditorGUILayout.Space();

            var script = (UnturnedModScriptableObject)target;

            script.masterBundleChoices = AssetDatabase.GetAllAssetBundleNames();

            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            script.masterBundleSelected = EditorGUILayout.Popup("Master Bundle", script.masterBundleSelected, script.masterBundleChoices);

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(script);
            }

            if (GUILayout.Button("Generate Mod", GUILayout.Height(40)))
            {
                script.GenerateMod();
                EditorAssetBundleHelper.Build(script.masterBundleChoices[script.masterBundleSelected], script.GetModDirectory(), false);
                script.GenerateMasterBundleEntry();
            }


        }

    }
}