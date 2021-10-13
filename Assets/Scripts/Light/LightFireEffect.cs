using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class LightFireEffect : MonoBehaviour {
    private HDAdditionalLightData lightData;
	private float defaultIntensity;

	private void Start() {
		lightData = GetComponent<HDAdditionalLightData>();
		defaultIntensity = lightData.intensity;
	}

	private void Update() {
		float multiplier = Mathf.Max(
			Mathf.Sin(Time.time),
			Mathf.Cos(Time.time),
			Mathf.Sin(Time.time / 1.5f)
			);
		lightData.intensity = defaultIntensity * multiplier;
	}
}
