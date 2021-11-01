using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable IDE0051

[RequireComponent(typeof(AudioSource))]
public class GameOverScreen : MonoBehaviour {
    private AudioSource music;
    private bool canSkipScreen;

    // Start is called before the first frame update
    void Start() {
        music = GetComponent<AudioSource>();
        _ = Utilities.DoAfter(500, () => canSkipScreen = true);
    }

    // Update is called once per frame
    void Update() {
        if (canSkipScreen && (!music.isPlaying || Input.GetButtonDown("Interact")))
            SceneManager.LoadScene("MainMenu");
    }
}
