using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LB3D.PuggosWorld.Unturned
{
    [CustomEditor(typeof(UnturnedCraftingRecipeObject))]
    public class UnturnedCraftingRecipeObjectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();
            UnturnedCraftingRecipeObject script = (UnturnedCraftingRecipeObject)target;
            if (GUILayout.Button("Test Text Output", GUILayout.Height(40)))
            {
                string text = script.GetText(0, 0);
                Debug.LogWarning(text);
            }
        }
    }
}