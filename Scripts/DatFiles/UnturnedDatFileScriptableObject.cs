using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
namespace LB3D.PuggosWorld.Unturned
{
    [CreateAssetMenu(fileName = "DatFile", menuName = "PuggosWorld/Unturned/Generic Data File", order = 2)]
    public class UnturnedDatFileScriptableObject : ScriptableObject
    {
        public enum Rarity
        {
            Common,
            Uncommon,
            Rare,
            Epic,
            Legendary,
            Mythical
        }
        public enum ModType
        {
            None,
            Animal,
            Arrest_End,
            Arrest_Start,
            Backpack,
            Barrel,
            Barricade,
            Beacon,
            Charge,
            Cloud,
            Compass,
            Decal,
            Detonator,
            Dialogue,
            Effect,
            Farm,
            Filter,
            Fisher,
            Food,
            Fuel,
            Generator,
            Glasses,
            Grip,
            Grower,
            Gun,
            Hat,
            Item,
            Large,
            Library,
            Magazine,
            Map,
            Mask,
            Medical,
            Medium,
            Melee,
            Mythic,            
            NPC,
            Oil_Pump,
            Optic,
            Pants,
            Quest,
            Refill,
            Resource,
            Sentry,
            Shirt,
            Sight,
            Skin,
            Small,
            Spawn,
            Storage,
            Structure,
            Supply,
            Tactical,
            Tank,
            Throwable,
            Tire,
            Tool,
            Trap,
            Vehicle,
            Vehicle_Repair_Tool,
            Vendor,
            Vest,
            Water
        }
        [SerializeField]
        public string guid;
        
        [HideInInspector]
        [SerializeField]
        public string coreMasterBundlePath = "";

        public ModType modType;
        [Header("Rarity")]
        [Tooltip("Only items and vehicles require rarity.")]
        public bool hasRarity = false;
        public Rarity rarity;
        [Header("ID")]
        [Tooltip("Many asset types only require a GUID to work.")]
        public bool hasId = true;
        public int id;
        [Tooltip("Protect ID from being changed.")]
        public bool idLock = false;
        [Header("Localization")]
        public string nameEnglish;
        [Tooltip("Only items need descriptions.")]
        public bool hasDescription = false;
        public string descriptionEnglish;
        public UnturnedDatFileScriptableObject cloneFrom;

        private string[] newlines = new string[] { "\r\n", "\r", "\n" };

        [System.Serializable]
        public class KeyValEntry
        {
            public string key;
            public string val;
        }

        public List<KeyValEntry> datFileValues = new List<KeyValEntry>();

        public List<UnturnedCraftingRecipeObject> craftingRecipies = new List<UnturnedCraftingRecipeObject>();

        public List<UnturnedVehicleConfigurationObject> vehicleConfiguration = new List<UnturnedVehicleConfigurationObject>();

        private void Awake()
        {
            GenerateGuid();
        }

        public string CleanText(string text)
        {

            string newText = "";
            string[] lines = text.Trim().Split(newlines, StringSplitOptions.None);

            foreach (string line in lines)
            {
                string cleanedLine = line.Trim();
                string[] cleanedValues = cleanedLine.Split();
                List<string> cleanedValuesList = new List<string>();
                foreach (string cleanedValue in cleanedValues)
                {
                    if (string.IsNullOrEmpty(cleanedValue)) continue;
                    cleanedValuesList.Add(cleanedValue);
                }
                string correctedLine = String.Join(" ", cleanedValuesList.ToArray());
                newText += correctedLine + "\n";
            }
            return newText;
        }

        public void CloneFromDatFileObject(bool masterBundleOverRide = false) {
            if (cloneFrom == null) {
                Debug.LogWarning("Please fill in the Clone From field above to clone a dat file.");
            }
            datFileValues.Clear();
            id = 0;
            modType = ModType.None;
            rarity = Rarity.Common; 
            nameEnglish = null;
            descriptionEnglish = null;

            id = cloneFrom.id;
            modType = cloneFrom.modType;
            rarity = cloneFrom.rarity; 
            nameEnglish = cloneFrom.nameEnglish;

            foreach (KeyValEntry keyValEntry in cloneFrom.datFileValues)
            {
                if (!SkipKey(keyValEntry.key)) {
                    datFileValues.Add(keyValEntry);
                }
            }
            if (masterBundleOverRide) {
                KeyValEntry masterBundleOverRideDirective = new KeyValEntry();
                masterBundleOverRideDirective.key = "Master_Bundle_Override";
                masterBundleOverRideDirective.val = "core.masterbundle";

                KeyValEntry masterBundleOverRidePath = new KeyValEntry();
                masterBundleOverRidePath.key = "Bundle_Override_Path";
                masterBundleOverRidePath.val = cloneFrom.coreMasterBundlePath.TrimEnd(new Char[] { '\\', '/' });

                KeyValEntry masterBundleOverRideId = new KeyValEntry();
                masterBundleOverRideId.key = "Shared_Skin_Lookup_ID";
                masterBundleOverRideId.val = cloneFrom.id.ToString();

                datFileValues.Add(masterBundleOverRideDirective);
                datFileValues.Add(masterBundleOverRidePath);
                datFileValues.Add(masterBundleOverRideId);

            }
            cloneFrom = null;
        }

        public void SetCoreMasterBundlePath(string path) {
            coreMasterBundlePath = path.Trim();
        }

