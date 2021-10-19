using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AccessoryEditor : Editor {
	public override void OnInspectorGUI() {
        serializedObject.Update();

        // Draw Item info
        EditorGUILayout.LabelField("Item Properties");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemIcon"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemDescription"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("isSellable"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("buyPrice"));

        // Draw apparel info
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxHPChange"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxSPChange"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("atkChange"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("magChange"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("dexChange"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("defChange"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("resChange"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("armChange"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("agiChange"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("effect"));

        AccessoryEffect effect = (AccessoryEffect) serializedObject.FindProperty("effect").enumValueIndex;
		if (effect != AccessoryEffect.None && effect != AccessoryEffect.ExtraTurn)
			EditorGUILayout.PropertyField(serializedObject.FindProperty("effectParameter"));


		serializedObject.ApplyModifiedProperties();
    }
}
