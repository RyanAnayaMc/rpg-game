using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Consumable))]
[CanEditMultipleObjects]
public class ConsumableEditor : Editor {
    public override void OnInspectorGUI() {
        serializedObject.Update();

        // Draw Item info
        EditorGUILayout.LabelField("Item Properties");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemIcon"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemDescription"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("isSellable"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("buyPrice"));

        // Draw Consumable info
        SerializedProperty type = serializedObject.FindProperty("type");
        EditorGUILayout.PropertyField(type);

        ConsumableType consType = (ConsumableType)type.enumValueIndex;

        if (consType == ConsumableType.HealthRecover || consType == ConsumableType.SpecialRecover)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("recovery"));
        else if (consType == ConsumableType.StatBuff || consType == ConsumableType.DebuffInflict) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("stat"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("statChange"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("statMultiplier"));
        } else if (consType == ConsumableType.DamageDeal)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("damage"));
        else
            EditorGUILayout.PropertyField(serializedObject.FindProperty("scriptedSkillEffect"));


        if (consType != ConsumableType.Scripted) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("animation"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("soundEffect"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
