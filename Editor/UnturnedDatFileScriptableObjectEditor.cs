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
            //base.OnInspectorGUI(); Removed so it does not draw everything twice
            var script = (UnturnedDatFileScriptableObject)target;

            // Start the serialized object update
            serializedObject.Update();

            // Draw properties using an iterator, but control the order
            SerializedProperty prop = serializedObject.GetIterator();
            prop.NextVisible(true); // Skip the script field

            while (prop.NextVisible(false))
            {
                if (prop.name == "hasId")
                {
                    // Draw the hasId toggle
                    EditorGUILayout.PropertyField(prop, true);

                    // Conditionally show the ID field based on hasId
                    if (script.hasId)
                    {
                        script.id = EditorGUILayout.IntField("ID", script.id);
                    }
                    else
                    {
                        script.id = 0; // Reset ID if hasId is false
                    }
                }
                else if (prop.name == "hasRarity")
                {
                    // Draw the hasRarity toggle
                    EditorGUILayout.PropertyField(prop, true);

                    // Conditionally show the Rarity field based on hasRarity
                    if (script.hasRarity)
                    {
                        script.rarity = (UnturnedDatFileScriptableObject.Rarity)EditorGUILayout.EnumPopup("Rarity", script.rarity);
                    }
                }
                else if (prop.name == "hasDescription")
                {
                    // Draw the hasDescription toggle
                    EditorGUILayout.PropertyField(prop, true);

                    // Conditionally show the description field based on hasDescription
                    if (script.hasDescription)
                    {
                        script.descriptionEnglish = EditorGUILayout.TextArea(script.descriptionEnglish, GUILayout.Height(80));
                    }
                    else
                    {
                        script.descriptionEnglish = string.Empty; // Clear description if hasDescription is false
                    }
                }
                else if (prop.name != "id" && prop.name != "rarity" && prop.name != "descriptionEnglish")
                {
                    // Draw all other properties except the ones we've handled manually
                    EditorGUILayout.PropertyField(prop, true);
                }
            }
            // Draw custom buttons at the bottom
            if (GUILayout.Button("Clone From .dat File", GUILayout.Height(40)))
            {
                script.CloneFromDatFileObject();
            }
            if (GUILayout.Button("Clone From .dat as Master Bundle Override", GUILayout.Height(40)))
            {
                script.CloneFromDatFileObject(true);
            }
            if (GUILayout.Button("Regenerate GUID", GUILayout.Height(40)))
            {
                script.GenerateGuid(true);
                EditorUtility.SetDirty(script);
            }
            if (GUILayout.Button("Test", GUILayout.Height(40)))
            {
                script.Test();
            }
            if (GUILayout.Button("Clear", GUILayout.Height(20)))
            {
                script.Clear();
            }

            // Apply any changes to the serialized object
            serializedObject.ApplyModifiedProperties();

            // Mark as dirty if changes were made
            if (GUI.changed)
            {
                EditorUtility.SetDirty(script);
            }

        }
    }
}