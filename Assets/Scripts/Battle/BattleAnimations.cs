using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimations {
    private static BattleAnimations _instance;
    public static BattleAnimations INSTANCE {
        get {
            if (_instance == null)
                _instance = new BattleAnimations();

            return _instance;
		}
	}

    [SerializeField] private GameObject _smallExplosion;
    [SerializeField] private GameObject _bigExplosion;
    [SerializeField] private GameObject _bloodHit;

    public GameObject SmallExplosion {
        get { return _smallExplosion; }
	}

    public GameObject BigExplosion {
        get { return _bigExplosion; }
	}

    public GameObject BloodHit {
		get { return _bloodHit; }
	}

    private BattleAnimations() {
        _smallExplosion = Resources.Load<GameObject>(Paths.SMALL_EXPLOSION_PATH);
        _bigExplosion = Resources.Load<GameObject>(Paths.BIG_EXPLOSION_PATH);
        _bloodHit = Resources.Load<GameObject>(Paths.BLOOD_HIT_PATH);
	}
}
