using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerUnit", menuName = "RPG Element/Unit/Player Unit")]
public class PlayerUnit : Unit {
	public GameObject meleePrefab;

	public GameObject magicPrefab;

	public GameObject rangedPrefab;
	public bool useBasicPrefabMode;
	public int xp;

	public new GameObject unitPrefab {
		get {
			if (useBasicPrefabMode)
				return base.unitPrefab;
			else {
				switch (weapon.atkType) {
					case AttackType.MELEE:
						return meleePrefab;
					case AttackType.MAGIC:
						return magicPrefab;
					case AttackType.RANGED:
						return rangedPrefab;
					default:
						return null;
				}
			}
		}
		set {
			base.unitPrefab = value;
		}
	}

}
