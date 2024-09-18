using UnityEditor;
using UnityEngine;
using LB3D.PuggosWorld.Unturned;

[CustomEditor(typeof(UnturnedVehicleConfigurationObject))]
public class UnturnedVehicleConfigurationObjectEditor : Editor
{
    // Serialized properties for the fields
    SerializedProperty useVehiclePaint;
    SerializedProperty paintableSection;
    SerializedProperty defaultPaintColorMode;
    SerializedProperty defaultPaintColors;
    SerializedProperty defaultPaintColorsRandom;

    SerializedProperty useWheelConfigurations;
    SerializedProperty wheelConfigurations;

    SerializedProperty useGears;
    SerializedProperty reverseGearRatio;
    SerializedProperty forwardGearRatios;
    SerializedProperty GearShiftDuration;
    SerializedProperty GearShiftInterval;
    SerializedProperty GearShiftUpThresholdRPM;
    SerializedProperty EngineIdleRPM;
    SerializedProperty EngineMaxRPM;
    SerializedProperty EngineMaxTorque;

    SerializedProperty useEngineSound;
    SerializedProperty engineSoundType;
    SerializedProperty engineSound;

    void OnEnable()
    {
        // Link properties to serialized fields
        useVehiclePaint = serializedObject.FindProperty("useVehiclePaint");
        paintableSection = serializedObject.FindProperty("paintableSection");
        defaultPaintColorMode = serializedObject.FindProperty("defaultPaintColorMode");
        defaultPaintColors = serializedObject.FindProperty("defaultPaintColors");
        defaultPaintColorsRandom = serializedObject.FindProperty("defaultPaintColorsRandom");

        useWheelConfigurations = serializedObject.FindProperty("useWheelConfigurations");
        wheelConfigurations = serializedObject.FindProperty("wheelConfigurations");

        useGears = serializedObject.FindProperty("useGears");
        reverseGearRatio = serializedObject.FindProperty("reverseGearRatio");
        forwardGearRatios = serializedObject.FindProperty("forwardGearRatios");
        GearShiftDuration = serializedObject.FindProperty("GearShiftDuration");
        GearShiftInterval = serializedObject.FindProperty("GearShiftInterval");
        GearShiftUpThresholdRPM = serializedObject.FindProperty("GearShiftUpThresholdRPM");
        EngineIdleRPM = serializedObject.FindProperty("EngineIdleRPM");
        EngineMaxRPM = serializedObject.FindProperty("EngineMaxRPM");
        EngineMaxTorque = serializedObject.FindProperty("EngineMaxTorque");

        useEngineSound = serializedObject.FindProperty("useEngineSound");
        engineSoundType = serializedObject.FindProperty("engineSoundType");
        engineSound = serializedObject.FindProperty("engineSound");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update(); // Start the update for serialized properties

        // Vehicle Paint Section
        EditorGUILayout.PropertyField(useVehiclePaint, new GUIContent("Use Vehicle Paint"));
        if (useVehiclePaint.boolValue)
        {
            EditorGUILayout.PropertyField(paintableSection, new GUIContent("Paintable Sections"), true);

            EditorGUILayout.PropertyField(defaultPaintColorMode, new GUIContent("Default Paint Color Mode"));

            // Show different properties based on the selected paint color mode
            if ((UnturnedVehicleConfigurationObject.DefaultPaintColorMode)defaultPaintColorMode.enumValueIndex == UnturnedVehicleConfigurationObject.DefaultPaintColorMode.List)
            {
                EditorGUILayout.PropertyField(defaultPaintColors, new GUIContent("Default Paint Colors"), true);
            }
            else if ((UnturnedVehicleConfigurationObject.DefaultPaintColorMode)defaultPaintColorMode.enumValueIndex == UnturnedVehicleConfigurationObject.DefaultPaintColorMode.RandomHueOrGrayscale)
            {
                EditorGUILayout.PropertyField(defaultPaintColorsRandom, new GUIContent("Default Paint Colors (Random)"));
            }
        }

        // Wheel Configuration Section
        EditorGUILayout.PropertyField(useWheelConfigurations, new GUIContent("Use Wheel Configurations"));
        if (useWheelConfigurations.boolValue)
        {
            EditorGUILayout.PropertyField(wheelConfigurations, new GUIContent("Wheel Configurations"), true);
        }

        // Gears Section
        EditorGUILayout.PropertyField(useGears, new GUIContent("Use Gears"));
        if (useGears.boolValue)
        {
            EditorGUILayout.PropertyField(reverseGearRatio, new GUIContent("Reverse Gear Ratio"));
            EditorGUILayout.PropertyField(forwardGearRatios, new GUIContent("Forward Gear Ratios"), true);
            EditorGUILayout.PropertyField(GearShiftDuration, new GUIContent("Gear Shift Duration"));
            EditorGUILayout.PropertyField(GearShiftInterval, new GUIContent("Gear Shift Interval"));
            EditorGUILayout.PropertyField(GearShiftUpThresholdRPM, new GUIContent("Gear Shift Up Threshold RPM"));
            EditorGUILayout.PropertyField(EngineIdleRPM, new GUIContent("Engine Idle RPM"));
            EditorGUILayout.PropertyField(EngineMaxRPM, new GUIContent("Engine Max RPM"));
            EditorGUILayout.PropertyField(EngineMaxTorque, new GUIContent("Engine Max Torque"));
        }

        // Engine Sound Section
        EditorGUILayout.PropertyField(useEngineSound, new GUIContent("Use Engine Sound"));
        if (useEngineSound.boolValue)
        {
            EditorGUILayout.PropertyField(engineSoundType, new GUIContent("Engine Sound Type"));
            EditorGUILayout.PropertyField(engineSound, new GUIContent("Engine Sounds"), true);
        }

        // Apply changes to serialized properties
        serializedObject.ApplyModifiedProperties();
    }
}
