using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    public string playButtonScene;
    public AudioSource sfxSource;
    public AudioClip buttonClickSFX;
    public AudioClip backSFX;
    public string buyURL;

    public PlayerUnit playerUnit;
    public List<Unit> enemyUnits;
    public AudioClip battleMusic;

    // TODO use SFXHandler
    
    public void playSFX() {
        sfxSource.PlayOneShot(buttonClickSFX);
    }

    public void playCancelSFX() {
        sfxSource.PlayOneShot(backSFX);
    }

    public void onPlayButton() {
        playSFX();
        BattleController.StartBattle(playerUnit, enemyUnits, battleMusic, gameObject.scene.name, gameObject.transform, playButtonScene);
        // TODO: loading screen?
    }

    public void onExitButton() {
        Application.Quit();
    }

    public void onBuyButton() {
        Application.OpenURL(buyURL);
	}
}
