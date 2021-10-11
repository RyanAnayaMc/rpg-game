using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewInventoryItem", menuName = "RPG Element/Debug/Inventory Item")]
public class InventoryItem : ScriptableObject, ButtonTextable
{
	[SerializeField]
    public Item item;
    [Range(0, 128)]
	[SerializeField]
    public int quantity;

	public string GetDescriptionText() {
		return item.itemDescription;
	}

	public Sprite GetIcon() {
		return item.itemIcon;
	}

	public string GetName() {
		return item.itemName;
	}

	public int GetNumber() {
		return quantity;
	}
}
