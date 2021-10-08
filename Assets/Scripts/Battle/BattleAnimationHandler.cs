using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleAnimationHandler : MonoBehaviour
{
    // Plays a flicker animation for when a unit takes damage
    public void FlickerAnimation(Unit unit)
    {
        StartCoroutine(FlickerAnimationCoroutine(unit));
    }

    private IEnumerator FlickerAnimationCoroutine(Unit unit)
    {
        SpriteRenderer sr = unit.gameObject.GetComponent<SpriteRenderer>();
        Color oldColor = sr.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 0);

        for (int i = 0; i < 5; i++)
        {
            sr.color = newColor;
            yield return new WaitForSeconds(0.1f);
            sr.color = oldColor;
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Plays a cast and damage animation, including sound effects
    public void PlayDamageAnimation(Unit attackingUnit, Unit targetUnit, AudioSource sfxSource)
    {
        StartCoroutine(PlayDamageAnimationCoroutine(attackingUnit, targetUnit, sfxSource));
    }

    private IEnumerator PlayDamageAnimationCoroutine(Unit attackingUnit, Unit targetUnit, AudioSource sfxSource)
    {
        // Play cast sound effect and animation
        Weapon weapon = attackingUnit.weapon;
        if (weapon.castSFX != null)
            sfxSource.PlayOneShot(weapon.castSFX);
        attackingUnit.effectRenderer.GetComponent<Animator>().SetTrigger(weapon.castAnimation.ToString());

        // Play attack sound effect and animation
        targetUnit.effectRenderer.GetComponent<Animator>().SetTrigger(weapon.weaponAnimation.ToString());
        if (weapon.attackSFX != null)
            sfxSource.PlayOneShot(weapon.attackSFX);

        // Play flicker animation
        StartCoroutine(FlickerAnimationCoroutine(targetUnit));

        yield return new WaitForSeconds(0);
    }

    // Makes a GameObject with an Image component fade in
    public void fadeIn(GameObject obj, float speed)
    {
        StartCoroutine(fadeInCoroutine(obj, speed));
    }

    private IEnumerator fadeInCoroutine(GameObject obj, float speed)
    {
        speed = Mathf.Abs(speed);
        Color originalColor = obj.GetComponent<Image>().color;
        float r = originalColor.r;
        float g = originalColor.g;
        float b = originalColor.b;
        float a = 0;

        while (a < 1)
        {
            a += speed;
            obj.GetComponent<Image>().color = new Color(r, g, b, a);
            yield return new WaitForFixedUpdate();
        }
    }

    // Makes a GameObject with an Image component fade out
    public void fadeOut(GameObject obj, float speed)
    {
        StartCoroutine(fadeOutCoroutine(obj, speed));
    }

    private IEnumerator fadeOutCoroutine(GameObject obj, float speed)
    {
        speed = Mathf.Abs(speed);
        Color originalColor = obj.GetComponent<Image>().color;
        float r = originalColor.r;
        float g = originalColor.g;
        float b = originalColor.b;
        float a = 1;

        while (a > 0)
        {
            a -= speed;
            obj.GetComponent<Image>().color = new Color(r, g, b, a);
            yield return new WaitForFixedUpdate();
        }
    }

    // Makes a GameObject with a Sprite component fade out
    public void fadeOutSprite(GameObject obj, float speed)
    {
        StartCoroutine(fadeOutSpriteCoroutine(obj, speed));
    }

    private IEnumerator fadeOutSpriteCoroutine(GameObject obj, float speed)
    {
        speed = Mathf.Abs(speed);
        Color originalColor = obj.GetComponent<SpriteRenderer>().color;
        float r = originalColor.r;
        float g = originalColor.g;
        float b = originalColor.b;
        float a = 1;

        while (a > 0)
        {
            a -= speed;
            obj.GetComponent<SpriteRenderer>().color = new Color(r, g, b, a);
            yield return new WaitForFixedUpdate();
        }
    }

    // Makes a GameObject appear by stretching out
    public void stretchIn(GameObject obj, float speed)
    {
        StartCoroutine(stretchInCoroutine(obj, speed));
    }
    
    private IEnumerator stretchInCoroutine(GameObject obj, float speed)
    {
        speed = Mathf.Abs(speed);
        obj.SetActive(true);
        Vector3 startScale = obj.transform.localScale;
        float scaleY = startScale.y;
        float scaleZ = startScale.z;
        float scaleX = 0;

        while (scaleX < 1)
        {
            scaleX += speed;
            obj.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            yield return new WaitForFixedUpdate();
        }
    }

    // Makes a GameObject disappear by stretching out
    public void stretchOut(GameObject obj, float speed)
    {
        StartCoroutine(stretchOutCoroutine(obj, speed));
    }

    private IEnumerator stretchOutCoroutine(GameObject obj, float speed)
    {
        speed = Mathf.Abs(speed);
        Vector3 startScale = obj.transform.localScale;
        float scaleY = startScale.y;
        float scaleZ = startScale.z;
        float scaleX = 1;

        while (scaleX > 0)
        {
            scaleX -= speed;
            obj.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            yield return new WaitForFixedUpdate();
        }

        obj.SetActive(false);
    }
}
