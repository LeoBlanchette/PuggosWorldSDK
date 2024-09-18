using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LB3D.PuggosWorld.Unturned
{
    [CreateAssetMenu(fileName = "VehicleConfiguration", menuName = "PuggosWorld/Unturned/Vehicle Configuration", order = 4)]
    public class UnturnedVehicleConfigurationObject : ScriptableObject
    {
        [System.Serializable]
        public class PaintableSections
        {
            public string path;
            public int materialIndex;
        }

        [System.Serializable]
        public class DefaultPaintColors
        {
            public string paintColor;
            public string paintColorComment;
        }

        [System.Serializable]
        public class DefaultPaintColorsRandom
        {
            public float minSaturation;
            public float maxSaturation;
            public float minValue;
            public float maxValue;
            public float grayscaleChance;
        }

        public enum DefaultPaintColorMode
        {
            List,
            RandomHueOrGrayscale
        }

        public enum EngineSoundType
        {
            Legacy,
            EngineRPMSimple
        }

        [System.Serializable]
        public class WheelConfigurations
        {
            public string wheelColliderPath;
            public bool isColliderSteered;
            public bool IsColliderPowered;
            public string ModelPath;
            public bool ModelUseColliderPose;
        }

        [System.Serializable]
        public class EngineSound
        {
            public float IdlePitch;
            public float IdleVolume;
            public float MaxPitch;
            public float MaxVolume;
        }

        // Properties
        public bool useVehiclePaint = false;
        public PaintableSections[] paintableSection;
        public DefaultPaintColorMode defaultPaintColorMode = DefaultPaintColorMode.List;
        public DefaultPaintColors[] defaultPaintColors;
        public DefaultPaintColorsRandom defaultPaintColorsRandom;

        public bool useWheelConfigurations = false;
        public WheelConfigurations[] wheelConfigurations;

        [Header("Gears")]
        public bool useGears = false;
        public int reverseGearRatio;
        public float[] forwardGearRatios;
        public float GearShiftDuration;
        public float GearShiftInterval;
        public int GearShiftUpThresholdRPM;
        public int EngineIdleRPM;
        public int EngineMaxRPM;
        public float EngineMaxTorque;

        [Header("Engine Sound")]
        public bool useEngineSound = false;
        public EngineSoundType engineSoundType = EngineSoundType.EngineRPMSimple;
        public EngineSound[] engineSound;

        public string Newline()
        {
            return "\n";
        }

        public string GetPaintableSections()
        {
            if (paintableSection == null || paintableSection.Length == 0 || !useVehiclePaint) return "";
            
            string text = "PaintableSections" + Newline() + "[" + Newline();
            
            foreach (var section in paintableSection)
            {
                text += "\t{" + Newline();
                text += $"\t\tPath {section.path}" + Newline();
                text += $"\t\tMaterialIndex {section.materialIndex}" + Newline();
                text += "\t}" + Newline();
            }
            
            text += "]" + Newline();
            return text;
        }

        public string GetDefaultPaintColors()
        {
            if (defaultPaintColors == null || defaultPaintColors.Length == 0 || !useVehiclePaint) return "";
            string text = "";
            if  (defaultPaintColorMode == DefaultPaintColorMode.List) {
                text = "DefaultPaintColors" + Newline() + "[" + Newline();
                foreach (var color in defaultPaintColors)
                {
                    text += $"\t\"{color.paintColor}\" // {color.paintColorComment}" + Newline();
                }
                text += "]" + Newline();
            }
            if  (defaultPaintColorMode == DefaultPaintColorMode.RandomHueOrGrayscale) {
                text = "DefaultPaintColor_Configuration" + Newline() + "[" + Newline();

                text += $"\tMinSaturation {defaultPaintColorsRandom.minSaturation}" + Newline();
                text += $"\tMaxSaturation {defaultPaintColorsRandom.maxSaturation}" + Newline();
                text += $"\tMinValue {defaultPaintColorsRandom.minValue}" + Newline();
                text += $"\tMaxValue {defaultPaintColorsRandom.maxValue}" + Newline();
                text += $"\tGrayscaleChance {defaultPaintColorsRandom.grayscaleChance}" + Newline();


                text += "]" + Newline();
            }
            return text;
        }

        public string GetWheelConfigurations()
        {
            if (wheelConfigurations == null || wheelConfigurations.Length == 0 || !useWheelConfigurations) return "";
            string text = "WheelConfigurations" + Newline() + "[" + Newline();
            foreach (var config in wheelConfigurations)
            {
                text += "\t{" + Newline();
                text += $"\t\tWheelColliderPath {config.wheelColliderPath}" + Newline();
                text += $"\t\tIsColliderSteered {config.isColliderSteered.ToString().ToLower()}" + Newline();
                text += $"\t\tIsColliderPowered {config.IsColliderPowered.ToString().ToLower()}" + Newline();
                text += $"\t\tModelPath {config.ModelPath}" + Newline();
                text += $"\t\tModelUseColliderPose {config.ModelUseColliderPose.ToString().ToLower()}" + Newline();
                text += "\t}" + Newline();
            }
            text += "]" + Newline();
            return text;
        }

        public string GetEngineSound()
        {
            if (engineSound == null || engineSound.Length == 0 || !useEngineSound) return "";
            string text = "EngineSound" + Newline() + "[" + Newline();
            foreach (var sound in engineSound)
            {
                text += "\t{" + Newline();
                text += $"\t\tIdlePitch {sound.IdlePitch}" + Newline();
                text += $"\t\tIdleVolume {sound.IdleVolume}" + Newline();
                text += $"\t\tMaxPitch {sound.MaxPitch}" + Newline();
                text += $"\t\tMaxVolume {sound.MaxVolume}" + Newline();
                text += "\t}" + Newline();
            }
            text += "]" + Newline();
            return text;
        }

        public string GetConfigurationText()
        {
            string text = "";
            text += GetPaintableSections();
            text += GetDefaultPaintColors();
            text += GetWheelConfigurations();
            text += GetEngineSound();
            return text;
        }
    }
}
