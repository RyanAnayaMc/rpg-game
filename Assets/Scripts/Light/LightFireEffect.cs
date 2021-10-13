using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

/// <summary>
/// Basic script that makes a HDRP light flicker like fire.
/// Lights with the time set offset will flicker in sync.
/// </summary>
[RequireComponent(typeof(HDAdditionalLightData))]
public class LightFireEffect : MonoBehaviour {
    private HDAdditionalLightData lightData;
	private float defaultIntensity;

	[SerializeField]
	private bool useRandomOffset;

	[SerializeField]
	private float setOffset;

	[SerializeField]
	private float speedMultiplier;

	[SerializeField]
	private float minimumMultiplier;

	[SerializeField]
	private float maximumMultiplier;

	private void Start() {
		lightData = GetComponent<HDAdditionalLightData>();
		defaultIntensity = lightData.intensity;
		if (useRandomOffset)
			setOffset = Random.value * 20;

		if (speedMultiplier == 0)
			speedMultiplier = 1;
	}

	private void Update() {
		float offsetTime = speedMultiplier * Time.time + setOffset;
		float multiplier = Mathf.Abs(
			Mathf.Max(
				Mathf.Sin(offsetTime),
				Mathf.Cos(offsetTime),
				Mathf.Sin(offsetTime / 1.5f),
				Mathf.Cos(5 * offsetTime - 5),
				Mathf.Sin(3 * offsetTime)
			)
		);

		multiplier = Mathf.Clamp(multiplier, minimumMultiplier, maximumMultiplier);

		lightData.intensity = defaultIntensity * multiplier;
	}
}
