using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum ApparelType {
    Armor,
    Clothing,
    Bulletproof
}

[CreateAssetMenu(fileName = "NewApparel", menuName = "RPG Element/Item/Apparel")]
public class Apparel : Item {
    public ApparelType type;
    public int defChange;
    public int resChange;
    public int armChange;
    public int agiChange;
    public new string itemDescription {
        get {
            return base.itemDescription + "\n" + GetInfo();
        }
        set {
            base.itemDescription = value;
        }
    }

	public string GetInfo() {
        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.Append(GetRichText("DEF", defChange))
            .Append(GetRichText("RES", resChange))
            .Append(GetRichText("ARM", armChange))
            .Append(GetRichText("AGI", agiChange));

        return stringBuilder.ToString();
    }

	private string GetRichText(string label, int number) {
        if (number == 0)
            return "";
        else if (number < 0)
            return label + "<color=red>" + number + "<color=white> ";
        else
            return label + "<color=green>+" + number + "<color=white> ";
    }

    public override int GetNumber() {
        return 1;
    }

	public override string GetDescriptionText() {
		return base.GetDescriptionText() + " " + GetInfo();
	}
}
