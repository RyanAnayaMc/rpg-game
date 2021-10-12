using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory
{
	private static Inventory _instance;

    public static Inventory INSTANCE {
		get {
			if (_instance is null)
				_instance = (Inventory) Resources.Load("PlayerInventory");
			return _instance;
		}
	}
 }
