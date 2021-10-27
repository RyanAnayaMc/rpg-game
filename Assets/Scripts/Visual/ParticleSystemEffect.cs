using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

#pragma warning disable IDE0044, IDE0051

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(AudioSource))]
public class ParticleSystemEffect : MonoBehaviour {
	private new ParticleSystem particleSystem;
	private AudioSource audioSource;
	[SerializeField] private HDAdditionalLightData lightData;
	[SerializeField] private AudioClip soundEffect;

	private void Awake() {
		// Get references to components
		particleSystem = GetComponent<ParticleSystem>();
		audioSource = GetComponent<AudioSource>();
	}

	private void Update() {
		// Destroy the particle system if it is not running anymore
		if (!particleSystem.IsAlive())
			Destroy(gameObject);
	}

	/// <summary>
	/// Overrides the sound effect for this system effect and plays it.
	/// </summary>
	/// <param name="clip">The AudioClip to play</param>
	public void OverrideSoundEffect(AudioClip clip) {
		audioSource.Stop();
		audioSource.clip = clip;
		audioSource.Play();
	}
}
