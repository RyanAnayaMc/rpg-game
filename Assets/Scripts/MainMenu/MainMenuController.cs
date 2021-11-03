using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    public string playButtonScene;
    public string playButton2Scene;
    public AudioSource sfxSource;
    public AudioClip buttonClickSFX;
    public AudioClip backSFX;
    public string buyURL;
    public string websiteURL;

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

    public void onPlayButton2() {
        playSFX();
        SceneManager.LoadScene(playButton2Scene);
    }

    public void onExitButton() {
        Application.Quit();
    }

    public void onBuyButton() {
        Application.OpenURL(buyURL);
	}

    public void onWebsiteButton() {
        Application.OpenURL(websiteURL);
	}
}
