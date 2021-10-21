using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum AccessoryEffect {
    None,
    ExtraTurn,
    DamageReductionPercent,
    DamageReductionFlat,
    HPRecoverPerTurn,
    SPRecoverPerTurn,
    Fury,
    Thermal,
    ExtraHit
}

public enum AccessoryType {
    Ring,
    Optic,
    Laser
}

[CreateAssetMenu(fileName = "NewAccessory", menuName = "RPG Element/Item/Accessory")]
public class Accessory : Item {
    public AccessoryEffect effect;
    public AccessoryType accessoryType;
    public MeleeWeaponType[] compatibleMelee;
    public MagicWeaponType[] compatibleMagic;
    public RangedWeaponType[] compatibleRanged;
    public int effectParameter;
    public int maxHPChange;
    public int maxSPChange;
    public int strChange;
    public int magChange;
    public int dexChange;
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

        stringBuilder.Append(GetRichText("Max HP", maxHPChange))
            .Append(GetRichText("Max SP", maxSPChange))
            .Append(GetRichText("STR", strChange))
            .Append(GetRichText("MAG", magChange))
            .Append(GetRichText("DEX", dexChange))
            .Append(GetRichText("DEF", defChange))
            .Append(GetRichText("RES", resChange))
            .Append(GetRichText("ARM", armChange))
            .Append(GetRichText("AGI", agiChange))
            .Append(GetRichText(effect));

        return stringBuilder.ToString();
	}

    private string GetRichText(string label, int number) {
        if (number == 0)
            return "";
        else if (number < 0)
            return label + " <color=red>" + number + "<color=white> ";
        else
            return label + " <color=green>" + number + "<color=white> ";
    }

    private string GetRichText(AccessoryEffect effect) {
        switch (effect) {
            case AccessoryEffect.ExtraTurn:
                return "Extra Turn";
            case AccessoryEffect.DamageReductionPercent:
                return effectParameter + "% Damage Reduction";
            case AccessoryEffect.DamageReductionFlat:
                return "-" + effectParameter + " Damage Taken";
            case AccessoryEffect.HPRecoverPerTurn:
                return effectParameter + " HP/turn";
            case AccessoryEffect.SPRecoverPerTurn:
                return effectParameter + " SP/turn";
            case AccessoryEffect.Fury:
                return "Fury";
            case AccessoryEffect.Thermal:
                return "Thermal";
            case AccessoryEffect.ExtraHit:
                return effectParameter + "% Extra Hit";
            default:
                return "";
		}
	}

    public override int GetNumber() {
        return 1;
    }
}
