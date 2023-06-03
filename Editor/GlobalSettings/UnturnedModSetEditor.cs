using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SDG.Unturned.Tools;
namespace LB3D.PuggosWorld.Unturned
{
    [CustomEditor(typeof(UnturnedModSetObject))]
    public class UnturnedModSetEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var modSet = (UnturnedModSetObject)target;

            if (GUILayout.Button("Generate Mods", GUILayout.Height(40)))
            {
                foreach (UnturnedModScriptableObject script in modSet.unturnedMods)
                {
                    script.GenerateMod();
                    EditorAssetBundleHelper.Build(script.GetMasterBundleName(), script.GetModDirectory(), false);
                    script.GenerateMasterBundleEntry();
                }
            }

            if (GUILayout.Button("Generate Ids", GUILayout.Height(40)))
            {                
                IDGenerator idGenerator = new IDGenerator(modSet);
                foreach (UnturnedModScriptableObject unturnedModScriptableObject in modSet.unturnedMods)
                {

                    foreach (UnturnedDatFileScriptableObject datFile in unturnedModScriptableObject.datFiles)
                    {
                        if (datFile.IsIdLocked())
                        {                            
                            continue;
                        }
                  
                        datFile.id = idGenerator.GetNewId();
                        datFile.LockId();
                        Debug.Log("ID " + datFile.id + " assigned to " + datFile.nameEnglish);
                        EditorUtility.SetDirty(datFile);                       
                    }
                }
                idGenerator = null;
            }
            if (GUILayout.Button("Re-Generate GUIDs", GUILayout.Height(40)))
            {
                foreach (UnturnedModScriptableObject unturnedModScriptableObject in modSet.unturnedMods)
                {

                    foreach (UnturnedDatFileScriptableObject datFile in unturnedModScriptableObject.datFiles)
                    {
                        
                        datFile.GenerateGuid(true);
                        EditorUtility.SetDirty(datFile);
                        Debug.LogWarning("GUIDS regenerated. Save project now.");
                    }
                }
            }
        }
    }
}