using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
namespace LB3D.PuggosWorld.Unturned
{
    public class UnturnedSetup : Editor
    {
        

        [MenuItem("PuggosWorld/Setup/1: Set Unturned Root Directory")]
        public static void SetUnturnedInstallationDirectory()
        {
            string path = GetInstallDirectoryWindow.GetUnturnedInstallationPath();
            UnturnedModsGlobalSettingsObject.Instance.SetGlobalUnturnedInstallationDirectoryPath(path);
            EditorUtility.SetDirty(UnturnedModsGlobalSettingsObject.Instance);
        }

        [MenuItem("PuggosWorld/Setup/2: Import Unturned Project")]
        public static void ImportUnturnedProject()
        {
            AssetDatabase.ImportPackage(Path.Combine(UnturnedModsGlobalSettingsObject.Instance.unturnedInstallationPath, "Bundles", "Sources", "Project.unitypackage"),  true);
        }
        [MenuItem("PuggosWorld/Setup/3: Create Modding Directory")]
        public static void CreateModdingDirectory()
        {
            string moddingDirectory = Path.Combine(Application.dataPath, "UnturnedMods");
            Directory.CreateDirectory(moddingDirectory);
            AssetDatabase.Refresh();
        }

        [MenuItem("PuggosWorld/Setup/4: Import Unturned Example Content")]
        public static void ImportUnturnedExampleContent()
        {
            AssetDatabase.ImportPackage(Path.Combine(UnturnedModsGlobalSettingsObject.Instance.unturnedInstallationPath, "Bundles", "Sources", "ExampleAssets.unitypackage"), true);

        }
        [MenuItem("PuggosWorld/Setup/5: [Optional] Create Item Database")]
        public static void CreateItemDatabase()
        {
            Debug.LogWarning("This will take a while...please be patient.");
            UnturnedModsGlobalSettingsObject.Instance.GenerateDatFileDataBase();

        }
    }
}