using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WorldSFXHandler : MonoBehaviour {
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip walkSFX;

	private void Awake() {
		audioSource = GetComponent<AudioSource>();
		audioSource.volume = Settings.INSTANCE.sfxVolume;
	}

	public void PlayWalkSFX() {
		if (!audioSource.isPlaying)
			audioSource.PlayOneShot(walkSFX, 0.3f);
	}
}