        public void AddFromTextFile(string datFile = null)
        {
            string readyText = "";

            if (datFile == null)
            {
                Debug.LogWarning(".dat file null. Nothing done...");
                return;
            }
            else
            {
                readyText = CleanText(File.ReadAllText(datFile));
            }

            string[] lines = readyText.Split(newlines, StringSplitOptions.None);
            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;
                string[] parts = line.Split();

                KeyValEntry keyValEntry = new KeyValEntry();
                if (parts.Length > 0)
                {
                    keyValEntry.key = parts[0];
                }
                else
                {
                    continue;
                }
                if (parts.Length > 1)
                {
                    keyValEntry.val = parts[1];
                }
                ProcessSpecialValues(keyValEntry);
                if (!SkipKey(keyValEntry.key))
                {
                    datFileValues.Add(keyValEntry);
                }
            }            
        }
        
        public bool SkipKey(string key) {
            if (key == null) return false;
            key = key.ToLower();
            if (key == "id") return true;
            if (key == "guid") return true;
            if (key == "type") return true;
            if (key == "rarity") return true;
            if (key == "descriptionEnglish") return true;
            return false;
        }

        public void ProcessSpecialValues(KeyValEntry keyValEntry)
        {
            if (keyValEntry.key.Trim().ToLower() == "id")
            {
                int.TryParse(keyValEntry.val, out id);
            }
            if (keyValEntry.key.Trim().ToLower() == "rarity")
            {
                rarity = (Rarity)System.Enum.Parse(typeof(Rarity), keyValEntry.val);
            }
            if (keyValEntry.key.Trim().ToLower() == "type")
            {
                try
                {
                    modType = (ModType)System.Enum.Parse(typeof(ModType), keyValEntry.val);

                }
                catch {
                    Debug.LogWarning(keyValEntry.val + " does not exist in ModTypes. You will have to manually assign this ID: " + id.ToString());
                }
            }

        }

        public void Clear()
        {
            datFileValues.Clear();
            id = 0;
            modType = ModType.None;
            rarity = Rarity.Common;
            nameEnglish = null;
            descriptionEnglish = null;
        }

        public void LockId(bool lockId = true) {
            idLock = lockId;
        }

        public bool IsIdLocked() {
            return idLock;
        }

        public void Test()
        {  
            Debug.Log(GetText());
        }

        public string GetText()
        {
            string datText = "";
            datText += "GUID " + GetGuid() + "\n";
            datText += "Type " + modType.ToString() + "\n";
            //Added option to disable rarity as it is only required for items and vehicles.
            if (hasRarity) {
            datText += "Rarity " + rarity.ToString() + "\n";
            }
            //Added option to disable ID as it is not required for assets like objects and vehicles.
            if (hasId) {
            datText += "ID " + id.ToString() + "\n";
            }

            foreach (KeyValEntry keyValEntry in datFileValues)
            {
                if (keyValEntry.key.ToLower() == "type") continue;
                if (keyValEntry.key.ToLower() == "rarity") continue;
                if (keyValEntry.key.ToLower() == "guid") continue;
                if (keyValEntry.key.ToLower() == "id") continue;
                
                if (keyValEntry.key.ToLower() == "speed_min") datText += "\n";
                if (keyValEntry.key.ToLower() == "steer_min") datText += "\n";
                if (keyValEntry.key.ToLower() == "brake") datText += "\n";
                if (keyValEntry.key.ToLower() == "fuel_min") datText += "\n";
                if (keyValEntry.key.ToLower() == "health_min") datText += "\n";
                if (keyValEntry.key.ToLower() == "trunk_storage_x") datText += "\n";
                if (keyValEntry.key.ToLower() == "explosion") datText += "\n";

                datText += keyValEntry.key + " " + keyValEntry.val + "\n";
            }

            datText += GetBlueprintsText();

            datText += GetVehicleConfigurationsText();

            return datText;
        }


        /// <summary>
        /// If you read the method, it will tell you what it does. 
        /// I don't know why I write comments. Maybe its because 
        /// writing can be fun some times. 
        /// </summary>
        public string GetBlueprintsText() {
            if (craftingRecipies == null) return "";
            if (craftingRecipies.Count == 0) return "";
            string text = "\n";
            int i = 0;
            foreach (UnturnedCraftingRecipeObject craftingRecipe in craftingRecipies)
            {
                string recipeText = craftingRecipe.GetText(i, craftingRecipies.Count) +"\n";
                text += recipeText;
                i++;
            }
            return text;
        }

        public string GetVehicleConfigurationsText() {
            if (vehicleConfiguration == null || vehicleConfiguration.Count == 0) return "";

            string text = "\n";
            foreach (UnturnedVehicleConfigurationObject vehicleConfig in vehicleConfiguration)
            {
                text += vehicleConfig.GetConfigurationText() + "\n";
            }

            return text;
        }

        public string GetNameEnglish()
        {
            return nameEnglish;
        }

        public string GetDescriptionEnglish()
        {
            return descriptionEnglish;
        }

        public string GetFolderName()
        {
            return this.name.Trim();
        }

        public void GenerateGuid(bool regenerate = false) {            
            if (string.IsNullOrEmpty(guid) || regenerate) { 
                guid = Guid.NewGuid().ToString("N");                
            }
            if (regenerate) {
                Debug.Log("Guid Regenerated. Now: " + guid);
            }
        }

        public string GetGuid() {
            GenerateGuid();
            return guid;
        }

    }
}