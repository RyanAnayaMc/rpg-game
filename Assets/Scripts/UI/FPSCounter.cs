using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour {
    private TMP_Text fpsText;

	public void Start() {
		gameObject.SetActive(Settings.INSTANCE.showFPS);

		fpsText = GetComponentInChildren<TMP_Text>();
	}
	public void Update() {
		if (Settings.INSTANCE.showFPS) {
			int fps = (int) (1 / Time.unscaledDeltaTime);
			fpsText.text = fps + "FPS";
		}
	}
}
