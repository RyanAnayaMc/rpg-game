using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
