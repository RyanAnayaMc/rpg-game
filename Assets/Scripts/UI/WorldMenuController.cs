using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class WorldMenuController : MonoBehaviour {
	#region Fields
	[SerializeField] private GameObject mapHUD;
	[SerializeField] private GameObject popupMenu;
	[SerializeField] private InventoryMenu inventoryMenu;
	[SerializeField] private StatusDisplay statusMenu;
	[SerializeField] private EquipmentMenu equipMenu;
	[SerializeField] private int popupMenuOpenOffset = 362;
	private bool isMenuOpen;
	private IMenuWindow currentWindow;
	[SerializeField] private CanvasGroup worldUI;
	private Task _running;
	#endregion

	public void Start() {
		inventoryMenu.gameObject.SetActive(false);
		statusMenu.gameObject.SetActive(false);
		equipMenu.gameObject.SetActive(false);
	}

	public async void Update() {
		if (Input.GetButtonDown("Menu")) {
			if (!isMenuOpen && !InputMovement.IsPlayerLocked()) {
				_running = OpenMenu();
				await _running;
			} else if (isMenuOpen && InputMovement.IsPlayerLocked()) {
				_running = CloseMenu();
				await _running;
			}
		}
	}

	#region Menu Buttons
	public void OnInventoryButton() {
		OnButton(inventoryMenu);
	}

	public void OnStatusButton() {
		OnButton(statusMenu);
	}

	public void OnEquipmentButton() {
		OnButton(equipMenu);
	}

	private void OnButton(IMenuWindow window) {
		if (IsTaskRunning()) return;

		if (window.IsOpen()) {
			window.Close();
			currentWindow = null;
		} else {
			if (currentWindow != null) {
				currentWindow.Close();
			}
			window.Open();
			currentWindow = window;
		}
	}

	public void OnMenuButton() {
		SceneManager.LoadScene("MainMenu");
	}

	public void OnCloseButton() {
		CloseMenu().GetAwaiter().GetResult();
	}

	public async Task CloseMenu() {
		if (IsTaskRunning()) return;
		float positionDelta = (float) popupMenuOpenOffset / 10; ;

		_ = FadeOut();

		for (int i = 0; i < 10; i++) {

			Vector3 newMapPosition = mapHUD.transform.localPosition;
			newMapPosition.x -= positionDelta;
			Vector3 newMenuPosition = popupMenu.transform.localPosition;
			newMenuPosition.x -= positionDelta;
			mapHUD.transform.localPosition = newMapPosition;
			popupMenu.transform.localPosition = newMenuPosition;
			await Task.Delay(10);
		}

		InputMovement.UnlockPlayer();
		isMenuOpen = false;

		if (currentWindow != null)
			_ = currentWindow.Close();
	}
	#endregion

	#region Open and Close
	private async Task FadeOut() {
		worldUI.alpha = 1;

		while (worldUI.alpha > 0) {
			worldUI.alpha -= 0.1f;
			await Task.Delay(10);
		}
	}

	private async Task FadeIn() {
		await Utilities.WaitUntil(() => gameObject.activeSelf);
		worldUI.alpha = 0;

		while (worldUI.alpha < 1) {
			worldUI.alpha += 0.1f;
			await Task.Delay(10);
		}
	}

	private async Task OpenMenu() {
		if (IsTaskRunning()) return;

		_ = FadeIn();
		InputMovement.LockPlayer();
		isMenuOpen = true;

		float positionDelta = (float) popupMenuOpenOffset / 10; ;

		for (int i = 0; i < 10; i++) {

			Vector3 newMapPosition = mapHUD.transform.localPosition;
			newMapPosition.x += positionDelta;
			Vector3 newMenuPosition = popupMenu.transform.localPosition;
			newMenuPosition.x += positionDelta;
			mapHUD.transform.localPosition = newMapPosition;
			popupMenu.transform.localPosition = newMenuPosition;
			await Task.Delay(10);
		}
	}

	private bool IsTaskRunning() {
		if (_running == null)
			return false;
		else {
			if (_running.IsCompleted) {
				_running = null;
				return false;
			} else
				return true;
		}
	}
	#endregion
}
