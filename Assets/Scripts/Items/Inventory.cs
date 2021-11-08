using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventory", menuName = "RPG Element/Inventory")]
public class Inventory : ScriptableObject {
    public static Inventory INSTANCE {
        get { return Resources.Load<Inventory>(Paths.INVENTORY_PATH); }
	}
    [SerializeField] private List<int> consumables;
    [SerializeField] private List<int> quantities;
    [SerializeField] private List<int> weapons;
    [SerializeField] private List<int> weaponQuantities;
    [SerializeField] private List<int> apparel;
    [SerializeField] private List<int> apparelQuantities;
    [SerializeField] private List<int> accessories;
    [SerializeField] private List<int> accessoryQuantities;

    [SerializeField] private int gold;

	#region Getters
	/// <summary>
	/// Get all the consumables in the inventory
	/// </summary>
	public InventoryItem<Consumable>[] GetConsumables() {
        InventoryItem<Consumable>[] inventoryItems = new InventoryItem<Consumable>[consumables.Count];

        for (int i = 0; i < consumables.Count; i++) {
            InventoryItem<Consumable> invItem = new InventoryItem<Consumable>();
            invItem.quantity = quantities[i];
            invItem.item = Atlas.GetConsumable(consumables[i]);
            inventoryItems[i] = invItem;
		}

        return inventoryItems;
	}

    /// <summary>
    /// Get all the weapons in the inventory
    /// </summary>
    public InventoryItem<Weapon>[] GetWeapons() {
        InventoryItem<Weapon>[] inventoryItems = new InventoryItem<Weapon>[weapons.Count];

        for (int i = 0; i < weapons.Count; i++) {
            InventoryItem<Weapon> invItem = new InventoryItem<Weapon>();
            invItem.item = Atlas.GetWeapon(weapons[i]);
            invItem.quantity = weaponQuantities[i];
            inventoryItems[i] = invItem;
		}

        return inventoryItems;
    }

    /// <summary>
    /// Get all the apparel in the inventory
    /// </summary>
    public InventoryItem<Apparel>[] GetApparel() {
        InventoryItem<Apparel>[] inventoryItems = new InventoryItem<Apparel>[apparel.Count];

        for (int i = 0; i < apparel.Count; i++) {
            InventoryItem<Apparel> invItem = new InventoryItem<Apparel>();
            invItem.item = Atlas.GetApparel(apparel[i]);
            invItem.quantity = apparelQuantities[i];
            inventoryItems[i] = invItem;
        }

        return inventoryItems;
    }

    /// <summary>
    /// Get all the accessories in the inventory
    /// </summary>
    public InventoryItem<Accessory>[] GetAccessories() {
        InventoryItem<Accessory>[] inventoryItems = new InventoryItem<Accessory>[accessories.Count];

        for (int i = 0; i < accessories.Count; i++) {
            InventoryItem<Accessory> invItem = new InventoryItem<Accessory>();
            invItem.item = Atlas.GetAccessory(accessories[i]);
            invItem.quantity = accessoryQuantities[i];
            inventoryItems[i] = invItem;
        }

        return inventoryItems;
    }
	#endregion

	#region Gold
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
	#endregion

	#region Consumables
	/// <summary>
	/// Adds an item to the inventory.
	/// </summary>
	/// <param name="item">The item to add. ID comes from the <see cref="Atlas"/>.</param>
	public void AddConsumable(int itemId) {
        AddConsumable(itemId, 1);
	}

    /// <summary>
    /// Adds an item to the inventory.
    /// </summary>
    /// <param name="item">The item to add. ID comes from the <see cref="Atlas"/>.</param>
    /// <param name="quantity">How many of the item to add.</param>
    public void AddConsumable(int itemId, int quantity) {
        int index = consumables.IndexOf(itemId);

        if (index < 0) {
            consumables.Add(itemId);
            quantities.Add(quantity);
        } else
            quantities[index] += quantity;
	}

    /// <summary>
    /// Removes one item from inventory if possible.
    /// </summary>
    /// <param name="item">The item to remove. ID comes from the <see cref="Atlas"/></param>
    /// <returns>Whether or not the item was removed.</returns>
    public bool RemoveConsumable(int itemId) {
        return RemoveConsumable(itemId, 1);
	}

