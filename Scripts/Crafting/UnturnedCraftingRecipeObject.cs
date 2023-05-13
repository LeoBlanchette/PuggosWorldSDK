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
        public BluePrintType bluePrintType = BluePrintType.Repair;
        public BluePrintSkill blueprintSkill = BluePrintSkill.None;
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

        private string bluePrintNumber = "";

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
        public string GetText(int blueprintNumber)
        {
            this.bluePrintNumber = "Blueprint_" + blueprintNumber.ToString();
            string text = "";

            text += GetBlueprintType();
            text += GetBlueprintTool();
            text += GetBluePrintSkill();
            text += GetBluePrintSuppliesCount();
            text += GetBlueprintSuppliesList();
            text += GetBlueprintProductList();
            text += GetBlueOutputsCount();
            text += GetBlueprintOutputsList();

            return text;
        }
        /// <summary>
        /// Gives the total expected count of ingredients / supplies.
        /// </summary>
        /// <returns></returns>
        public string GetBluePrintSuppliesCount()
        {
            if (ingredients == null) return "";
            return bluePrintNumber + "_Supplies" + " " + ingredients.Length+Newline();
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
                text += bluePrintNumber + "_Supply_" + count + "_ID " + ingredient.item.id + Newline();
                text += bluePrintNumber + "_Supply_" + count + "_Amount " + ingredient.amount + Newline();
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
            return bluePrintNumber + "_Outputs" + " " + outputs.Length + Newline();
        }

        public string GetBlueprintOutputsList()
        {
            string text = "";
            int count = 0;
            foreach (Supply ingredient in ingredients)
            {
                text += bluePrintNumber + "_Output_" + count + "_ID " + ingredient.item.id + Newline();
                text += bluePrintNumber + "_Output_" + count + "_Amount " + ingredient.amount + Newline();
                count++;
            }
            return text;
        }

        /// <summary>
        /// Generates the products of scrapping n' stuff.
        /// </summary>
        /// <returns></returns>
        public string GetBlueprintProductList()
        {
            string text = "";
            if (product == null) return text;
            text += bluePrintNumber + "_Product " + product.item.id + Newline();
            text += bluePrintNumber + "_Products " + product.amount + Newline();
            return text;
        }

        /// <summary>
        /// Gives the blueprint type
        /// </summary>
        /// <returns></returns>
        public string GetBlueprintType() {
            string text = "";
            text += bluePrintNumber + "_Type " + bluePrintType.ToString() + Newline();
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
            text += bluePrintNumber + "_Tool " + tool.id + Newline();
            return text;
        }

        public string GetBluePrintSkill() {
            string text = "";           
            text += bluePrintNumber + "_Level " + GetSkillLevelInt().ToString() + Newline();
            text += bluePrintNumber + "_Skill " + blueprintSkill.ToString() + Newline();
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
    }
}