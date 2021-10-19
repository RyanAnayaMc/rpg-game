using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemAtlas", menuName = "RPG Element/Item/Atlas")]
public class Atlas : ScriptableObject {
    private static Atlas atlas;
    public Consumable[] consumables;
    public Weapon[] weapons;
    public Apparel[] apparel;
    public Accessory[] accessories;
    public Skill[] skills;

	public void OnEnable() {
        atlas = Resources.Load<Atlas>(Paths.ATLAS_PATH);
    }

	public static int GetID(Consumable item) {
        for (int i = 0; i < atlas.consumables.Length; i++)
            if (atlas.consumables[i] == item)
                return i;
        return -1;
    }

    public static int GetID(Weapon item) {
        for (int i = 0; i < atlas.weapons.Length; i++)
            if (atlas.weapons[i] == item)
                return i;
        return -1;
    }

    public static int GetID(Apparel item) {
        for (int i = 0; i < atlas.apparel.Length; i++)
            if (atlas.apparel[i] == item)
                return i;
        return -1;
    }

    public static int GetID(Accessory item) {
        for (int i = 0; i < atlas.accessories.Length; i++)
            if (atlas.accessories[i] == item)
                return i;
        return -1;
    }

    public static int GetID(Skill skill) {
        for (int i = 0; i < atlas.skills.Length; i++)
            if (atlas.skills[i] == skill)
                return i;
        return -1;
    }

    public static Weapon GetWeapon(int id) {
        return atlas.weapons[id];
	}
}