    /// <summary>
    /// Removes a given quantity of an item from inventory if possible. If there is not enough
    /// quantity then no items are removed, even if some of the items existed in the inventory.
    /// </summary>
    /// <param name="item">The item to remove. ID comes from the <see cref="Atlas"/></param>
    /// <param name="amount">The amount to remove.</param>
    /// <returns>Whether or not the remove was successful.</returns>
    public bool RemoveConsumable(int itemId, int amount) {
        int index = consumables.IndexOf(itemId);
        if (index < 0)
            return false;
        else {
            if (quantities[index] == amount) {
                quantities.RemoveAt(index);
                consumables.RemoveAt(index);
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
    /// <param name="itemId">The item to check the quantity of. ID comes from the <see cref="Atlas"/></param>
    /// <returns></returns>
    public int QuantityOfConsumable(int itemId) {
        int index = consumables.IndexOf(itemId);
        if (index < 0)
            return 0;
        else
            return quantities[index];
	}
    #endregion

    #region Weapons
    /// <summary>
    /// Adds a weapon to the inventory.
    /// </summary>
    /// <param name="itemId">The ID of the weapon. ID comes from the <see cref="Atlas"/></param>
    public void AddWeapon(int itemId) {
        AddWeapon(itemId, 1);
	}

    /// <summary>
    /// Adds a weapon to the inventory. Ensures that the weapons are sorted by index.
    /// </summary>
    /// <param name="itemId">The ID of the weapon. ID comes from the <see cref="Atlas"/></param>
    /// <param name="quantity">How many of the item to add.</param>
    public void AddWeapon(int itemId, int quantity) {
        int index = weapons.IndexOf(itemId);

        if (index < 0) {
            index = 0;
            while (weapons[index] < itemId) { index++; }
            weapons.Insert(index, itemId);
            weaponQuantities.Insert(index, quantity);
		} else {
            weaponQuantities[index] += quantity;
		}
    }

    /// <summary>
    /// Removes a weapon from the inventory if it exists.
    /// </summary>
    /// <param name="itemId">The ID of the weapon. ID comes from the <see cref="Atlas"/></param>
    /// <returns>Whether or not the item was in the inventory and removed.</returns>
    public bool RemoveWeapon(int itemId) {
        return RemoveWeapon(itemId, 1);
    }

    /// <summary>
    /// Removes a weapon from the inventory if it exists. The inventory will not be changed if
    /// not enough items exist.
    /// </summary>
    /// <param name="itemId">The ID of the weapon. ID comes from the <see cref="Atlas"/></param>
    /// <param name="amount">How many of the item to remove.</param>
    /// <returns>Whether or not the inventory was changed by this operation.</returns>
    public bool RemoveWeapon(int itemId, int amount) {
        int index = weapons.IndexOf(itemId);
        if (index < 0)
            return false;
        else {
            if (weaponQuantities[index] == amount) {
                weaponQuantities.RemoveAt(index);
                weapons.RemoveAt(index);
                return true;
            } else if (weaponQuantities[index] > amount) {
                weaponQuantities[index] -= amount;
                return true;
            } else
                return false;
        }
    }

    /// <summary>
    /// Finds out if a weapon is in the inventory.
    /// </summary>
    /// <param name="itemId">The ID of the weapon. ID comes from the <see cref="Atlas"/></param>
    /// <returns>Whether or not the weapon is in the inventory.</returns>
    public bool HasWeapon(int itemId) {
        return weapons.Contains(itemId);
    }
    #endregion

    #region Apparel
    /// <summary>
    /// Adds an apparel item to the inventory.
    /// </summary>
    /// <param name="itemId">The ID of the apparel. ID comes from the <see cref="Atlas"/></param>
    public void AddApparel(int itemId) {
        AddApparel(itemId, 1);
    }

    /// <summary>
    /// Adds an apparel item to the inventory. Keeps items sorted by item ID.
    /// </summary>
    /// <param name="itemId">The ID of the apparel. ID comes from the <see cref="Atlas"/></param>
    /// <param name="amount">The amount of the apparel item to add.</param>
    public void AddApparel(int itemId, int amount) {
        int index = apparel.IndexOf(itemId);

        if (index < 0) {
            index = 0;
            while (apparel[index] < itemId) { index++; }
            apparel.Insert(index, itemId);
            apparelQuantities.Insert(index, itemId);
        } else
            apparelQuantities[index] += amount;
	}

    /// <summary>
    /// Removes an apparel item from the inventory if it exists.
    /// </summary>
    /// <param name="itemId">The ID of the apparel. ID comes from the <see cref="Atlas"/></param>
    /// <returns>Whether or not the item was in the inventory and removed.</returns>
    public bool RemoveApparel(int itemId) {
        return RemoveApparel(itemId, 1);
	}

    /// <summary>
    /// Removes an apparel item from the inventory if it exists.
    /// </summary>
    /// <param name="itemId">The ID of the apparel. ID comes from the <see cref="Atlas"/></param>
    /// <param name="amount">The amount of the item to remove.</param>
    /// <returns>Whether or not the item was in the inventory and removed.</returns>
    public bool RemoveApparel(int itemId, int amount) {
        int index = apparel.IndexOf(itemId);
        if (index < 0)
            return false;
        else {
            if (apparelQuantities[index] == amount) {
                apparel.RemoveAt(index);
                apparelQuantities.RemoveAt(index);
                return true;
            } else if (apparelQuantities[index] > amount) {
                apparelQuantities[index] -= amount;
                return true;
            } else
                return false;
        }
    }

    /// <summary>
    /// Finds out if an apparel item is in the inventory.
    /// </summary>
    /// <param name="itemId">The ID of the apparel. ID comes from the <see cref="Atlas"/></param>
    /// <returns>Whether or not the apparel is in the inventory.</returns>
    public bool HasApparel(int itemId) {
        return apparel.Contains(itemId);
    }
    #endregion

    #region Accessory
    /// <summary>
    /// Adds an accessory item to the inventory.
    /// </summary>
    /// <param name="itemId">The ID of the accessories. ID comes from the <see cref="Atlas"/></param>
    public void AddAccessory(int itemId) {
        AddAccessory(itemId, 1);
    }

    /// <summary>
    /// Adds an accessory item to the inventory. Keeps items sorted by item ID.
    /// </summary>
    /// <param name="itemId">The ID of the accessories. ID comes from the <see cref="Atlas"/></param>
    /// <param name="amount">The amount of the accessory to add.</param>
    public void AddAccessory(int itemId, int amount) {
        int index = accessories.IndexOf(itemId);
        if (index < 0) {
            index = 0;
            while (accessories[index] < itemId) { index++; }
            accessories.Insert(index, itemId);
            accessoryQuantities.Insert(index, amount);
        } else
            accessoryQuantities[index] += amount;
	}

    /// <summary>
    /// Removes an accessory item from the inventory if it exists.
    /// </summary>
    /// <param name="itemId">The ID of the accessories. ID comes from the <see cref="Atlas"/></param>
    /// <returns>Whether or not the item was in the inventory and removed.</returns>
    public bool RemoveAccessory(int itemId) {
        return RemoveAccessory(itemId, 1);
	}

    /// <summary>
    /// Removes an accessory item from the inventory if it exists.
    /// </summary>
    /// <param name="itemId">The ID of the accessories. ID comes from the <see cref="Atlas"/></param>
    /// <param name="amount">The amount of the item to remove.</param>
    /// <returns>Whether or not the item was in the inventory and removed.</returns>
    public bool RemoveAccessory(int itemId, int amount) {
        int index = accessories.IndexOf(itemId);
        if (index < 0)
            return false;
        else {
            if (accessoryQuantities[index] == amount) {
                accessories.RemoveAt(index);
                accessoryQuantities.RemoveAt(index);
                return true;
            } else if (accessoryQuantities[index] > amount) {
                accessoryQuantities[index] -= amount;
                return true;
            } else
                return false;
        }
    }

    /// <summary>
    /// Finds out if an accessory item is in the inventory.
    /// </summary>
    /// <param name="itemId">The ID of the accessories. ID comes from the <see cref="Atlas"/></param>
    /// <returns>Whether or not the accessory is in the inventory.</returns>
    public bool HasAccessory(int itemId) {
        return accessories.Contains(itemId);
    }
    #endregion
}
