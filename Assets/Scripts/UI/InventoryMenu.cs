using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

#pragma warning disable IDE0044
public class InventoryMenu : MonoBehaviour, IMenuWindow {
	[SerializeField] private Transform content;
	[SerializeField] private GameObject buttonPrefab;
	private CanvasGroup canvasGroup;
	[HideInInspector] public bool isOpen;
	private List<GameObject> buttons;
	[SerializeField] private TMP_Text infoText;

	public void Awake() {
		canvasGroup = GetComponent<CanvasGroup>();
	}

	#region IMenuWindow implementation
	public bool IsOpen() {
		return isOpen;
	}

	public async Task Open() {
		PopulateMenu();
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

		DestroyButtons();

		isOpen = false;
		gameObject.SetActive(false);
	}
	#endregion

	#region Show Items Methods
	/// <summary>
	/// Populates the menu with all the inventory buttons. Make sure
	/// the menu has been cleared with DestroyButtons() before
	/// calling this!
	/// </summary>
	private void PopulateMenu() {
		buttons = new List<GameObject>();
		ShowConsumables();
		ShowWeapons();
		ShowApparel();
		ShowAccessories();
	}

	/// <summary>
	/// Adds consumables to the displayed inventory menu.
	/// </summary>
	private void ShowConsumables() {
		InventoryItem<Consumable>[] consumables = Inventory.INSTANCE.GetConsumables();

		foreach (InventoryItem<Consumable> item in consumables) {
			GameObject obj = Instantiate(buttonPrefab, content);
			SkillButtonController controller = obj.GetComponent<SkillButtonController>();
			controller.SetData(item);
			controller.useTextField = true;
			controller.textField = infoText;

			buttons.Add(obj);
		}
	}

	/// <summary>
	/// Adds weapons to the displayed inventory menu.
	/// </summary>
	private void ShowWeapons() {
		InventoryItem<Weapon>[] weapons = Inventory.INSTANCE.GetWeapons();

		foreach (InventoryItem<Weapon> item in weapons) {
			GameObject obj = Instantiate(buttonPrefab, content);
			SkillButtonController controller = obj.GetComponent<SkillButtonController>();
			controller.SetData(item);
			controller.useTextField = true;
			controller.textField = infoText;

			buttons.Add(obj);
		}
	}

	/// <summary>
	/// Adds apparel to the displayed inventory menu.
	/// </summary>
	private void ShowApparel() {
		InventoryItem<Apparel>[] apparel = Inventory.INSTANCE.GetApparel();

		foreach (InventoryItem<Apparel> item in apparel) {
			GameObject obj = Instantiate(buttonPrefab, content);
			SkillButtonController controller = obj.GetComponent<SkillButtonController>();
			controller.SetData(item);
			controller.useTextField = true;
			controller.textField = infoText;

			buttons.Add(obj);
		}
	}

	/// <summary>
	/// Adds the accessories to the displayed inventory menu.
	/// </summary>
	private void ShowAccessories() {
		InventoryItem<Accessory>[] accessories = Inventory.INSTANCE.GetAccessories();

		foreach (InventoryItem<Accessory> item in accessories) {
			GameObject obj = Instantiate(buttonPrefab, content);
			SkillButtonController controller = obj.GetComponent<SkillButtonController>();
			controller.SetData(item);
			controller.useTextField = true;
			controller.textField = infoText;

			buttons.Add(obj);
		}
	}
	#endregion

	/// <summary>
	/// Destroys all inventory buttons.
	/// </summary>
	private void DestroyButtons() {
		if (buttons != null && buttons.Count > 0)
			foreach (GameObject obj in buttons)
				Destroy(obj);
	}
}
