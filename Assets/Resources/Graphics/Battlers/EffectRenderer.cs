using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

#pragma warning disable IDE0051

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
        StartCoroutine(animation.ToString());
	}


	IEnumerator NONE() { yield return null; }

	IEnumerator SlashAttack() {
        animator.SetTrigger("SlashAttack");
        lightData.color = Utilities.RGB(255, 210, 0);

        lightData.intensity = 78;

        for (int i = 0; i < 13; i++) {
            lightData.intensity -= 6;
            yield return new WaitForEndOfFrame();
		}

        lightData.intensity = 0;
    }

    IEnumerator FireAttack() {
        animator.SetTrigger("FireAttack");
        lightData.color = Utilities.RGB(248, 80, 0);
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
        animator.SetTrigger("CastAnimation");
        yield return null;
    }

    IEnumerator ThunderBowAttack() {
        animator.SetTrigger("ThunderBowAttack");
        lightData.color = Utilities.RGB(199, 255, 0);
        lightData.intensity = 70;

        for (int i = 0; i < 35; i++) {
            lightData.intensity -= 2;
            yield return new WaitForEndOfFrame();
        }

        lightData.intensity = 0;
    }

    IEnumerator HealAnimation() {
        animator.SetTrigger("HealAnimation");
        lightData.color = Color.green;
        lightData.intensity = 65;

        while (lightData.intensity > 0) {
            lightData.intensity--;
            yield return new WaitForEndOfFrame();
		}

        lightData.intensity = 0;
    }

    IEnumerator FireSlashAttack() {
        animator.SetTrigger("FireSlashAttack");
        lightData.color = Utilities.RGB(255, 223, 0);
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
        animator.SetTrigger("GunAttack");
        lightData.color = Utilities.RGB(255, 221, 0);
        lightData.intensity = 30;

        for (int i = 0; i < 30; i++) {
            lightData.intensity -= 2;
            yield return new WaitForEndOfFrame();
        }

        lightData.intensity = 0;
	}

    IEnumerator MendAnimation() {
        animator.SetTrigger("MendAnimation");
        lightData.color =  Utilities.RGB(0, 255, 234);
        lightData.intensity = 50;

        for (int i = 0; i < 50; i++) {
            lightData.intensity --;
            yield return new WaitForEndOfFrame();
        }

        lightData.intensity = 0;
    }

    IEnumerator RecoveryAnimation() {
        animator.SetTrigger("RecoveryAnimation");
        lightData.color = Utilities.RGB(0, 180, 255);
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

    IEnumerator Rapidfire() {
        animator.SetTrigger("Rapidfire");
        yield return null;
	}

    IEnumerator RifleAttack() {
        animator.SetTrigger("RifleAttack");
        yield return null;
	}

    IEnumerator SlamAttack() {
        animator.SetTrigger("SlamAttack");
        yield return null;
	}
}
