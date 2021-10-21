using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#pragma warning disable CS8632

[RequireComponent(typeof(CanvasGroup))]
public class EquipmentMenu : MonoBehaviour, IMenuWindow {

	private enum EquipWindow {
		Weapon,
		Apparel,
		Accessory
	}

	private CanvasGroup canvasGroup;
	private EquipWindow openWindow;
	private List<GameObject> buttons;
	[HideInInspector]
	public bool isOpen;
	public GameObject itemButtonPrefab;
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

	private void Awake() {
		canvasGroup = GetComponent<CanvasGroup>();
	}

	public void OnWeaponButton() {
		if (openWindow != EquipWindow.Weapon) {
			hideCurrent();
			showWeapons();
		}
	}

	public void OnApparelButton() {
		if (openWindow != EquipWindow.Apparel) {
			hideCurrent();
			showApparel();
		}
	}

	public void OnAccessoryButton() {
		if (openWindow != EquipWindow.Accessory) {
			hideCurrent();
			showAccessories();
		}
	}

	public void OnUnequipButton() {
		switch (openWindow) {
			case EquipWindow.Weapon:
				infoText.text = "<color=red>You cannot unequip your weapon.";
				break;
			case EquipWindow.Apparel:
				if (PlayerUnit.INSTANCE.apparelID < 0)
					infoText.text = "<color=red>You don't have any apparel equipped.";
				else {
					Inventory.INSTANCE.AddApparel(PlayerUnit.INSTANCE.apparelID);
					PlayerUnit.INSTANCE.apparelID = -1;
					infoText.text = "Unequipped apparel.";
				}
				break;
			case EquipWindow.Accessory:
				if (PlayerUnit.INSTANCE.accessoryID < 0)
					infoText.text = "<color=red>You don't have any accessory equipped.";
				else {
					Inventory.INSTANCE.AddApparel(PlayerUnit.INSTANCE.accessoryID);
					PlayerUnit.INSTANCE.accessoryID = -1;
					infoText.text = "Unequipped apparel.";
				}
				break;
		}

		StartCoroutine(refresh());
	}

	public void Close() {
		StartCoroutine(fadeOut());
	}

	public void Open() {
		gameObject.SetActive(true);
		StartCoroutine(fadeIn());
		showWeapons();
		StartCoroutine(refresh());
	}

	private void showWeapons() {
		openWindow = EquipWindow.Weapon;
		Inventory inventory = Inventory.INSTANCE;
		InventoryItem<Weapon>[] weapons = inventory.GetWeapons();
		PlayerUnit player = PlayerUnit.INSTANCE;
		buttons = new List<GameObject>();

		foreach (InventoryItem<Weapon> weapon in weapons) {
			GameObject button = Instantiate(itemButtonPrefab, buttonArea);
			SkillButtonController controller = button.GetComponent<SkillButtonController>();
			buttons.Add(button);

			controller.setData(weapon);
			controller.useTextField = true;
			controller.textField = infoText;
			int itemId = Atlas.GetID(weapon.item);

			button.GetComponent<Button>().onClick.AddListener(
				() => equipItem(itemId)
			);
		}
	}

	private void showApparel() {
		openWindow = EquipWindow.Apparel;
		Inventory inventory = Inventory.INSTANCE;
		InventoryItem<Apparel>[] apparel = inventory.GetApparel();
		PlayerUnit player = PlayerUnit.INSTANCE;
		buttons = new List<GameObject>();

		foreach (InventoryItem<Apparel> apparelItem in apparel) {
			GameObject button = Instantiate(itemButtonPrefab, buttonArea);
			SkillButtonController controller = button.GetComponent<SkillButtonController>();
			buttons.Add(button);

			controller.setData(apparelItem);
			controller.useTextField = true;
			controller.textField = infoText;
			int itemId = Atlas.GetID(apparelItem.item);

			button.GetComponent<Button>().onClick.AddListener(
				() => equipItem(itemId)
			);
		}
	}

