using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Determines which animation plays when casting/attacking
public enum WeaponAnimation {
    NONE,
    SlashAttack,
    FireAttack,
    CastAnimation,
    BowAttack,
    ThunderBowAttack,
    HealAnimation
}

public class BattleAnimationHandler : MonoBehaviour {
    /// <summary>
    /// Plays a flicker animation for when a unit takes damage.
    /// </summary>
    /// <param name="unitObject">GameObject to play the flicker animation on. Must have a SpriteRenderer.</param>
    public void FlickerAnimation(GameObject unitObject) {
        StartCoroutine(FlickerAnimationCoroutine(unitObject));
    }

    private IEnumerator FlickerAnimationCoroutine(GameObject unitObject) {
        SpriteRenderer sr = unitObject.GetComponent<SpriteRenderer>();
        Color oldColor = sr.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 0);

        for (int i = 0; i < 5; i++) {
            sr.color = newColor;
            yield return new WaitForSeconds(0.1f);
            sr.color = oldColor;
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// Plays a cast and damage animation, including sound effects.
    /// </summary>
    /// <param name="attackingUnit">The unit attacking.</param>
    /// <param name="targetUnit">The unit being attacked.</param>
    /// <param name="sfxHandler">The battle's BattleSFXHandler.</param>
    public void PlayDamageAnimation(BattleUnit attackingUnit, BattleUnit targetUnit, BattleSFXHandler sfxHandler) {
        // Play cast sound effect and animation
        Weapon weapon = attackingUnit.unit.weapon;
        if (weapon.castSFX != null)
            sfxHandler.PlaySFX(weapon.castSFX);
        attackingUnit.effectRenderer.GetComponent<Animator>().SetTrigger(weapon.castAnimation.ToString());

        // Play attack sound effect and animation
        targetUnit.effectRenderer.GetComponent<Animator>().SetTrigger(weapon.weaponAnimation.ToString());
        if (weapon.attackSFX != null)
            sfxHandler.PlaySFX(weapon.attackSFX);

        // Play flicker animation
        StartCoroutine(FlickerAnimationCoroutine(targetUnit.gameObject));
    }
    
    /// <summary>
    /// Plays a skill animaiton, including sound effects, for nonscripted skills.
    /// </summary>
    /// <param name="attackingUnit">The unit attacking.</param>
    /// <param name="targetUnit">The unit being attacked.</param>
    /// <param name="skill">The non-scripted skill.</param>
    /// <param name="sfxHandler">The battle's BattleSFXHandler.</param>
    public void PlaySkillAnimation(BattleUnit attackingUnit, BattleUnit targetUnit, Skill skill, BattleSFXHandler sfxHandler) {
        BattleUnit battleUnit = (skill.targetType == TargetType.SELF) ? attackingUnit : targetUnit;

        battleUnit.effectRenderer.GetComponent<Animator>().SetTrigger(skill.skillAnimation.ToString());
        if (skill.targetType == TargetType.ENEMY)
            FlickerAnimation(battleUnit.gameObject);
        if (skill.skillSFX != null)
            sfxHandler.PlaySFX(skill.skillSFX);
    }

    /// <summary>
    /// Makes a GameObject with an Image component fade in.
    /// </summary>
    /// <param name="obj">The GameObject with an image component to fade in.</param>
    /// <param name="speed">The speed to fade in the image in alpha per fixed update.</param>
    public void fadeIn(GameObject obj, float speed) {
        StartCoroutine(fadeInCoroutine(obj, speed));
    }

    private IEnumerator fadeInCoroutine(GameObject obj, float speed) {
        speed = Mathf.Abs(speed);
        Color originalColor = obj.GetComponent<Image>().color;
        float r = originalColor.r;
        float g = originalColor.g;
        float b = originalColor.b;
        float a = 0;

        while (a < 1) {
            a += speed;
            obj.GetComponent<Image>().color = new Color(r, g, b, a);
            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// Makes a GameObject with an Image component fade out.
    /// </summary>
    /// <param name="obj">The GameObject with an image component to fade out.</param>
    /// <param name="speed">The speed to fade out the image in alpha per fixed update.</param>
    public void fadeOut(GameObject obj, float speed) {
        StartCoroutine(fadeOutCoroutine(obj, speed));
    }

    private IEnumerator fadeOutCoroutine(GameObject obj, float speed) {
        speed = Mathf.Abs(speed);
        Color originalColor = obj.GetComponent<Image>().color;
        float r = originalColor.r;
        float g = originalColor.g;
        float b = originalColor.b;
        float a = 1;

        while (a > 0) {
            a -= speed;
            obj.GetComponent<Image>().color = new Color(r, g, b, a);
            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// Makes a GameObject with a Sprite component fade in.
    /// </summary>
    /// <param name="obj">The GameObject with an sprite component to fade in.</param>
    /// <param name="speed">The speed to fade in the sprite in alpha per fixed update.</param>
    public void fadeOutSprite(GameObject obj, float speed) {
        StartCoroutine(fadeOutSpriteCoroutine(obj, speed));
    }

    private IEnumerator fadeOutSpriteCoroutine(GameObject obj, float speed) {
        speed = Mathf.Abs(speed);
        Color originalColor = obj.GetComponent<SpriteRenderer>().color;
        float r = originalColor.r;
        float g = originalColor.g;
        float b = originalColor.b;
        float a = 1;

        while (a > 0) {
            a -= speed;
            obj.GetComponent<SpriteRenderer>().color = new Color(r, g, b, a);
            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// Makes a GameObject fade in by stretching it.
    /// </summary>
    /// <param name="obj">The GameObject to stretch in.</param>
    /// <param name="speed">The speed to stretch it. For best results, use a number that has 1 as a multiple.</param>
    public void stretchIn(GameObject obj, float speed) {
        StartCoroutine(stretchInCoroutine(obj, speed));
    }
    
    private IEnumerator stretchInCoroutine(GameObject obj, float speed) {
        speed = Mathf.Abs(speed);
        obj.SetActive(true);
        Vector3 startScale = obj.transform.localScale;
        float scaleY = startScale.y;
        float scaleZ = startScale.z;
        float scaleX = 0;

        while (scaleX < 1) {
            scaleX += speed;
            obj.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// Makes a GameObject fade out by stretching it.
    /// </summary>
    /// <param name="obj">The GameObject to stretch out.</param>
    /// <param name="speed">The speed to stretch it. For best results, use a number that has 1 as a multiple.</param>
    public void stretchOut(GameObject obj, float speed) {
        StartCoroutine(stretchOutCoroutine(obj, speed));
    }

    private IEnumerator stretchOutCoroutine(GameObject obj, float speed) {
        speed = Mathf.Abs(speed);
        Vector3 startScale = obj.transform.localScale;
        float scaleY = startScale.y;
        float scaleZ = startScale.z;
        float scaleX = 1;

        while (scaleX > 0) {
            scaleX -= speed;
            obj.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            yield return new WaitForFixedUpdate();
        }

        obj.SetActive(false);
    }
}
