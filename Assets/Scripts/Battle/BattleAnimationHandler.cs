using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    HealAnimation,
    FireSlashAttack,
    GunAttack,
    MendAnimation,
    RecoveryAnimation,
    Rapidfire,
    RifleAttack,
    SlamAttack
}

public class BattleAnimationHandler : MonoBehaviour {
    /// <summary>
    /// Reference to the battle's BattleController for convenience.
    /// </summary>
    [HideInInspector]
    public BattleController battleController;

    /// <summary>
    /// Plays a flicker animation for when a unit takes damage.
    /// </summary>
    /// <param name="unitObject">GameObject to play the flicker animation on. Must have a SpriteRenderer.</param>
    public async Task FlickerAnimation(GameObject unitObject) {
        SpriteRenderer sr = unitObject.GetComponent<SpriteRenderer>();
        Material oldMaterial = sr.material;

        for (int i = 0; i < 5; i++) {
            sr.material = null;
            await Task.Delay(100);
            sr.material = oldMaterial;
            await Task.Delay(100);
        }
    }

    /// <summary>
    /// Makes a GameObject with an Image component fade in.
    /// </summary>
    /// <param name="obj">The GameObject with an image component to fade in.</param>
    /// <param name="speed">The speed to fade in the image in alpha per fixed update.</param>
    public async void FadeIn(GameObject obj, float speed) {
        speed = Mathf.Abs(speed);
        Color originalColor = obj.GetComponent<Image>().color;
        float r = originalColor.r;
        float g = originalColor.g;
        float b = originalColor.b;
        float a = 0;

        while (a < 1) {
            a += speed;
            obj.GetComponent<Image>().color = new Color(r, g, b, a);
            await Utilities.UntilNextFrame();
        }
    }

    /// <summary>
    /// Makes a GameObject with an Image component fade out.
    /// </summary>
    /// <param name="obj">The GameObject with an image component to fade out.</param>
    /// <param name="speed">The speed to fade out the image in alpha per fixed update.</param>
    public async void FadeOut(GameObject obj, float speed) {
        speed = Mathf.Abs(speed);
        Color originalColor = obj.GetComponent<Image>().color;
        float r = originalColor.r;
        float g = originalColor.g;
        float b = originalColor.b;
        float a = 1;

        while (a > 0) {
            a -= speed;
            obj.GetComponent<Image>().color = new Color(r, g, b, a);
            await Utilities.UntilNextFrame();
        }
    }

    /// <summary>
    /// Makes a GameObject fade in by stretching it.
    /// </summary>
    /// <param name="obj">The GameObject to stretch in.</param>
    /// <param name="speed">The speed to stretch it. For best results, use a number that has 1 as a multiple.</param>
    public async Task StretchIn(GameObject obj, float speed) {
        speed = Mathf.Abs(speed);
        obj.SetActive(true);
        Vector3 startScale = obj.transform.localScale;
        float scaleY = startScale.y;
        float scaleZ = startScale.z;
        float scaleX = 0;

        while (scaleX < 1) {
            scaleX += speed;
            obj.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            await Utilities.UntilNextFrame();
        }
    }

    /// <summary>
    /// Makes a GameObject fade out by stretching it.
    /// </summary>
    /// <param name="obj">The GameObject to stretch out.</param>
    /// <param name="speed">The speed to stretch it. For best results, use a number that has 1 as a multiple.</param>
    public async Task StretchOut(GameObject obj, float speed) {
        speed = Mathf.Abs(speed);
        Vector3 startScale = obj.transform.localScale;
        float scaleY = startScale.y;
        float scaleZ = startScale.z;
        float scaleX = 1;

        while (scaleX > 0) {
            scaleX -= speed;
            obj.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            await Utilities.UntilNextFrame();
        }

        obj.SetActive(false);
    }
}
