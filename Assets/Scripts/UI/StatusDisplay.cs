using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusDisplay : MonoBehaviour {
	public bool isOpen = false;
	private CanvasGroup canvasGroup;
	private PlayerUnit player;
	[SerializeField]
	private Image faceField;
	[SerializeField]
	private TMP_Text nameField;
	[SerializeField]
	private TMP_Text levelField;
	[SerializeField]
	private TMP_Text xpField;
	[SerializeField]
	private TMP_Text hpField;
	[SerializeField]
	private TMP_Text spField;
	[SerializeField]
	private TMP_Text strField;
	[SerializeField]
	private TMP_Text magField;
	[SerializeField]
	private TMP_Text dexField;
	[SerializeField]
	private TMP_Text defField;
	[SerializeField]
	private TMP_Text resField;
	[SerializeField]
	private TMP_Text armField;
	[SerializeField]
	private TMP_Text agiField;

	private void Start() {
		player = PlayerUnit.INSTANCE;
		canvasGroup = GetComponent<CanvasGroup>();
	}

	public void Open() {
		StartCoroutine(fadeIn());
		faceField.sprite = player.characterFace;
		nameField.text = player.unitName;
		levelField.text = "Level " + player.level;
		xpField.text = player.xp + " / 100";
		hpField.text = player.cHP + " / " + player.maxHP;
		spField.text = player.cSP + " / " + player.maxSP;
		strField.text = player.str.ToString();
		magField.text = player.mag.ToString();
		dexField.text = player.dex.ToString();
		defField.text = player.def.ToString();
		resField.text = player.res.ToString();
		armField.text = player.arm.ToString();
		agiField.text = player.agi.ToString();
	}

	public void Close() {
		StartCoroutine(fadeOut());
	}

	private IEnumerator fadeOut() {
		canvasGroup.alpha = 1;

		while (canvasGroup.alpha > 0) {
			canvasGroup.alpha -= 0.1f;
			yield return new WaitForSeconds(0.01f);
		}
	}

	private IEnumerator fadeIn() {
		canvasGroup.alpha = 0;

		while (canvasGroup.alpha < 1) {
			canvasGroup.alpha += 0.1f;
			yield return new WaitForSeconds(0.01f);
		}
	}
}
