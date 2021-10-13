using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class LightIntensitySine : MonoBehaviour {
	private HDAdditionalLightData lightData;

	[SerializeField]
	private float period;

	private float defaultIntensity;

	private void Start() {
		lightData = GetComponent<HDAdditionalLightData>();
		defaultIntensity = lightData.intensity;
	}

	private void Update() {
		float intensity = Mathf.Abs(Mathf.Sin(Time.time * 2 * Mathf.PI / period)) * defaultIntensity;
		lightData.intensity = intensity;
	}
}
