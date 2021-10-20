using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerUnit", menuName = "RPG Element/Unit/Player Unit")]
public class PlayerUnit : Unit {
	private static PlayerUnit _instance;
	public static PlayerUnit INSTANCE {
		get {
			if (_instance == null)
				_instance = Resources.Load<PlayerUnit>("Player/Unit");

			return _instance;
		}
	}

	public Sprite characterFace;

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
					case AttackType.Melee:
						return meleePrefab;
					case AttackType.Magic:
						return magicPrefab;
					case AttackType.Ranged:
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
