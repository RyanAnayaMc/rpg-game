using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#pragma warning disable CS0618

[CustomEditor(typeof(Inventory))]
public class InventoryEditor : Editor {
	public override void OnInspectorGUI() {
		InventoryEditorGUI(target as Inventory);
	}

	public void InventoryEditorGUI(Inventory inventory) {
		List<(Item, int)> items = inventory.inventory;

		EditorGUILayout.LabelField("Inventory Editor");
		EditorGUILayout.Space();

		int size = inventory.inventory.Count;

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Items");
		size = EditorGUILayout.IntField(size);
		EditorGUILayout.EndHorizontal();

		while (size > items.Count)
			items.Add((ScriptableObject.CreateInstance<Item>(), 0));
		while (size < items.Count)
			items.RemoveAt(items.Count - 1);

		if (size > 0) {
			inventory.showElements = EditorGUILayout.Foldout(inventory.showElements, "Items", true);

			if (inventory.showElements) {
				EditorGUI.indentLevel++;
				for (int i = 0; i < size; i++) {
					(Item, int) inventoryItem = items[i];

					EditorGUILayout.LabelField("Item " + i);
					EditorGUI.indentLevel++;

					EditorGUILayout.LabelField("Item");
					inventoryItem.Item1 = (Item) EditorGUILayout.ObjectField(inventoryItem.Item1, typeof(Item));
					EditorGUILayout.LabelField("Quantity");
					inventoryItem.Item2 = EditorGUILayout.IntField(inventoryItem.Item2);

					EditorGUI.indentLevel--;
					EditorGUILayout.Space();
				}
				EditorGUI.indentLevel--;
			}
		}
	}

	private void serializedObjectInspectorGUI() {
		serializedObject.Update();

		SerializedProperty inventoryList = serializedObject.FindProperty("inventory");
		SerializedProperty quantityList = serializedObject.FindProperty("quantities");

		EditorGUILayout.LabelField("Inventory Editor");

		for (int i = 0; i < inventoryList.arraySize; i++) {
			SerializedProperty item = inventoryList.GetArrayElementAtIndex(i);
			SerializedProperty quantity = quantityList.GetArrayElementAtIndex(i);

			GUIContent itemName = new GUIContent();
			itemName.text = "Item";

			GUIContent itemQuantity = new GUIContent();
			itemQuantity.text = "Quantity";

			EditorGUILayout.LabelField("Element " + i);
			EditorGUILayout.PropertyField(item, itemName);
			EditorGUILayout.PropertyField(quantity, itemQuantity);

			if (GUILayout.Button("Delete Item")) {
				int j = i;
				inventoryList.DeleteArrayElementAtIndex(j);
				quantityList.DeleteArrayElementAtIndex(j);
			}

			EditorGUILayout.Space();
		}

		if (GUILayout.Button("Add Item")) {
			int index = inventoryList.arraySize;
			inventoryList.InsertArrayElementAtIndex(index);
			quantityList.InsertArrayElementAtIndex(index);
			inventoryList.GetArrayElementAtIndex(index).objectReferenceValue = ScriptableObject.CreateInstance<Item>();
			quantityList.GetArrayElementAtIndex(index).intValue = 0;
		}

		serializedObject.ApplyModifiedProperties();
	}
}
