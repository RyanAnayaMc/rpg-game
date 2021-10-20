using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusDisplay : MonoBehaviour, IMenuWindow {
	public bool isOpen = false;
	public bool baseStats = true;
	public TMP_Text statsToggleButton;
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
	[SerializeField]
	private TMP_Text weaponType;
	[SerializeField]
	private TMP_Text weaponName;
	[SerializeField]
	private Image weaponIcon;
	[SerializeField]
	private TMP_Text apparelType;
	[SerializeField]
	private TMP_Text apparelName;
	[SerializeField]
	private Image apparelIcon;
	[SerializeField]
	private TMP_Text accessoryType;
	[SerializeField]
	private TMP_Text accessoryName;
	[SerializeField]
	private Image accessoryIcon;
	[SerializeField]
	private Sprite emptyApparelIcon;
	[SerializeField]
	private Sprite emptyAccessoryIcon;

	private void Awake() {
		player = PlayerUnit.INSTANCE;
		canvasGroup = GetComponent<CanvasGroup>();
	}

	public void toggleStats() {
		baseStats = !baseStats;

		if (baseStats)
			statsToggleButton.text = "<color=white>Base Stats";
		else
			statsToggleButton.text = "<color=green>Effective Stats";
	}

	public void Open() {
		gameObject.SetActive(true);
		StartCoroutine(fadeIn());
		SetData();
	}

	private void SetData() {
		faceField.sprite = player.characterFace;
		nameField.text = player.unitName;
		levelField.text = "Level " + player.level;
		xpField.text = player.xp + " / 100";
		
		if (baseStats) {
			hpField.text = "<color=white>" + player.cHP + " / " + player.maxHP;
			spField.text = "<color=white>" + player.cSP + " / " + player.maxSP;
			strField.text = "<color=white>" + player.str.ToString();
			magField.text = "<color=white>" + player.mag.ToString();
			dexField.text = "<color=white>" + player.dex.ToString();
			defField.text = "<color=white>" + player.def.ToString();
			resField.text = "<color=white>" + player.res.ToString();
			armField.text = "<color=white>" + player.arm.ToString();
			agiField.text = "<color=white>" + player.agi.ToString();
		} else {
			hpField.text = player.cHP + " / " + ((player.effMaxHP == player.maxHP) ? "<color=green>" : "<color=white>") + player.effMaxHP;
			spField.text = player.cSP + " / " + ((player.effMaxSP == player.maxSP) ? "<color=green>" : "<color=white>") + player.effMaxSP;
			strField.text = ((player.effStr == player.str) ? "<color=green>" : "<color=white>") + player.effStr.ToString();
			magField.text = ((player.effMag == player.mag) ? "<color=green>" : "<color=white>") + player.effMag.ToString();
			dexField.text = ((player.effDex == player.dex) ? "<color=green>" : "<color=white>") + player.effDex.ToString();
			defField.text = ((player.effDef == player.def) ? "<color=green>" : "<color=white>") + player.effDef.ToString();
			resField.text = ((player.effRes == player.res) ? "<color=green>" : "<color=white>") + player.effRes.ToString();
			armField.text = ((player.effArm == player.arm) ? "<color=green>" : "<color=white>") + player.effArm.ToString();
			agiField.text = ((player.effAgi == player.agi) ? "<color=green>" : "<color=white>") + player.effAgi.ToString();
		}

		// Draw weapon info
		weaponIcon.sprite = player.weapon.itemIcon;
		weaponName.text = player.weapon.itemName;
		weaponType.text = "<color=red>" + player.weapon.atkType.ToString() + "<color=white>\n";
		switch (player.weapon.atkType) {
			case AttackType.Melee:
				weaponType.text += player.weapon.meleeType.ToString();
				break;
			case AttackType.Magic:
				weaponType.text += player.weapon.magicType.ToString();
				break;
			case AttackType.Ranged:
				weaponType.text += player.weapon.rangedType.ToString();
				break;
		}

		// Draw apparel info
		if (player.apparel is null) {
			apparelIcon.sprite = emptyApparelIcon;
			apparelName.text = "<color=red>No Apparel";
			apparelType.text = "None";
		} else {
			apparelIcon.sprite = player.apparel.itemIcon;
			apparelName.text = player.apparel.itemName;
			apparelType.text = player.apparel.type.ToString();
		}

		// Draw accessory info
		if (player.accessory is null) {
			accessoryIcon.sprite = emptyAccessoryIcon;
			accessoryName.text = "<color=red>No Accessory";
			accessoryType.text = "None";

		} else {
			accessoryIcon.sprite = player.accessory.itemIcon;
			accessoryName.text = player.accessory.itemName;
			accessoryType.text = player.accessory.accessoryType.ToString();
		}
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
