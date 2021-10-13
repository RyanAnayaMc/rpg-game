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
        lightData.color = new Color(1, (float) 210 / 255, 0);

        lightData.intensity = 78;

        for (int i = 0; i < 13; i++) {
            lightData.intensity -= 6;
            yield return new WaitForEndOfFrame();
		}
    }

    IEnumerator FireAttack() {
        lightData.color = new Color((float) 248 / 255, (float) 80 / 255, 0);
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
	}

    IEnumerator CastAnimation() {
        yield return null;
    }

    IEnumerator ThunderBowAttack() {
        yield return null;
    }

    IEnumerator HealAnimation() {
        lightData.color = Color.green;
        lightData.intensity = 65;

        while (lightData.intensity > 0) {
            lightData.intensity--;
            yield return new WaitForEndOfFrame();
		}
    }
}
