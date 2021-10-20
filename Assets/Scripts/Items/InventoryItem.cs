using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem<T> : ButtonTextable where T : Item {
	[SerializeField]
	public T item;
	[Range(0, 128)]
	[SerializeField]
	public int quantity;

	public InventoryItem() {
		item = null;
		quantity = 0;
	}

	public string GetDescriptionText() {
		if (item is Weapon)
			return (item as Weapon).GetDescriptionText();
		else if (item is Apparel)
			return (item as Apparel).GetDescriptionText();
		else if (item is Accessory)
			return (item as Accessory).GetDescriptionText();
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
