using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMenuController : MonoBehaviour {
    public GameObject mapHUD;
    public GameObject popupMenu;
	public StatusDisplay statusMenu;
    public int popupMenuOpenOffset = 362;
	public int extraMenuOffset = 438;
	private bool isMenuOpen;
	private Coroutine _running;
	[SerializeField]
	private Button equipmentButton;
	[SerializeField]
	private Button inventoryButton;
	[SerializeField]
	private Button skillsButton;
	[SerializeField]
	private Button statusButton;
	[SerializeField]
	private Button saveButton;
	[SerializeField]
	private Button exitButton;
	[SerializeField]
	private Button closeMenuButton;

	public void Start() {
		equipmentButton.onClick.AddListener(() => {

		});

		inventoryButton.onClick.AddListener(() => {

		});

		skillsButton.onClick.AddListener(() => {

		});

		statusButton.onClick.AddListener(() => {

		});

		saveButton.onClick.AddListener(() => {

		});

		exitButton.onClick.AddListener(() => {

		});

		closeMenuButton.onClick.AddListener(() => {

		});
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

	#region Open and Close
	private IEnumerator openMenu() {
		Debug.Log("menu open");
		CharacterMovementController.isPlayerLocked = true;
		isMenuOpen = true;

		float positionDelta = (float) 362 / 10; ;

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
		float positionDelta = (float) 362 / 10; ;

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
		_running = null;
	}

	private void StartCoroutineIfNoneRunning(IEnumerator coroutine) {
		if (_running == null)
			_running = StartCoroutine(coroutine);
	}
	#endregion
}
