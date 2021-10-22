using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSFXHandler : MonoBehaviour {
    /// <summary>
    /// Reference to the battle's BattleController for convenience.
    /// </summary>
    [HideInInspector]
    public BattleController battleController;
    [SerializeField]
    private AudioSource sfxSource;
    private AudioSource[] audioSources;
    private int index;
    [SerializeField]
    private int audioSourceCount;
    [SerializeField]
    private AudioClip confirmSFX;
    [SerializeField]
    private AudioClip backSFX;

	private void Start() {
        audioSources = new AudioSource[audioSourceCount];
        for (int i = 0; i < audioSourceCount; i++)
            audioSources[i] = Instantiate(sfxSource.gameObject).GetComponent<AudioSource>();
        index = 0;
	}

    private AudioSource CurrentAudioSource() {
        AudioSource src = audioSources[index];
        index++;
        if (index >= audioSourceCount) index = 0;

        return src;
	}

	/// <summary>
	/// Plays the confirm sound effect.
	/// </summary>
	public void PlayConfirmSFX() {
        CurrentAudioSource().PlayOneShot(confirmSFX);
    }

    /// <summary>
    /// Plays the back sound effect.
    /// </summary>
    public void PlayBackSFX() {
        CurrentAudioSource().PlayOneShot(backSFX);
    }

    /// <summary>
    /// Plays a specific sound effect.
    /// </summary>
    /// <param name="sfxClip">The sound effect to play.</param>
    public void PlaySFX(AudioClip sfxClip) {
        CurrentAudioSource().PlayOneShot(sfxClip);
    }
}
