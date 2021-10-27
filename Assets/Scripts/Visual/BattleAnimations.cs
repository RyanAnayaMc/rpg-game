using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable IDE0044

public class BattleAnimations {
    private static BattleAnimations _instance;
    public static BattleAnimations INSTANCE {
        get {
            if (_instance == null)
                _instance = new BattleAnimations();

            return _instance;
		}
	}

    private Dictionary<WeaponAnimation, GameObject> animations;

    #region Animation Prefab Fields
    private GameObject _smallExplosion;
    private GameObject _bigExplosion;
    // private GameObject _bloodHit;
    private GameObject _bowAttack;
    private GameObject _castAnimation;
    private GameObject _fireAttack;
    private GameObject _fireSlashAttack;
    private GameObject _gunAttack;
    private GameObject _healAnimation;
    private GameObject _mendAnimation;
    private GameObject _rapidfireAnimation;
    private GameObject _recoveryAnimation;
    private GameObject _rifleAttack;
    private GameObject _slamAttack;
    private GameObject _slashAttack;
    private GameObject _thunderBowAttack;
	#endregion

    private BattleAnimations() {
        // Get resources from project
        _smallExplosion = Resources.Load<GameObject>(Paths.SMALL_EXPLOSION_PATH);
        _bigExplosion = Resources.Load<GameObject>(Paths.BIG_EXPLOSION_PATH);
        // _bloodHit = Resources.Load<GameObject>(Paths.BLOOD_HIT_PATH);
        _bowAttack = Resources.Load<GameObject>(Paths.BOW_ATTACK_PATH);
        _castAnimation = Resources.Load<GameObject>(Paths.CAST_ANIMATION_PATH);
        _fireAttack = Resources.Load<GameObject>(Paths.FIRE_ATTACK_PATH);
        _fireSlashAttack = Resources.Load<GameObject>(Paths.FIRE_SLASH_PATH);
        _gunAttack = Resources.Load<GameObject>(Paths.GUN_IMPACT_EFFECT);
        _healAnimation = Resources.Load<GameObject>(Paths.HEAL_ANIMATION_PATH);
        _mendAnimation = Resources.Load<GameObject>(Paths.MEND_ANIMATION_PATH);
        _rapidfireAnimation = Resources.Load<GameObject>(Paths.RAPIDFIRE_ANIMATION_PATH);
        _recoveryAnimation = Resources.Load<GameObject>(Paths.RECOVER_ANIMATION_PATH);
        _rifleAttack = null;
        _slamAttack = Resources.Load<GameObject>(Paths.SLAM_ATTACK_PATH);
        _slashAttack = Resources.Load<GameObject>(Paths.SLASH_ATTACK_PATH);
        _thunderBowAttack = Resources.Load<GameObject>(Paths.THUNDER_BOW_ATTACK_PATH);

		// Add them to a dictionary
		animations = new Dictionary<WeaponAnimation, GameObject> {
			{ WeaponAnimation.BigExplosionAttack, _bigExplosion },
			{ WeaponAnimation.BowAttack, _bowAttack },
			{ WeaponAnimation.CastAnimation, _castAnimation },
			{ WeaponAnimation.ExplosionAttack, _smallExplosion },
			{ WeaponAnimation.FireAttack, _fireAttack },
			{ WeaponAnimation.FireSlashAttack, _fireSlashAttack },
			{ WeaponAnimation.GunAttack, _gunAttack },
			{ WeaponAnimation.HealAnimation, _healAnimation },
			{ WeaponAnimation.MendAnimation, _mendAnimation },
			{ WeaponAnimation.Rapidfire, _rapidfireAnimation },
			{ WeaponAnimation.RecoveryAnimation, _recoveryAnimation },
			{ WeaponAnimation.RifleAttack, _rifleAttack },
			{ WeaponAnimation.SlamAttack, _slamAttack },
			{ WeaponAnimation.SlashAttack, _slashAttack },
			{ WeaponAnimation.ThunderBowAttack, _thunderBowAttack }
		};
	}

    /// <summary>
    /// Performs an animation on the target.
    /// </summary>
    /// <param name="animation">The animation to perform.</param>
    /// <param name="target">The target to perform the animation on.</param>
    public void DoAnimationOn(WeaponAnimation animation, Transform target) {
        GameObject.Instantiate(animations[animation], target);
	}
}
