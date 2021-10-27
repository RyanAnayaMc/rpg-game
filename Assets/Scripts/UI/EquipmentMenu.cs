using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

#pragma warning disable IDE0044, IDE0051, CS8632
[RequireComponent(typeof(CanvasGroup))]
public class EquipmentMenu : MonoBehaviour, IMenuWindow {
	#region Fields
	private const string kRed = "<color=red>";
	private enum EquipWindow {
		Weapon,
		Apparel,
		Accessory
	}

	private CanvasGroup canvasGroup;
	private EquipWindow openWindow;
	private List<GameObject> buttons;
	private bool isOpen;
	[SerializeField]
	private GameObject itemButtonPrefab;
	[SerializeField]
	private Transform buttonArea;
	[SerializeField]
	private Image weaponIcon;
	[SerializeField]
	private Image accessoryIcon;
	[SerializeField]
	private Image apparelIcon;
	[SerializeField]
	private Sprite blankApparelIcon;
	[SerializeField]
	private Sprite blankAccessoryIcon;
	[SerializeField]
	private TMP_Text infoText;
	#endregion

	private void Awake() {
		canvasGroup = GetComponent<CanvasGroup>();
	}

	#region Button Handlers
	/// <summary>
	/// Triggered when the weapon button is pressed.
	/// Displays the weapon equip menu.
	/// </summary>
	public void OnWeaponButton() {
		if (openWindow != EquipWindow.Weapon) {
			HideCurrent();
			ShowWeapons();
		}
	}

	/// <summary>
	/// Triggered when the apparel button is pressed.
	/// Displays the apparel equip menu.
	/// </summary>
	public void OnApparelButton() {
		if (openWindow != EquipWindow.Apparel) {
			HideCurrent();
			ShowApparel();
		}
	}

	/// <summary>
	/// Triggered when the accessory button is pressed.
	/// Displays the accessory equip menu.
	/// </summary>
	public void OnAccessoryButton() {
		if (openWindow != EquipWindow.Accessory) {
			HideCurrent();
			ShowAccessories();
		}
	}

	/// <summary>
	/// Triggered when unequip button is pressed. Tries to
	/// unequip the equipped gear piece in a specific category
	/// based on the currently opened equipment window. Displays
	/// success or error message.
	/// </summary>
	public void OnUnequipButton() {
		switch (openWindow) {
			case EquipWindow.Weapon:
				// Unequip button pressed on weapon page
				infoText.text = kRed + "You cannot unequip your weapon.";
				break;
			case EquipWindow.Apparel:
				// Unequip button pressed on apparel page
				if (PlayerUnit.INSTANCE.apparelID < 0)
					// Player has no apparel equipped
					infoText.text = kRed + "You don't have any apparel equipped.";
				else {
					// Player has apparel equipped
					Inventory.INSTANCE.AddApparel(PlayerUnit.INSTANCE.apparelID);
					PlayerUnit.INSTANCE.apparelID = -1;
					infoText.text = "Unequipped apparel.";
				}
				break;
			case EquipWindow.Accessory:
				// Unequip button pressed on accessory page
				if (PlayerUnit.INSTANCE.accessoryID < 0)
					// Player has no accessory equipped
					infoText.text = kRed + "You don't have any accessory equipped.";
				else {
					// Player has accessory equipped
					Inventory.INSTANCE.AddApparel(PlayerUnit.INSTANCE.accessoryID);
					PlayerUnit.INSTANCE.accessoryID = -1;
					infoText.text = "Unequipped accessory.";
				}
				break;
		}

		_ = Refresh();
	}
	#endregion

	#region IMenuWindow implementation
	public bool IsOpen() {
		return isOpen;
	}

