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
    [SerializeField] private List<int> apparel;
    [SerializeField] private List<int> accessories;

    [SerializeField] private int gold;

	#region Getters
	/// <summary>
	/// Get all the consumables in the inventory
	/// </summary>
	public InventoryItem[] GetConsumables() {
        Atlas atlas = Resources.Load<Atlas>(Paths.ATLAS_PATH);
        InventoryItem[] inventoryItems = new InventoryItem[consumables.Count];

        for (int i = 0; i < consumables.Count; i++) {
            InventoryItem invItem = ScriptableObject.CreateInstance<InventoryItem>();
            invItem.item = atlas.consumables[consumables[i]];
            invItem.quantity = quantities[i];
            inventoryItems[i] = invItem;
		}

        return inventoryItems;
	}

    /// <summary>
    /// Get all the weapons in the inventory
    /// </summary>
    public Weapon[] GetWeapons() {
        Atlas atlas = Resources.Load<Atlas>(Paths.ATLAS_PATH);
        Weapon[] weps = new Weapon[weapons.Count];

        for (int i = 0; i < weapons.Count; i++)
            weps[i] = atlas.weapons[weapons[i]];

        return weps;
    }

    /// <summary>
    /// Get all the apparel in the inventory
    /// </summary>
    public Apparel[] GetApparel() {
        Atlas atlas = Resources.Load<Atlas>(Paths.ATLAS_PATH);
        Apparel[] apparelItems = new Apparel[apparel.Count];

        for (int i = 0; i < apparel.Count; i++)
            apparelItems[i] = atlas.apparel[apparel[i]];

        return apparelItems;
    }

    /// <summary>
    /// Get all the accessories in the inventory
    /// </summary>
    public Accessory[] GetAccessories() {
        Atlas atlas = Resources.Load<Atlas>(Paths.ATLAS_PATH);
        Accessory[] accessoryItems = new Accessory[accessories.Count];

        for (int i = 0; i < accessories.Count; i++)
            accessoryItems[i] = atlas.accessories[accessories[i]];

        return accessoryItems;
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
    /// Adds a weapon to the inventory. You can only hold one of each weapon.
    /// </summary>
    /// <param name="itemId">The ID of the weapon. ID comes from the <see cref="Atlas"/></param>
    /// <returns>Whether or not the weapon was added.</returns>
    public bool AddWeapon(int itemId) {
        if (weapons.Contains(itemId))
            return false;
        else {
            weapons.Add(itemId);
            return true;
        }
    }

    /// <summary>
    /// Removes a weapon from the inventory if it exists.
    /// </summary>
    /// <param name="itemId">The ID of the weapon. ID comes from the <see cref="Atlas"/></param>
    /// <returns>Whether or not the item was in the inventory and removed.</returns>
    public bool RemoveWeapon(int itemId) {
        return weapons.Remove(itemId);
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
    /// Adds an apparel item to the inventory. You can only hold one of each apparel.
    /// </summary>
    /// <param name="itemId">The ID of the apparel. ID comes from the <see cref="Atlas"/></param>
    /// <returns>Whether or not the apparel was added.</returns>
    public bool AddApparel(int itemId) {
        if (apparel.Contains(itemId))
            return false;
        else {
            apparel.Add(itemId);
            return true;
        }
    }

    /// <summary>
    /// Removes an apparel item from the inventory if it exists.
    /// </summary>
    /// <param name="itemId">The ID of the apparel. ID comes from the <see cref="Atlas"/></param>
    /// <returns>Whether or not the item was in the inventory and removed.</returns>
    public bool RemoveApparel(int itemId) {
        return apparel.Remove(itemId);
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
    /// Adds an accessory item to the inventory. You can only hold one of each accessories.
    /// </summary>
    /// <param name="itemId">The ID of the accessories. ID comes from the <see cref="Atlas"/></param>
    /// <returns>Whether or not the accessory was added.</returns>
    public bool AddAccessory(int itemId) {
        if (accessories.Contains(itemId))
            return false;
        else {
            accessories.Add(itemId);
            return true;
        }
    }

    /// <summary>
    /// Removes an accessory item from the inventory if it exists.
    /// </summary>
    /// <param name="itemId">The ID of the accessories. ID comes from the <see cref="Atlas"/></param>
    /// <returns>Whether or not the item was in the inventory and removed.</returns>
    public bool RemoveAccessory(int itemId) {
        return accessories.Remove(itemId);
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
