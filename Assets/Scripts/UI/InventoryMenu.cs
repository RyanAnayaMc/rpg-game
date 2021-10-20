using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryMenu : MonoBehaviour, IMenuWindow {
	public Transform content;
	public GameObject buttonPrefab;
	private CanvasGroup canvasGroup;
	[HideInInspector] public bool isOpen;
	private List<GameObject> buttons;
	public TMP_Text infoText;

	public void Start() {
		canvasGroup = GetComponent<CanvasGroup>();
	}

	public void Close() {
		StartCoroutine(fadeOut());
	}

	public void Open() {
		gameObject.SetActive(true);
		StartCoroutine(fadeIn());
		Inventory inv = Inventory.INSTANCE;
		InventoryItem<Consumable>[] consumables = inv.GetConsumables();
		InventoryItem<Weapon>[] weapons = inv.GetWeapons();
		InventoryItem<Apparel>[] apparel = inv.GetApparel();
		InventoryItem<Accessory>[] accessories = inv.GetAccessories();

		buttons = new List<GameObject>();

		foreach (InventoryItem<Consumable> item in consumables) {
			GameObject obj = Instantiate(buttonPrefab, content);
			SkillButtonController controller = obj.GetComponent<SkillButtonController>();
			controller.setData(item);
			controller.useTextField = true;
			controller.textField = infoText;

			buttons.Add(obj);
		}

		foreach (InventoryItem<Weapon> item in weapons) {
			GameObject obj = Instantiate(buttonPrefab, content);
			SkillButtonController controller = obj.GetComponent<SkillButtonController>();
			controller.setData(item);
			controller.useTextField = true;
			controller.textField = infoText;

			buttons.Add(obj);
		}

		foreach (InventoryItem<Apparel> item in apparel) {
			GameObject obj = Instantiate(buttonPrefab, content);
			SkillButtonController controller = obj.GetComponent<SkillButtonController>();
			controller.setData(item);
			controller.useTextField = true;
			controller.textField = infoText;

			buttons.Add(obj);
		}

		foreach (InventoryItem<Accessory> item in accessories) {
			GameObject obj = Instantiate(buttonPrefab, content);
			SkillButtonController controller = obj.GetComponent<SkillButtonController>();
			controller.setData(item);
			controller.useTextField = true;
			controller.textField = infoText;

			buttons.Add(obj);
		}
	}

	private IEnumerator fadeOut() {
		canvasGroup.alpha = 1;

		while (canvasGroup.alpha > 0) {
			canvasGroup.alpha -= 0.1f;
			yield return new WaitForSeconds(0.01f);
		}

		foreach (GameObject obj in buttons)
			Destroy(obj);

		isOpen = false;
		gameObject.SetActive(false);
	}

	private IEnumerator fadeIn() {
		yield return new WaitUntil(() => gameObject.activeSelf);
		canvasGroup.alpha = 0;

		while (canvasGroup.alpha < 1) {
			canvasGroup.alpha += 0.1f;
			yield return new WaitForSeconds(0.01f);
		}

		isOpen = true;
	}
}
