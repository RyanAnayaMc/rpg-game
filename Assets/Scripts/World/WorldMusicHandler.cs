using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WorldMusicHandler : MonoBehaviour {
    private AudioSource audioSource;
    public AudioClip worldMusic;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = worldMusic;
        audioSource.Play();
    }

    /// <summary>
    /// Stops the currently playing overworld music.
    /// </summary>
    public void StopMusic() {
        audioSource.Stop();
	}

    /// <summary>
    /// Plays a new audio clip as the music.
    /// </summary>
    /// <param name="clip">The new clip to play.</param>
    /// <returns>The clip that was playing previously.</returns>
    public AudioClip PlayNewClip(AudioClip clip) {
        AudioClip oldClip = audioSource.clip;
        if (audioSource.isPlaying)
            StopMusic();

        audioSource.clip = clip;
        audioSource.Play();

        return oldClip;
	}
}
