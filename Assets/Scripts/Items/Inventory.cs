using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventory", menuName = "RPG Element/Inventory")]
public class Inventory : ScriptableObject {
    public List<Item> items;
    public List<int> quantities;
    [SerializeField]
    private int gold;

    public List<InventoryItem> GetConsumableItems() {
        List<InventoryItem> consumables = new List<InventoryItem>();

        for (int i = 0; i < items.Count; i++)
            if (items[i] is Consumable)
                consumables.Add(getByIndex(i));

        return consumables;
	}

    /// <summary>
    /// Gets the amount of gold in this inventory
    /// </summary>
    public int GetGold() {
        return gold;
	}

    /// <summary>
    /// Subtracts the amount of gold from the inventory if the inventory has enough gold.
    /// If the inventory does not have enough gold, it will not be changed.
    /// </summary>
    /// <param name="amount">The amount is</param>
    /// <returns><see langword="true"/> if the gold was removed. <see langword="false"/> if
    /// there was not enough gold in the inventory.</returns>
    public bool SubtractGold(int amount) {
        if (amount > gold)
            return false;
        else {
            gold -= amount;
            return true;
		}
	}

    /// <summary>
    /// Adds an item to the inventory.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public void AddItem(Item item) {
        AddItem(item, 1);
	}

    /// <summary>
    /// Adds an item to the inventory.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <param name="quantity">How many of the item to add.</param>
    public void AddItem(Item item, int quantity) {
        int index = items.IndexOf(item);

        if (index < 0) {
            items.Add(item);
            quantities.Add(quantity);
        } else
            quantities[index] += quantity;
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

    /// <summary>
    /// Returns the quantity of an item in the inventory.
    /// </summary>
    /// <param name="item">The item to check the quantity of.</param>
    /// <returns></returns>
    public int QuantityOfItem(Item item) {
        int index = items.IndexOf(item);

        if (index < 0)
            return 0;
        else
            return quantities[index];
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
