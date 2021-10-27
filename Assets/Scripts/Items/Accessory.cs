using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#pragma warning disable IDE0090, IDE1006

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
    public List<MeleeWeaponType> compatibleMelee;
    public List<MagicWeaponType> compatibleMagic;
    public List<RangedWeaponType> compatibleRanged;
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

    public (bool isCompatible, string errorMessage) CheckCompatibility(Weapon weapon) {
        string msg;

        switch (weapon.atkType) {
            case AttackType.Melee:
                if (compatibleMelee.Count == 0) {
                    msg = itemName + " is not compatible with Melee weapons.";
                    return (false, msg);
                } else {
                    MeleeWeaponType type = weapon.meleeType;
                    if (compatibleMelee.Contains(type))
                        return (true, "");
                    else {
                        msg = itemName + " is not compatible with " + type.ToString() + "s.";
                        return (false, msg);
					}
				}
            case AttackType.Magic:
                if (compatibleMagic.Count == 0) {
                    msg = itemName + " is not compatible with Magic weapons.";
                    return (false, msg);
                } else {
                    MagicWeaponType type = weapon.magicType;
                    if (compatibleMagic.Contains(type))
                        return (true, "");
                    else {
                        msg = itemName + " is not compatible with " + type.ToString() + "s.";
                        return (false, msg);
                    }
                }
            case AttackType.Ranged:
                if (compatibleRanged.Count == 0) {
                    msg = itemName + " is not compatible with Ranged weapons.";
                    return (false, msg);
                } else {
                    RangedWeaponType type = weapon.rangedType;
                    if (compatibleRanged.Contains(type))
                        return (true, "");
                    else {
                        msg = itemName + " is not compatible with " + type.ToString() + "s.";
                        return (false, msg);
                    }
                }
        }

		return (false, "error");
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
		return effect switch {
			AccessoryEffect.ExtraTurn => "Extra Turn",
			AccessoryEffect.DamageReductionPercent => effectParameter + "% Damage Reduction",
			AccessoryEffect.DamageReductionFlat => "-" + effectParameter + " Damage Taken",
			AccessoryEffect.HPRecoverPerTurn => effectParameter + " HP/turn",
			AccessoryEffect.SPRecoverPerTurn => effectParameter + " SP/turn",
			AccessoryEffect.Fury => "Fury",
			AccessoryEffect.Thermal => "Thermal",
			AccessoryEffect.ExtraHit => effectParameter + "% Extra Hit",
			_ => "",
		};
	}

    public override int GetNumber() {
        return 1;
    }
}
