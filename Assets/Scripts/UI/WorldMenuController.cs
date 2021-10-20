using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorldMenuController : MonoBehaviour {
    public GameObject mapHUD;
    public GameObject popupMenu;
	public InventoryMenu inventoryMenu;
	public StatusDisplay statusMenu;
    public int popupMenuOpenOffset = 362;
	private bool isMenuOpen;
	private IMenuWindow currentWindow;
	[SerializeField] private CanvasGroup worldUI;

	private Coroutine _running;

	public void Start() {
		inventoryMenu.gameObject.SetActive(false);
		statusMenu.gameObject.SetActive(false);
	}

	public void Update() {
		if (Input.GetButtonDown("Menu")) {
			Debug.Log("menu button");
			Debug.Log(isMenuOpen + " " + CharacterMovementController.isPlayerLocked);
			if (!isMenuOpen && !CharacterMovementController.isPlayerLocked)
				StartCoroutineIfNoneRunning(openMenu());
			else if (isMenuOpen && CharacterMovementController.isPlayerLocked)
				StartCoroutineIfNoneRunning(closeMenu());
		}
	}

	#region Menu Buttons
	public void onInventoryButton() {
		if (_running == null) {
			if (inventoryMenu.isOpen) {
				inventoryMenu.Close();
				currentWindow = null;
			} else {
				if (currentWindow != null) {
					currentWindow.Close();
				}

				inventoryMenu.Open();
				currentWindow = inventoryMenu;
			}
		}
	}

	public void onStatusButton() {
		if (_running == null) {
			if (statusMenu.isOpen) {
				statusMenu.Close();
				currentWindow = null;
			} else {
				if (currentWindow != null) {
					currentWindow.Close();
				}

				statusMenu.Open();
				currentWindow = statusMenu;
			}
		}
	}

	public void onMenuButton() {
		SceneManager.LoadScene("MainMenu");
	}

	public void onCloseButton() {
		StartCoroutineIfNoneRunning(closeMenu());
	}
	#endregion

	#region Open and Close
	private IEnumerator fadeOut() {
		worldUI.alpha = 1;

		while (worldUI.alpha > 0) {
			worldUI.alpha -= 0.1f;
			yield return new WaitForSeconds(0.01f);
		}

		_running = null;
	}

	private IEnumerator fadeIn() {
		yield return new WaitUntil(() => gameObject.activeSelf);
		worldUI.alpha = 0;

		while (worldUI.alpha < 1) {
			worldUI.alpha += 0.1f;
			yield return new WaitForSeconds(0.01f);
		}

		_running = null;
	}

	private IEnumerator openMenu() {
		StartCoroutineIfNoneRunning(fadeOut());
		Debug.Log("menu open");
		CharacterMovementController.isPlayerLocked = true;
		isMenuOpen = true;

		float positionDelta = (float) popupMenuOpenOffset / 10; ;

		for (int i = 0; i < 10; i++) {

			Vector3 newMapPosition = mapHUD.transform.localPosition;
			newMapPosition.x += positionDelta;
			Vector3 newMenuPosition = popupMenu.transform.localPosition;
			newMenuPosition.x += positionDelta;
			mapHUD.transform.localPosition = newMapPosition;
			popupMenu.transform.localPosition = newMenuPosition;
			yield return new WaitForSeconds(0.01f);
		}

		_running = null;
	}

	private IEnumerator closeMenu() {
		Debug.Log("menu close");
		float positionDelta = (float) popupMenuOpenOffset / 10; ;

		for (int i = 0; i < 10; i++) {

			Vector3 newMapPosition = mapHUD.transform.localPosition;
			newMapPosition.x -= positionDelta;
			Vector3 newMenuPosition = popupMenu.transform.localPosition;
			newMenuPosition.x -= positionDelta;
			mapHUD.transform.localPosition = newMapPosition;
			popupMenu.transform.localPosition = newMenuPosition;
			yield return new WaitForSeconds(0.01f);
		}

		CharacterMovementController.isPlayerLocked = false;
		isMenuOpen = false;

		if (currentWindow != null) {
			currentWindow.Close();
		}

		_running = StartCoroutine(fadeIn());
	}

	private void StartCoroutineIfNoneRunning(IEnumerator coroutine) {
		if (_running == null)
			_running = StartCoroutine(coroutine);
	}
	#endregion
}
