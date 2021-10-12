using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Skill))]
[CanEditMultipleObjects]
public class SkillEditor : Editor {
    public override void OnInspectorGUI() {
        // Draw basic info
        EditorGUILayout.PropertyField(serializedObject.FindProperty("icon"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillDescription"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("costSP"));

        // Draw weapon requirement info
        SerializedProperty requiresWeapon = serializedObject.FindProperty("requiresWeaponType");
        EditorGUILayout.PropertyField(requiresWeapon);

        if (requiresWeapon.boolValue)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("requiredAtkType"));

        // Draw skill effect info
        SerializedProperty scriptedSkillEff = serializedObject.FindProperty("useScriptedSkillEffects");
        EditorGUILayout.PropertyField(scriptedSkillEff);

        if (scriptedSkillEff.boolValue)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("scriptedSkillEffect"));
        else {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("skillSFX"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("skillAnimation"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("targetType"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("unitChpMultiplier"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("unitMhpMultiplier"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("unitCspMultiplier"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("unitMspMultiplier"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("unitStrMultiplier"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("unitMagMultiplier"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("unitDexMultiplier"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("unitDefMultiplier"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("unitResMultiplier"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("unitArmMultiplier"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("unitAgiMultiplier"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("unitMtMultiplier"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyChpMultiplier"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyMhpMultiplier"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyCspMultiplier"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyMspMultiplier"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyStrMultiplier"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyMagMultiplier"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyDexMultiplier"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyDefMultiplier"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyResMultiplier"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyArmMultiplier"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyAgiMultiplier"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyMtMultiplier"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("flatValue"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
