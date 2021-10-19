using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0659, CS0661

/// <summary>
/// Base class for items that the user can have in their inventory.
/// </summary>
[System.Serializable]
public class Item : ScriptableObject, ButtonTextable {
    /// <summary>
    /// The icon for the item.
    /// </summary>
    public Sprite itemIcon;

    /// <summary>
    /// The name of the item.
    /// </summary>
    public string itemName;

    /// <summary>
    /// The description of the item.
    /// </summary>
    public string itemDescription;

    /// <summary>
    /// Whether or not the item can be sold.
    /// </summary>
    public bool isSellable;

    /// <summary>
    /// The buy price of the item for shops.
    /// </summary>
    public int buyPrice;

    /// <summary>
    /// The maximum stackable amount for an item in the inventory.
    /// </summary>
    public int stackableMax;

	public static bool operator==(Item item1, Item item2) {
        return item1.itemName == item2.itemName
            && item1.itemDescription == item2.itemDescription
            && item1.isSellable == item2.isSellable
            && item1.buyPrice == item2.buyPrice;
	}

    public static bool operator!=(Item item1, Item item2) {
        return !(item1 == item2);
	}

	public override bool Equals(object other) {
        return this == (Item) other;
	}

    public string GetDescriptionText() {
        return itemDescription;
    }

    public Sprite GetIcon() {
        return itemIcon;
    }

    public string GetName() {
        return itemName;
    }

    public virtual int GetNumber() {
        return 1;
    }
}
