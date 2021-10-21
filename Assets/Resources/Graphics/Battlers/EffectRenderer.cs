using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Animator))]
public class EffectRenderer : MonoBehaviour {
    private Animator animator;
    private HDAdditionalLightData lightData;

    void Start() {
        animator = GetComponent<Animator>();
        lightData = GetComponentInChildren<HDAdditionalLightData>();
        lightData.lightUnit = LightUnit.Lumen;
    }

    public void DoAnimation(WeaponAnimation animation) {
        // Do animation sprite
        animator.SetTrigger(animation.ToString());
        StartCoroutine(animation.ToString());
	}

    IEnumerator NONE() { yield return null; }

    IEnumerator SlashAttack() {
        lightData.color = RGB(255, 210, 0);

        lightData.intensity = 78;

        for (int i = 0; i < 13; i++) {
            lightData.intensity -= 6;
            yield return new WaitForEndOfFrame();
		}

        lightData.intensity = 0;
    }

    IEnumerator FireAttack() {
        lightData.color = RGB(248, 80, 0);
        int[] intensities = {300, 250, 275};
        lightData.intensity = 300;

        for (int i = 0; i < 35; i++) {
            yield return new WaitForEndOfFrame();
            if (i % 3 == 0)
                lightData.intensity = intensities[Random.Range(0, 3)];
        }
        while (lightData.intensity > 0) {
            lightData.intensity -= 12.5f;
            yield return new WaitForEndOfFrame();
        }

        lightData.intensity = 0;
    }

    IEnumerator CastAnimation() {
        yield return null;
    }

    IEnumerator ThunderBowAttack() {
        lightData.color = RGB(199, 255, 0);
        lightData.intensity = 70;

        for (int i = 0; i < 35; i++) {
            lightData.intensity -= 2;
            yield return new WaitForEndOfFrame();
        }

        lightData.intensity = 0;
    }

    IEnumerator HealAnimation() {
        lightData.color = Color.green;
        lightData.intensity = 65;

        while (lightData.intensity > 0) {
            lightData.intensity--;
            yield return new WaitForEndOfFrame();
		}

        lightData.intensity = 0;
    }

    IEnumerator FireSlashAttack() {
        lightData.color = RGB(255, 223, 0);
        lightData.intensity = 25;

        for (int i = 0; i < 25; i++) {
            lightData.intensity --;
            yield return new WaitForEndOfFrame();
        }

        lightData.intensity = 90;
        for (int i = 0; i < 45; i++) {
            lightData.intensity -= 2;
            yield return new WaitForEndOfFrame();
        }

        lightData.intensity = 0;
    }

    IEnumerator GunAttack() {
        lightData.color = RGB(255, 221, 0);
        lightData.intensity = 30;

        for (int i = 0; i < 30; i++) {
            lightData.intensity -= 2;
            yield return new WaitForEndOfFrame();
        }

        lightData.intensity = 0;
	}

    IEnumerator MendAnimation() {
        lightData.color =  RGB(0, 255, 234);
        lightData.intensity = 50;

        for (int i = 0; i < 50; i++) {
            lightData.intensity --;
            yield return new WaitForEndOfFrame();
        }

        lightData.intensity = 0;
    }

    IEnumerator RecoveryAnimation() {
        lightData.color = RGB(0, 180, 255);
        lightData.intensity = 55;

        for (int i = 0; i < 30; i++) {
            lightData.intensity--;
            yield return new WaitForEndOfFrame();
        }

        lightData.intensity = 70;
        for (int i = 0; i < 30; i++) {
            lightData.intensity -= 2;
            yield return new WaitForEndOfFrame();
        }

        lightData.intensity = 0;
    }

    private Color RGB(int r, int g, int b, int a = 255) {
        return new Color(
            (float) r / 255,
            (float) g / 255,
            (float) b / 255,
            (float) a / 255
        );
	}
}
