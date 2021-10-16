using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSFXHandler : MonoBehaviour {
    public BattleController battleController;
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private AudioClip confirmSFX;
    [SerializeField]
    private AudioClip backSFX;

    /// <summary>
    /// Plays the confirm sound effect.
    /// </summary>
    public void PlayConfirmSFX() {
        sfxSource.PlayOneShot(confirmSFX);
    }

    /// <summary>
    /// Plays the back sound effect.
    /// </summary>
    public void PlayBackSFX() {
        sfxSource.PlayOneShot(backSFX);
    }

    /// <summary>
    /// Plays a specific sound effect.
    /// </summary>
    /// <param name="sfxClip">The sound effect to play.</param>
    public void PlaySFX(AudioClip sfxClip) {
        sfxSource.PlayOneShot(sfxClip);
    }
}
