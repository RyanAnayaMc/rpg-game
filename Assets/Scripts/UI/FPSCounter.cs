using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour {
	private float countdown;
    private TMP_Text fpsText;

	public void Start() {
		countdown = 0.125f;
		fpsText = GetComponentInChildren<TMP_Text>();
	}
	public void FixedUpdate() {
		float timeDelta = Time.unscaledDeltaTime;

		if (countdown > 0)
			countdown -= timeDelta;
		else {
			countdown = 0.125f;
			int fps = (int) (1 / timeDelta);
			fpsText.text = fps + "FPS";
		}
	}
}