	public async Task Open() {
		openWindow = EquipWindow.Weapon;
		_ = Refresh();

		gameObject.SetActive(true);
		await Utilities.WaitUntil(() => gameObject.activeInHierarchy);

		canvasGroup.alpha = 0;

		while (canvasGroup.alpha < 1) {
			canvasGroup.alpha += 0.1f;
			await Task.Delay(10);
		}

		isOpen = true;
	}

	public async Task Close() {
		canvasGroup.alpha = 1;

		while (canvasGroup.alpha > 0) {
			canvasGroup.alpha -= 0.1f;
			await Task.Delay(10);
		}

		isOpen = false;
		gameObject.SetActive(false);
		HideCurrent();
	}
	#endregion

	#region Show Equipment Category
	/// <summary>
	/// Displays the player's weapons in the sub-inventory menu.
	/// </summary>
	private void ShowWeapons() {
		openWindow = EquipWindow.Weapon;
		Inventory inventory = Inventory.INSTANCE;
		InventoryItem<Weapon>[] weapons = inventory.GetWeapons();
		PlayerUnit player = PlayerUnit.INSTANCE;
		buttons = new List<GameObject>();

		foreach (InventoryItem<Weapon> weapon in weapons) {
			GameObject button = Instantiate(itemButtonPrefab, buttonArea);
			SkillButtonController controller = button.GetComponent<SkillButtonController>();
			buttons.Add(button);

			controller.SetData(weapon);
			controller.useTextField = true;
			controller.textField = infoText;
			int itemId = Atlas.GetID(weapon.item);

			button.GetComponent<Button>().onClick.AddListener(
				() => EquipItem(itemId)
			);
		}
	}

	/// <summary>
	/// Displays the player's apparel in the sub-inventory menu.
	/// </summary>
	private void ShowApparel() {
		openWindow = EquipWindow.Apparel;
		Inventory inventory = Inventory.INSTANCE;
		InventoryItem<Apparel>[] apparel = inventory.GetApparel();
		PlayerUnit player = PlayerUnit.INSTANCE;
		buttons = new List<GameObject>();

		foreach (InventoryItem<Apparel> apparelItem in apparel) {
			GameObject button = Instantiate(itemButtonPrefab, buttonArea);
			SkillButtonController controller = button.GetComponent<SkillButtonController>();
			buttons.Add(button);

			controller.SetData(apparelItem);
			controller.useTextField = true;
			controller.textField = infoText;
			int itemId = Atlas.GetID(apparelItem.item);

			button.GetComponent<Button>().onClick.AddListener(
				() => EquipItem(itemId)
			);
		}
	}

	/// <summary>
	/// Displays the player's accessories in the sub-inventory menu.
	/// </summary>
	private void ShowAccessories() {
		openWindow = EquipWindow.Accessory;
		Inventory inventory = Inventory.INSTANCE;
		InventoryItem<Accessory>[] accessories = inventory.GetAccessories();
		PlayerUnit player = PlayerUnit.INSTANCE;
		buttons = new List<GameObject>();

		foreach (InventoryItem<Accessory> accessory in accessories) {
			GameObject button = Instantiate(itemButtonPrefab, buttonArea);
			SkillButtonController controller = button.GetComponent<SkillButtonController>();
			buttons.Add(button);

			controller.SetData(accessory);
			controller.useTextField = true;
			controller.textField = infoText;
			int itemId = Atlas.GetID(accessory.item);

			button.GetComponent<Button>().onClick.AddListener(
				() => EquipItem(itemId)
			);
		}
	}
	#endregion

	#region Equipping
	/// <summary>
	/// Equips an item with the given ID. Automatically determines
	/// if it is a weapon, apparel, or accessory based on the currently
	/// opened menu.
	/// </summary>
	/// <param name="id">The ID of the item to equip.</param>
	private void EquipItem(int id) {
		PlayerUnit player = PlayerUnit.INSTANCE;
		Inventory inventory = Inventory.INSTANCE;
		string? infotext = null;

		switch (openWindow) {
			case EquipWindow.Weapon:
				infotext = EquipWeapon(player, inventory, id);
				break;
			case EquipWindow.Apparel:
				EquipApparel(player, inventory, id);
				break;
			case EquipWindow.Accessory:
				infotext = EquipAccessory(player, inventory, id);
				break;
		}

		_ = Refresh(infotext);
	}

