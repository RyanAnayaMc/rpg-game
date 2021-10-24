using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

#pragma warning disable IDE0044, IDE0051

public class StatusDisplay : MonoBehaviour, IMenuWindow {
	#region Fields
	private const string kWhite = "<color=white>";
	private const string kGreen = "<color=green>";
	private const string kRed = "<color=red>";

	private bool isOpen = false;
	private bool baseStats = true;
	[SerializeField]
	private TMP_Text statsToggleButton;
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
	#endregion

	private void Awake() {
		player = PlayerUnit.INSTANCE;
		canvasGroup = GetComponent<CanvasGroup>();
	}

	#region IMenuWindow implementation
	public bool IsOpen() {
		return isOpen;
	}

	public async Task Open() {
		DrawData();
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
	}
	#endregion

	public void ToggleStats() {
		// Toggle showing base stats and effective stats
		baseStats = !baseStats;

		if (baseStats)
			statsToggleButton.text = kWhite + "Base Stats";
		else
			statsToggleButton.text = kGreen + "Effective Stats";

		DrawData();
	}

	#region Information Drawing
	private void DrawData() {
		DrawBasicInfo();

		if (baseStats)
			DrawBaseStats();
		else
			DrawEffectiveStats();

		// Draw gear info
		DrawWeaponInfo();
		DrawApparelInfo();
		DrawAccessoryInfo();

	}

	private void DrawBasicInfo() {
		faceField.sprite = player.characterFace;
		nameField.text = player.unitName;
		levelField.text = "Level " + player.level;
		xpField.text = player.xp + " / 100";
	}

	private void DrawBaseStats() {
		hpField.text = kWhite + player.cHP + " / " + player.maxHP;
		spField.text = kWhite + player.cSP + " / " + player.maxSP;
		strField.text = kWhite + player.str.ToString();
		magField.text = kWhite + player.mag.ToString();
		dexField.text = kWhite + player.dex.ToString();
		defField.text = kWhite + player.def.ToString();
		resField.text = kWhite + player.res.ToString();
		armField.text = kWhite + player.arm.ToString();
		agiField.text = kWhite + player.agi.ToString();
	}

	private void DrawEffectiveStats() {
		string hpText;
		string spText;
		string strText;
		string magText;
		string dexText;
		string defText;
		string resText;
		string armText;
		string agiText;

		// Get fancy HP text
		if (player.effMaxHP == player.maxHP) {
			hpText = player.cHP + " / " + player.maxHP;
		} else {
			int diff = player.effMaxHP - player.maxHP;
			hpText = player.cHP + " / " + (diff > 0 ? kGreen : kRed) + player.effMaxHP + " (" + diff + ")";
		}

		// Get fancy SP text
		if (player.effMaxSP == player.maxSP) {
			spText = player.cSP + " / " + player.maxSP;
		} else {
			int diff = player.effMaxSP - player.maxSP;
			spText = player.cSP + " / " + (diff > 0 ? kGreen : kRed) + player.effMaxSP + " (" + diff + ")";
		}

		// Get fancy str text
		if (player.effStr == player.str)
			strText = player.str.ToString();
		else {
			int diff = player.effStr - player.str;
			strText = player.effStr + (diff > 0 ? kGreen : kRed) + " (" + diff + ")";
		}

		// Get fancy mag text
		if (player.effMag == player.mag)
			magText = player.mag.ToString();
		else {
			int diff = player.effMag - player.mag;
			magText = player.effMag + (diff > 0 ? kGreen : kRed) + " (" + diff + ")";
		}

		// Get fancy dex text
		if (player.effDex == player.dex)
			dexText = player.dex.ToString();
		else {
			int diff = player.effDex - player.dex;
			dexText = player.effDex + (diff > 0 ? kGreen : kRed) + " (" + diff + ")";
		}

		// Get fancy def text
		if (player.effDef == player.def)
			defText = player.def.ToString();
		else {
			int diff = player.effDef - player.def;
			defText = player.effDef + (diff > 0 ? kGreen : kRed) + " (" + diff + ")";
		}

		// Get fancy res text
		if (player.effRes == player.res)
			resText = player.res.ToString();
		else {
			int diff = player.effRes - player.res;
			resText = player.effRes + (diff > 0 ? kGreen : kRed) + " (" + diff + ")";
		}

		// Get fancy arm text
		if (player.effArm == player.arm)
			armText = player.arm.ToString();
		else {
			int diff = player.effArm - player.arm;
			armText = player.effArm + (diff > 0 ? kGreen : kRed) + " (" + diff + ")";
		}

		// Get fancy agi text
		if (player.effAgi == player.agi)
			agiText = player.agi.ToString();
		else {
			int diff = player.effAgi - player.agi;
			agiText = player.effAgi + (diff > 0 ? kGreen : kRed) + " (" + diff + ")";
		}

		hpField.text = hpText;
		spField.text = spText;
		strField.text = strText;
		magField.text = magText;
		dexField.text = dexText;
		defField.text = defText;
		resField.text = resText;
		armField.text = armText;
		agiField.text = agiText;
	}

	private void DrawWeaponInfo() {
		weaponIcon.sprite = player.weapon.itemIcon;
		weaponName.text = player.weapon.itemName;
		weaponType.text = kRed + player.weapon.atkType.ToString() + kWhite + "\n";
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
	}

	private void DrawApparelInfo() {
		if (player.apparel is null) {
			apparelIcon.sprite = emptyApparelIcon;
			apparelName.text = kRed + "No Apparel";
			apparelType.text = "None";
		} else {
			apparelIcon.sprite = player.apparel.itemIcon;
			apparelName.text = player.apparel.itemName;
			apparelType.text = player.apparel.type.ToString();
		}
	}

	private void DrawAccessoryInfo() {
		if (player.accessory is null) {
			accessoryIcon.sprite = emptyAccessoryIcon;
			accessoryName.text = kRed + "No Accessory";
			accessoryType.text = "None";

		} else {
			accessoryIcon.sprite = player.accessory.itemIcon;
			accessoryName.text = player.accessory.itemName;
			accessoryType.text = player.accessory.accessoryType.ToString();
		}
	}
	#endregion
}
