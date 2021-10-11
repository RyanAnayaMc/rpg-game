using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    public string playButtonScene;
    public AudioSource sfxSource;
    public AudioClip buttonClickSFX;
    public AudioClip backSFX;

    // TODO use SFXHandler
    
    public void playSFX() {
        sfxSource.PlayOneShot(buttonClickSFX);
    }

    public void playCancelSFX() {
        sfxSource.PlayOneShot(backSFX);
    }

    public void onPlayButton() {
        playSFX();
        SceneManager.LoadScene(playButtonScene);
        // TODO: loading screen?
    }

    public void onExitButton() {
        Application.Quit();
    }
}