	/// <summary>
	/// Equips a weapon with the given ID. Unequips
	/// incompatible accessories automatically.
	/// </summary>
	/// <returns>The accessory unequip message if an accessory was unequipped.</returns>
	private string? EquipWeapon(PlayerUnit player, Inventory inventory, int id) {
		string? infotext = null;

		inventory.AddWeapon(player.weaponId);
		player.weaponId = id;
		inventory.RemoveWeapon(id);
		if (player.accessoryID >= 0) {
			(bool isCompatible, string msg) = player.accessory.CheckCompatibility(Atlas.GetWeapon(id));
			if (!isCompatible) {
				inventory.AddAccessory(player.accessoryID);
				player.accessoryID = -1;
				infotext = msg;
			}
		}

		return infotext;
	}

	/// <summary>
	/// Equips an apparel item with the given ID.
	/// </summary>
	private void EquipApparel(PlayerUnit player, Inventory inventory, int id) {
		if (player.apparelID >= 0)
			inventory.AddApparel(player.apparelID);
		player.apparelID = id;
		inventory.RemoveApparel(id);
	}

	/// <summary>
	/// Equips an accessory with the given ID if possible. If incompatible with the
	/// weapon, nothing happens and an error string is returned.
	/// </summary>
	/// <returns>The accessory equip fail message if it cannot be equipped.</returns>
	private string? EquipAccessory(PlayerUnit player, Inventory inventory, int id) {
		string? infotext = null;

		(bool isCompatible, string msg) = Atlas.GetAccessory(id).CheckCompatibility(player.weapon);
		if (isCompatible) {
			if (player.accessoryID >= 0)
				inventory.AddAccessory(player.accessoryID);
			player.accessoryID = id;
			inventory.RemoveAccessory(id);
		} else
			infotext = msg;

		return infotext;
	}
	#endregion

	#region Helper Methods
	/// <summary>
	/// Closes the current menu by destroying all the buttons in it
	/// </summary>
	private void HideCurrent() {
		if (buttons != null)
			foreach (GameObject obj in buttons)
				Destroy(obj);
	}

	/// <summary>
	/// Deletes all buttons on the menu and repopulates it
	/// </summary>
	/// <param name="refreshMsg">
	/// The message to display in the dialogue
	/// box after refreshing the menu.
	/// </param>
	private async Task Refresh(string? refreshMsg = null) {
		HideCurrent();

		// Show the currently opened window again
		switch (openWindow) {
			case EquipWindow.Weapon:
				ShowWeapons();
				break;
			case EquipWindow.Apparel:
				ShowApparel();
				break;
			case EquipWindow.Accessory:
				ShowAccessories();
				break;
		}

		// Player always has a weapon so this will never throw a
		// NullReferenceException
		weaponIcon.sprite = PlayerUnit.INSTANCE.weapon.itemIcon;
		
		// Display apparel icon if apparel equipped
		if (PlayerUnit.INSTANCE.apparelID >= 0)
			apparelIcon.sprite = PlayerUnit.INSTANCE.apparel.itemIcon;
		else
			apparelIcon.sprite = blankApparelIcon;

		// Display accessory icon if equipped
		if (PlayerUnit.INSTANCE.accessoryID >= 0)
			accessoryIcon.sprite = PlayerUnit.INSTANCE.accessory.itemIcon;
		else
			accessoryIcon.sprite = blankAccessoryIcon;

		// Short delay before showing refresh message it beats the 
		// button hover message
		await Task.Delay(10);
		if (refreshMsg != null)
			infoText.text = refreshMsg;
	}
	#endregion
}
