using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventory", menuName = "RPG Element/Inventory")]
public class Inventory : ScriptableObject {
    public List<(Item, int)> inventory;
    [HideInInspector]
    public bool showElements;

    public Inventory() {
        inventory = new List<(Item, int)>(0);
        showElements = false;
	}

    public List<InventoryItem> GetConsumableItems() {
        return new List<InventoryItem>() {

		};
	}
}
