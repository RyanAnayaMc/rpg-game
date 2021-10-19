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
    public int weaponId;

    // TODO add more equipment

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
    /// The unit's maximum HP.
    /// </summary>
    public int maxHP;

    /// <summary>
    /// The unit's current SP. Consumed when using skills.
    /// Cannot be higher than maxSP.
    /// </summary>
    public int cSP;

    /// <summary>
    /// The unit's maximum SP.
    /// </summary>
    public int maxSP;

    /// <summary>
    /// The unit's strength. Determines melee damage.
    /// </summary>
    public int str;

    /// <summary>
    /// The unit's magic power. Determines magic damage.
    /// </summary>
    public int mag;

    /// <summary>
    /// The unit's dexterity. Determines melee and magic accuracy.
    /// Also determines ranged crit rate.
    /// </summary>
    public int dex;

    /// <summary>
    /// The unit's defense. Reduces incoming melee damage.
    /// </summary>
    public int def;

    /// <summary>
    /// The unit's resistance. Reduces incoming magic damage.
    /// </summary>
    public int res;

    /// <summary>
    /// The unit's armor. Reduces incoming ranged damage.
    /// </summary>
    public int arm;

    /// <summary>
    /// The unit's agility. Increases dodge chance against melee nad magic
    /// attacks. Also reduces ranged enemy's crit chance.
    /// </summary>
    public int agi;

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
