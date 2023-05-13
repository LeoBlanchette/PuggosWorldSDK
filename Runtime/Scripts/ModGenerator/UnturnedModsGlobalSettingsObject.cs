using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
namespace LB3D.PuggosWorld.Unturned
{
    [CreateAssetMenu(fileName = "ModGlobalSettings", menuName = "PuggosWorld/Unturned/ModGlobalSettings", order = 20)]
    public class UnturnedModsGlobalSettingsObject : SingletonScriptableObject<UnturnedModsGlobalSettingsObject>
    {
        [Header("Unturned Path")]
        [ReadOnly]
        [SerializeField]
        public string unturnedInstallationPath = null;
  
        public void SetGlobalUnturnedInstallationDirectoryPath(string path)
        {
            if (!IsUnturnedDirectoryValid(path))
            {
                Debug.LogError("Not a valid Unturned directory: " + path);
                return;
            }
            Debug.LogWarning("Unturned installation directory set to: " + path);
            unturnedInstallationPath = path;            
        }
         
        public void GenerateDatFileDataBase()
        {
            if (unturnedInstallationPath == null)
            {
                Debug.LogWarning("Please set the path to the Unturned installation directory.");
                return;
            };
            if (!IsUnturnedDirectoryValid(unturnedInstallationPath))
            {
                Debug.LogWarning("Please set the path to the Unturned installation directory.");
                return;
            }
        
            string[] datFiles = Directory.GetFiles(Path.Combine(unturnedInstallationPath, "Bundles"), "*.dat", SearchOption.AllDirectories);
            foreach (string datFile in datFiles)
            {
                CreateDatFileDatabaseEntry(datFile);
            }

            #if UNITY_EDITOR
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            #endif
        }
        public void CreateDatFileDatabaseEntry(string datFilePath) {
            if (!IsDatFileValid(datFilePath)) return;
            
            //Path to database in our PuggosWorldSDK folder in assets.
            string databasePath = GetDatabasePath();
            
            //The filename in question (a particlar dat file, without the path included)
            string fileName = Path.GetFileName(datFilePath);
            
            //The extended path beneath Database folder. Example: Database/Vehicles/SomeVehicle/
            string extendedDatabasePath = datFilePath.Replace(Path.Combine(unturnedInstallationPath, "Bundles"), "").Replace(fileName, "");
            extendedDatabasePath = extendedDatabasePath.TrimStart(new Char[] { '\\', '/' });
            extendedDatabasePath = extendedDatabasePath.TrimEnd(new Char[] { '\\', '/' });
            //Create the scriptable object 
            UnturnedDatFileScriptableObject datFileObject = ScriptableObject.CreateInstance<UnturnedDatFileScriptableObject>();
            datFileObject.AddFromTextFile(datFilePath);
            string localDatabasePath = Path.Combine(GetDatabasePathLocal());
            string extendedPath = Path.Combine(localDatabasePath, extendedDatabasePath);
            string assetFileName = fileName.Replace(".dat", ".asset");
            string assetDestination = Path.Combine(extendedPath, assetFileName);
            datFileObject.nameEnglish = GetNameEnglish(datFilePath);
            
            Directory.CreateDirectory(Path.Combine(GetDatabasePath(), extendedDatabasePath));
            
            #if UNITY_EDITOR
            AssetDatabase.CreateAsset(datFileObject, Path.Combine("Assets", assetDestination));
            #endif
        }


        public string GetNameEnglish(string datFilePath) {
            string dir = Path.GetDirectoryName(datFilePath);
            string nameEnglishFile = Path.Combine(dir, "English.dat");
            if (!File.Exists(Path.Combine(dir, "English.dat"))) return null;
            string textInDatFile = File.ReadAllText(nameEnglishFile);
            if (textInDatFile.StartsWith("Name ")) {
                textInDatFile = textInDatFile.Replace("Name ", "");
            }
            return textInDatFile;
        }

        public bool IsDatFileValid(string datFilePath) {
            //First, see if an English.dat file exists.
            string dir = Path.GetDirectoryName(datFilePath);
            if (!File.Exists(Path.Combine(dir, "English.dat"))) return false;
            //Next, see if file contains an ID ...
            string textInDatFile = File.ReadAllText(datFilePath);
            if (!textInDatFile.Contains("ID ")) return false;
            return true;
        }

        public bool IsUnturnedDirectoryValid(string path)
        {
            if (File.Exists(Path.Combine(path, "Unturned.exe")))
            {
                return true;
            }
            return false;
        }
        string GetDatabasePath() {
            string databasePath = Path.Combine(Application.dataPath, "PuggosWorldSDK", "Unturned", "Database");
            if (!Directory.Exists(databasePath)) {
                Directory.CreateDirectory(databasePath);
            }
            return databasePath;
        }
        string GetDatabasePathLocal() {
            return Path.Combine("PuggosWorldSDK", "Unturned",  "Database");
        }
    }
}
