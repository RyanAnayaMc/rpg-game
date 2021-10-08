using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMusicHandler : MonoBehaviour
{
    public AudioSource musicSource; // The Scene's music player
    public AudioClip battleMusic; // The music to play during the battle
    public AudioClip victoryFanfare; // Fanfare that plays if you win
    public AudioClip victoryMusic; // Music that plays after the fanfare

    // Plays the battle music
    public void PlayBattleMusic()
    {
        musicSource.clip = battleMusic;
        musicSource.loop = true;
        musicSource.Play();
    }


    public void PlayVictoryFanfare()
    {
        musicSource.Stop();
        StartCoroutine(PlayVictoryFanfareCoroutine());
    }

    IEnumerator PlayVictoryFanfareCoroutine()
    {
        // Play victory fanfare
        musicSource.loop = false;
        musicSource.clip = victoryFanfare;
        musicSource.Play();

        // Wait for it to finish
        while (musicSource.isPlaying)
            yield return new WaitForSeconds(0.5f);

        // Play victory music
        musicSource.clip = victoryMusic;
        musicSource.loop = true;
        musicSource.Play();
    }
}
