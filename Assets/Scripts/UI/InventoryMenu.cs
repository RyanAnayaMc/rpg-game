using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour, IMenuWindow {
	public Transform content;
	public GameObject buttonPrefab;
	private CanvasGroup canvasGroup;
	public bool isOpen;
	private List<GameObject> buttons;

	public void Start() {
		canvasGroup = GetComponent<CanvasGroup>();
	}

	public void Close() {
		StartCoroutine(fadeOut());
	}

	public void Open() {
		StartCoroutine(fadeIn());
		Inventory inv = Inventory.INSTANCE;
		InventoryItem[] consumables = inv.GetConsumables();
		Weapon[] weapons = inv.GetWeapons();
		Apparel[] apparel = inv.GetApparel();
		Accessory[] accessories = inv.GetAccessories();

		buttons = new List<GameObject>();

		foreach (InventoryItem item in consumables) {
			GameObject obj = Instantiate(buttonPrefab, content);
			SkillButtonController controller = obj.GetComponent<SkillButtonController>();
			controller.setData(item);

			buttons.Add(obj);
		}

		foreach (Weapon item in weapons) {
			GameObject obj = Instantiate(buttonPrefab, content);
			SkillButtonController controller = obj.GetComponent<SkillButtonController>();
			controller.setData(item);

			buttons.Add(obj);
		}

		foreach (Apparel item in apparel) {
			GameObject obj = Instantiate(buttonPrefab, content);
			SkillButtonController controller = obj.GetComponent<SkillButtonController>();
			controller.setData(item);

			buttons.Add(obj);
		}

		foreach (Accessory item in accessories) {
			GameObject obj = Instantiate(buttonPrefab, content);
			SkillButtonController controller = obj.GetComponent<SkillButtonController>();
			controller.setData(item);

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
