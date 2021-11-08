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

	public static void LoadAtlas() {
        atlas = Resources.Load<Atlas>(Paths.ATLAS_PATH);
    }

	public static int GetID(Consumable item) {
        if (atlas == null)
            LoadAtlas();
        for (int i = 0; i < atlas.consumables.Length; i++)
            if (atlas.consumables[i] == item)
                return i;
        return -1;
    }

    public static int GetID(Weapon item) {
        if (atlas == null)
            LoadAtlas();
        for (int i = 0; i < atlas.weapons.Length; i++)
            if (atlas.weapons[i] == item)
                return i;
        return -1;
    }

    public static int GetID(Apparel item) {
        if (atlas == null)
            LoadAtlas();
        for (int i = 0; i < atlas.apparel.Length; i++)
            if (atlas.apparel[i] == item)
                return i;
        return -1;
    }

    public static int GetID(Accessory item) {
        if (atlas == null)
            LoadAtlas();
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

    public static Consumable GetConsumable(int id) {
        if (atlas == null)
            LoadAtlas();
        if (id < 0 || id >= atlas.consumables.Length)
            return null;
        return atlas.consumables[id];
	}

    public static Weapon GetWeapon(int id) {
        if (atlas == null)
            LoadAtlas();
        if (id < 0 || id >= atlas.weapons.Length)
            return null;
        return atlas.weapons[id];
	}

    public static Apparel GetApparel(int id) {
        if (atlas == null)
            LoadAtlas();
        if (id < 0 || id >= atlas.apparel.Length)
            return null;
        return atlas.apparel[id];
	}

    public static Accessory GetAccessory(int id) {
        if (atlas == null)
            LoadAtlas();
        if (id < 0 || id >= atlas.accessories.Length)
            return null;
        return atlas.accessories[id];
	}
}
