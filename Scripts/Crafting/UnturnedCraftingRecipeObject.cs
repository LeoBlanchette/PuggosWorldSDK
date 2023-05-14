using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LB3D.PuggosWorld.Unturned
{
    /// <summary>
    /// This is a crafting recipe object. It is required for automatic 
    /// crafting system output. This helps blueprint creation because 
    /// you can easily assign elements to the input and outputs, and 
    /// the system takes care of adding the directives to the .dat file
    /// when the mod is built.
    /// </summary>
    [CreateAssetMenu(fileName = "CraftingRecipe", menuName = "PuggosWorld/Unturned/Crafting Recipe", order = 4)]
    public class UnturnedCraftingRecipeObject : ScriptableObject
    {
        public enum BluePrintType
        {
            Repair,
            Tool,
            Apparel,
            Supply,
            Gear,
            Ammo,
            Barricade,
            Structure,
            Utilities,
            Furniture,
        }

        public enum BluePrintSkill
        {
            None,
            Craft,
            Tool,
            Repair,
        }
        public enum BluePrintSkillLevel
        {
            None,
            SkillLevel_1,
            SkillLevel_2,
            SkillLevel_3,
        }
        [Tooltip("This refers to the category of blueprint.")]
        public BluePrintType bluePrintType = BluePrintType.Repair;
        [Tooltip("Skill needed for blueprint.")]
        public BluePrintSkill blueprintSkill = BluePrintSkill.None;
        [Tooltip("Skill level needed for skill (based on skill selected above)")]
        public BluePrintSkillLevel bluePrintSkillLevel = BluePrintSkillLevel.None;

        [System.Serializable]
        public class Supply
        {
            public UnturnedDatFileScriptableObject item;
            public int amount = 1;
        }

        [System.Serializable]
        public class Output
        {
            public UnturnedDatFileScriptableObject item;
            public int amount = 1;
        }

        private string bluePrintNumberString = "";

        [Header("Tool Used")]
        [Tooltip("Leave blank if no tool is needed.")]
        public UnturnedDatFileScriptableObject tool;

        [Header("Blueprint Ingredients")]
        [Tooltip("These are the ingredients required to build the thing.")]
        public Supply[] ingredients;


        [Header("Blueprint Result")]        
        [Tooltip("This refers to build result. Such as crafting if supplies are available.")]
        public Output[] outputs;

        [Header("Blueprint Outputs / Biproducts")]
        [Tooltip("This refers to biproducts of the operation. Such as breaking down item.")]
        public Output product;

        [Header("Build Animation")]
        public int build = 27;

        [Header("Spawned on Deployment")]
        [Tooltip("This causes a spawn to occur when an object is placed. It is used, for instance, in placing the Makeshift Vehicle.")]
        public UnturnedDatFileScriptableObject explosion;

        [Header("Other Directives")]
        [Tooltip("These are other directives, such as you would find in a .dat file, such as PlacementAudioClip or other key / value entries.")]
        public UnturnedDatFileScriptableObject.KeyValEntry[] datFileValues;

        public string Newline()
        {
            return "\n";
        }


        /// <summary>
        /// Outputs the blueprint text in .dat file format. 
        /// </summary>
        /// <returns></returns>
        public string GetText(int blueprintNumber, int blueprintTotalNumber)
        {
            
            this.bluePrintNumberString = "Blueprint_" + blueprintNumber.ToString();
            string text = "";

            if (blueprintNumber == 0)
            {
                text += GetBluePrintTotalCount(blueprintTotalNumber);
            }
            text += GetBlueprintType();
            text += GetBlueprintTool();
            text += GetBluePrintSkill();
            text += GetBluePrintSuppliesCount();
            text += GetBlueprintSuppliesList();
            text += GetBlueprintProduct();
            text += GetBlueOutputsCount();
            text += GetBlueprintOutputsList();
            text += GetExplosionID();
            text += GetBuildAnimation();
            text += GetMiscValues();
            return text;
        }

        public string GetBluePrintTotalCount(int blueprintTotalNumber) {
            return "Blueprints " + blueprintTotalNumber.ToString() + Newline();
        }

        /// <summary>
        /// Gives the total expected count of ingredients / supplies.
        /// </summary>
        /// <returns></returns>
        public string GetBluePrintSuppliesCount()
        {
            if (ingredients == null) return "";
            return bluePrintNumberString + "_Supplies" + " " + ingredients.Length+Newline();
        }
        /// <summary>
        /// Provides the ingredient list text.
        /// </summary>
        /// <returns></returns>
        public string GetBlueprintSuppliesList() {
            string text = "";
            int count = 0;
            foreach (Supply ingredient in ingredients)
            {
                text += bluePrintNumberString + "_Supply_" + count + "_ID " + ingredient.item.id + Newline();
                text += bluePrintNumberString + "_Supply_" + count + "_Amount " + ingredient.amount + Newline();
                count++;
            }
            return text;
        }

        /// <summary>
        /// Gives the total expected count of ingredients / supplies.
        /// </summary>
        /// <returns></returns>
        public string GetBlueOutputsCount()
        {
            if (outputs == null) return "";
            if (outputs.Length == 0) return "";
            return bluePrintNumberString + "_Outputs" + " " + outputs.Length + Newline();
        }

        public string GetBlueprintOutputsList()
        {
            string text = "";
            int count = 0;
            foreach (Output ingredient in outputs)
            {
                text += bluePrintNumberString + "_Output_" + count + "_ID " + ingredient.item.id + Newline();
                text += bluePrintNumberString + "_Output_" + count + "_Amount " + ingredient.amount + Newline();
                count++;
            }
            return text;
        }

        /// <summary>
        /// Generates the products of scrapping n' stuff.
        /// </summary>
        /// <returns></returns>
        public string GetBlueprintProduct()
        {            
            if (product.item == null) return "";

            string text = "";
            if (product == null) return text;
            text += bluePrintNumberString + "_Product " + product.item.id + Newline();
            text += bluePrintNumberString + "_Products " + product.amount + Newline();
            return text;
        }

        public string GetExplosionID()
        {
            if (explosion == null) return "";
            if (explosion.id < 1) return "";             
            string text = "";
            text += "Explosion " + explosion.id.ToString() + Newline();
            return text;
        }


        /// <summary>
        /// Gives the blueprint type
        /// </summary>
        /// <returns></returns>
        public string GetBlueprintType() {
            string text = "";
            text += bluePrintNumberString + "_Type " + bluePrintType.ToString() + Newline();
            return text;
        }
        /// <summary>
        /// Gives the blueprint tool
        /// </summary>
        /// <returns></returns>
        public string GetBlueprintTool()
        {
            string text = "";
            if (tool == null) return text;
            text += bluePrintNumberString + "_Tool " + tool.id + Newline();
            return text;
        }

        public string GetBluePrintSkill() {
            string text = "";
            if (GetSkillLevelInt() < 1) return text;
            if (blueprintSkill == BluePrintSkill.None) return text;
            text += bluePrintNumberString + "_Level " + GetSkillLevelInt().ToString() + Newline();
            text += bluePrintNumberString + "_Skill " + blueprintSkill.ToString() + Newline();
            return text;
        }
        public int GetSkillLevelInt() {
            switch (bluePrintSkillLevel)
            {
                case BluePrintSkillLevel.None:
                    return 0;
                case BluePrintSkillLevel.SkillLevel_1:
                    return 1;
                case BluePrintSkillLevel.SkillLevel_2:
                    return 2;
                case BluePrintSkillLevel.SkillLevel_3:
                    return 3;
                default:
                    return 0;                   
            }
        }

        public string GetBuildAnimation() {
            return bluePrintNumberString + "_Build " + build.ToString() + Newline();            
        }

        public string GetMiscValues() {
            string text = "";

            foreach (UnturnedDatFileScriptableObject.KeyValEntry keyValEntry in datFileValues)
            {
                string entry = keyValEntry.key.Trim() + " " + keyValEntry.val.Trim() + Newline();
                text += entry;
            }

            return text;
        }
    }
}