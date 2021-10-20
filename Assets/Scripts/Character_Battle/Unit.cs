using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType {
    MELEE,
    MAGIC,
    RANGED,
    ALL
}

/// <summary>
/// Basis for a playable or enemy Unit that can see combat.
/// </summary>
[CreateAssetMenu(fileName = "NewGenericUnit", menuName = "RPG Element/Unit/GenericUnit")]
public class Unit : ScriptableObject {
    /// <summary>
    /// The Prefab for the Unit.
    /// </summary>
    public GameObject unitPrefab;

    /// <summary>
    /// The unit's name.
    /// </summary>
    public string unitName;

    public AttackType weaponType;

    /// <summary>
    /// The sprite for the unit's icon in menus.
    /// </summary>
    public Sprite unitIcon;

    /// <summary>
    /// The unit's equipped <see cref="Weapon"/>.
    /// </summary>
    public Weapon weapon {
        get {
            return Atlas.GetWeapon(weaponId);
		}
        set {
            weaponId = Atlas.GetID(value);
		}
	}

    /// <summary>
    /// The unit's weapon ID.
    /// </summary>
    public int weaponId = -1;

    /// <summary>
    /// The unit's equipped apparel item.
    /// </summary>
    public Apparel apparel {
        get {
            return Atlas.GetApparel(apparelID);
		}
        set {
            apparelID = Atlas.GetID(value);
		}
	}

    /// <summary>
    /// The ID of the unit's equipped apparel.
    /// </summary>
    public int apparelID = -1;

    /// <summary>
    /// The unit's equipped accessory.
    /// </summary>
    public Accessory accessory {
        get {
            return Atlas.GetAccessory(accessoryID);
		}
        set {
            accessoryID = Atlas.GetID(value);
		}
	}

    public int accessoryID = -1;

    /// <summary>
    /// The unit's level.
    /// </summary>
    public int level;

    /// <summary>
    /// The unit's current HP. Unit dies if it is depleted.
    /// Cannot be higher than maxHP.
    /// </summary>
    public int cHP;

    /// <summary>
    /// The unit's base maximum HP.
    /// </summary>
    public int maxHP;

    /// <summary>
    /// The unit's current SP. Consumed when using skills.
    /// Cannot be higher than maxSP.
    /// </summary>
    public int cSP;

    /// <summary>
    /// The unit's base maximum SP.
    /// </summary>
    public int maxSP;

    /// <summary>
    /// The unit's base strength. Determines melee damage.
    /// </summary>
    public int str;

    /// <summary>
    /// The unit's base magic power. Determines magic damage.
    /// </summary>
    public int mag;

    /// <summary>
    /// The unit's base dexterity. Determines melee and magic accuracy.
    /// Also determines ranged crit rate.
    /// </summary>
    public int dex;

    /// <summary>
    /// The unit's base defense. Reduces incoming melee damage.
    /// </summary>
    public int def;

    /// <summary>
    /// The unit's base resistance. Reduces incoming magic damage.
    /// </summary>
    public int res;

    /// <summary>
    /// The unit's base armor. Reduces incoming ranged damage.
    /// </summary>
    public int arm;

    /// <summary>
    /// The unit's base agility. Increases dodge chance against melee nad magic
    /// attacks. Also reduces ranged enemy's crit chance.
    /// </summary>
    public int agi;

    /// <summary>
    /// The unit's effective maximum HP. Takes into account equipped accessory.
    /// </summary>
    public int effMaxHP {
        get {
            return maxHP +  (!(accessory is null) ? accessory.maxHPChange : 0);
		}
	}

    /// <summary>
    /// The unit's effective maximum SP. Takes into account equipped accessory.
    /// </summary>
    public int effMaxSP {
        get {
            return maxSP + (!(accessory is null) ? accessory.maxSPChange : 0);
        }
	}

    /// <summary>
    /// The unit's effective strength. Takes into account equipped accessory.
    /// </summary>
    public int effStr {
        get {
            return str + (!(accessory is null) ? accessory.strChange : 0);
        }
    }

    /// <summary>
    /// The unit's effective magic power. Takes into account equipped accessory.
    /// </summary>
    public int effMag {
        get {
            return mag + (!(accessory is null) ? accessory.magChange : 0);
        }
    }

    /// <summary>
    /// The unit's effective dexterity. Takes into account equipped accessory.
    /// </summary>
    public int effDex {
        get {
            return str + (!(accessory is null) ? accessory.dexChange : 0);
        }
    }

    /// <summary>
    /// The unit's effective defense. Takes into account equipped apparel and accessory.
    /// </summary>
    public int effDef {
        get {
            return def + (!(accessory is null) ? accessory.defChange : 0) + (!(apparel is null) ? apparel.defChange : 0);
        }
    }

    /// <summary>
    /// The unit's effective resistance. Takes into account equipped apparel and accessory.
    /// </summary>
    public int effRes {
        get {
            return res + (!(accessory is null) ? accessory.resChange : 0) + (!(apparel is null) ? apparel.resChange : 0);
        }
    }

    /// <summary>
    /// The unit's effective armor. Takes into account equipped apparel and accessory.
    /// </summary>
    public int effArm {
        get {
            return arm + (!(accessory is null) ? accessory.armChange : 0) + (!(apparel is null) ? apparel.armChange : 0);
        }
    }

    /// <summary>
    /// The unit's effective agility. Takes into account equipped apparel and accessory.
    /// </summary>
    public int effAgi {
        get {
            return agi + (!(accessory is null) ? accessory.agiChange : 0) + (!(apparel is null) ? apparel.agiChange : 0);
        }
    }

    /// <summary>
    /// Whether or not the unit is defending. If defending, incoming damage
    /// is halved.
    /// </summary>
    public bool isDefending = false; // Whether or not the unit is defending

    /// <summary>
    /// A list of the unit's skills.
    /// </summary>
    public List<Skill> skills; // Skills the Unit can use in combat

    public bool showStats;
}
