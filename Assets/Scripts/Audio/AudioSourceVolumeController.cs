using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceVolumeController : MonoBehaviour {
    [SerializeField]
    private bool isSFX;
    private Settings settings = Settings.INSTANCE;

    void Update() {
        GetComponent<AudioSource>().volume = isSFX ? settings.sfxVolume : settings.musicVolume;
    }
}
