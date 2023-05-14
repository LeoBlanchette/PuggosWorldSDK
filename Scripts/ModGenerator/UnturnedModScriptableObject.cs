using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace LB3D.PuggosWorld.Unturned
{
    [CreateAssetMenu(fileName = "ModSettings", menuName = "PuggosWorld/Unturned/ModSettings", order = 1)]
    public class UnturnedModScriptableObject : ScriptableObject
    {

        public enum Asset_Bundle_Version
        {
            Asset_Bundle_Version_4,
            Asset_Bundle_Version_3,
            Asset_Bundle_Version_2,
            Asset_Bundle_Version_1,
        }
                
        [Header("Mod Name (no spaces)")]
        [Tooltip("If the modname is ExampleMod then make sure the masterbundle is called examplemod.masterbundle.")]
        public string modName;

        public UnturnedDatFileScriptableObject[] datFiles;

        [Header("Master Bundle Attributes")]
        public Asset_Bundle_Version assetBundleVersion;
        public void GenerateMod()
        {
            
            string modFolder = GenerateModFolder();
            foreach (UnturnedDatFileScriptableObject datFile in datFiles)
            {
                GenerateDataFolder(datFile);
            }
        }

        public string GetModFolder(string modPath) {
           
            string modFolder = modPath.Replace("Assets/UnturnedMods/", "").Replace(Path.GetFileName(modPath), "");
          
            return modFolder;
        }
        public void GenerateDataFolder(UnturnedDatFileScriptableObject datFile)
        {
            #if UNITY_EDITOR
            string text = datFile.GetText();
            string nameEnglish = datFile.GetNameEnglish();
            string folderName = datFile.GetFolderName();

            string unturnedSandBox = Path.Combine(UnturnedModsGlobalSettingsObject.Instance.unturnedInstallationPath, "Sandbox", "Bundles");

            string destinationFolder = Path.Combine(unturnedSandBox, GetModFolder(AssetDatabase.GetAssetPath(datFile)));
            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }
            File.WriteAllText(Path.Combine(destinationFolder, folderName + ".dat"), text);
            File.WriteAllText(Path.Combine(destinationFolder, "English.dat"), "Name " + nameEnglish);
            #endif
        }

        public string GenerateModFolder()
        {
            string modDirectory = GetModDirectory();        
            if (Directory.Exists(modDirectory))
            {
                return modDirectory;
            }
            else
            {
                Directory.CreateDirectory(modDirectory);
            }
            return modDirectory;
        }

        public string GetSandboxDirectory()
        {
            return Path.Combine(GetUnturnedInstallationDirectory(), "Sandbox");
        }
        public string GetModDirectory()
        {
            string modDirectory = Path.Combine(GetSandboxDirectory(), "Bundles", modName.Trim());   
            return modDirectory;
        }


        public void GenerateMasterBundleEntry()
        {
            string modDirectory = GetModDirectory();
            Debug.Log(modDirectory);
            string assetBundleName = "Asset_Bundle_Name " + GetMasterBundleName();
            string assetPrefix = "Asset_Prefix " + Path.Combine("Assets", "UnturnedMods", modName.Trim());
            string assetBundleVersion = "Asset_Bundle_Version " + GetAssetBundleVersion().ToString();
            string data = assetBundleName + "\n" + assetPrefix + "\n" + assetBundleVersion;
            File.WriteAllText(Path.Combine(modDirectory, "MasterBundle.dat"), data);
            GenerateReadMe();
        }

        public void GenerateReadMe() {
            string text = "";
            text += "ID List:\n\n";
            string modDirectory = GetModDirectory();
            foreach (UnturnedDatFileScriptableObject datFile in datFiles)
            {
                string modRecord = datFile.id + " - " + datFile.nameEnglish.Trim() + "\n";
                text += modRecord;
            }
            text += "\n\nMod generated using PuggosWorldSDK:  tinyurl.com/2p9sy29r";
            File.WriteAllText(Path.Combine(modDirectory, "README.txt"), text);
        }

        public int GetAssetBundleVersion()
        {
            switch (assetBundleVersion)
            {
                case Asset_Bundle_Version.Asset_Bundle_Version_4:
                    return 4;
                case Asset_Bundle_Version.Asset_Bundle_Version_3:
                    return 3;
                case Asset_Bundle_Version.Asset_Bundle_Version_2:
                    return 2;
                case Asset_Bundle_Version.Asset_Bundle_Version_1:
                    return 1;
                default:
                    return 4;
            }
        }
        public string GetUnturnedInstallationDirectory()
        {
            return UnturnedModsGlobalSettingsObject.Instance.unturnedInstallationPath;
        }

        public string GetMasterBundleName() {
            return modName.ToLower() + ".masterbundle";
        }
    }
}