	private void showAccessories() {
		openWindow = EquipWindow.Accessory;
		Inventory inventory = Inventory.INSTANCE;
		InventoryItem<Accessory>[] accessories = inventory.GetAccessories();
		PlayerUnit player = PlayerUnit.INSTANCE;
		buttons = new List<GameObject>();

		foreach (InventoryItem<Accessory> accessory in accessories) {
			GameObject button = Instantiate(itemButtonPrefab, buttonArea);
			SkillButtonController controller = button.GetComponent<SkillButtonController>();
			buttons.Add(button);

			controller.setData(accessory);
			controller.useTextField = true;
			controller.textField = infoText;
			int itemId = Atlas.GetID(accessory.item);

			button.GetComponent<Button>().onClick.AddListener(
				() => equipItem(itemId)
			);
		}
	}

	public bool IsOpen() {
		return isOpen;
	}

	private void equipItem(int id) {
		PlayerUnit player = PlayerUnit.INSTANCE;
		Inventory inventory = Inventory.INSTANCE;
		string? infotext = null;

		switch (openWindow) {
			case EquipWindow.Weapon:
				inventory.AddWeapon(player.weaponId);
				player.weaponId = id;
				inventory.RemoveWeapon(id);
				if (player.accessoryID >= 0) {
					(bool isCompatible, string msg) data = player.accessory.CheckCompatibility(Atlas.GetWeapon(id));
					if (!data.isCompatible) {
						inventory.AddAccessory(player.accessoryID);
						player.accessoryID = -1;
						infotext = data.msg;
					}
				}
				break;
			case EquipWindow.Apparel:
				if (player.apparelID >= 0)
					inventory.AddApparel(player.apparelID);
				player.apparelID = id;
				inventory.RemoveApparel(id);
				break;
			case EquipWindow.Accessory:
				(bool isCompatible, string msg) itemData = Atlas.GetAccessory(id).CheckCompatibility(player.weapon);
				if (itemData.isCompatible) {
					if (player.accessoryID >= 0)
						inventory.AddAccessory(player.accessoryID);
					player.accessoryID = id;
					inventory.RemoveAccessory(id);
				} else
					infotext = itemData.msg;
				break;
		}

		StartCoroutine(refresh(infotext));
	}

	private void hideCurrent() {
		if (buttons != null)
			foreach (GameObject obj in buttons)
				Destroy(obj);
	}


	private IEnumerator refresh(string? refreshMsg = null) {
		hideCurrent();

		yield return new WaitForEndOfFrame();

		switch (openWindow) {
			case EquipWindow.Weapon:
				showWeapons();
				break;
			case EquipWindow.Apparel:
				showApparel();
				break;
			case EquipWindow.Accessory:
				showAccessories();
				break;
		}

		weaponIcon.sprite = PlayerUnit.INSTANCE.weapon.itemIcon;
		
		if (PlayerUnit.INSTANCE.apparelID >= 0)
			apparelIcon.sprite = PlayerUnit.INSTANCE.apparel.itemIcon;
		else
			apparelIcon.sprite = blankApparelIcon;

		if (PlayerUnit.INSTANCE.accessoryID >= 0)
			accessoryIcon.sprite = PlayerUnit.INSTANCE.accessory.itemIcon;
		else
			accessoryIcon.sprite = blankAccessoryIcon;

		yield return new WaitForSeconds(0.1f);

		if (refreshMsg != null)
			infoText.text = refreshMsg;
	}

	private IEnumerator fadeOut() {
		canvasGroup.alpha = 1;

		while (canvasGroup.alpha > 0) {
			canvasGroup.alpha -= 0.1f;
			yield return new WaitForSeconds(0.01f);
		}


		hideCurrent();
		isOpen = false;
		gameObject.SetActive(false);
	}

	private IEnumerator fadeIn() {
		canvasGroup.alpha = 0;

		while (canvasGroup.alpha < 1) {
			canvasGroup.alpha += 0.1f;
			yield return new WaitForSeconds(0.01f);
		}

		isOpen = true;
	}
}
