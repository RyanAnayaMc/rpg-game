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
        yield return null;
    }

    IEnumerator FireAttack() {
        lightData.color = new Color(248, 80, 0);
        lightData.intensity = 10;

        for (int i = 0; i < 14; i++) {
            yield return new WaitForEndOfFrame();
        }
        while (lightData.intensity > 0)
            lightData.intensity--;
	}

    IEnumerator CastAnimation() {
        yield return null;
    }

    IEnumerator ThunderBowAttack() {
        yield return null;
    }

    IEnumerator HealAnimation() {
        yield return null;
    }
}
