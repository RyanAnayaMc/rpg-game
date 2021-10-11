using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventory", menuName = "RPG Element/Inventory")]
public class Inventory : ScriptableObject {
    public List<Item> items;
    public List<int> quantities;

    public List<InventoryItem> GetConsumableItems() {
        List<InventoryItem> consumables = new List<InventoryItem>();

        for (int i = 0; i < items.Count; i++)
            if (items[i] is Consumable)
                consumables.Add(getByIndex(i));

        return consumables;
	}

    /// <summary>
    /// Removes one item from inventory if possible.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    /// <returns>Whether or not the item was removed.</returns>
    public bool RemoveItem(Item item) {
        return RemoveItem(item, 1);
	}

    /// <summary>
    /// Removes a given quantity of an item from inventory if possible. If there is not enough
    /// quantity then no items are removed, even if some of the items existed in the inventory.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    /// <param name="amount">The amount to remove.</param>
    /// <returns>Whether or not the remove was successful.</returns>
    public bool RemoveItem(Item item, int amount) {
        int index = items.IndexOf(item);
        if (index < 0)
            return false;
        else {
            if (quantities[index] == amount) {
                quantities.RemoveAt(index);
                items.RemoveAt(index);
                return true;
            } else if (quantities[index] > amount) {
                quantities[index] -= amount;
                return true;
            } else
                return false;
		}

	}

    private InventoryItem createInventoryItem(Item item, int quantity) {
        InventoryItem invItem = ScriptableObject.CreateInstance<InventoryItem>();
        invItem.item = item;
        invItem.quantity = quantity;
        return invItem;
	}

    private InventoryItem getByIndex(int i) {
        return createInventoryItem(items[i], quantities[i]);
	}
}
