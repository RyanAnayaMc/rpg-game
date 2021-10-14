using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenuController : MonoBehaviour {
	public void Start() {
		SaveDataHandler.LoadSettings();
	}
	public void SetMusicVolume(float newVolume) {
		Settings.INSTANCE.musicVolume = newVolume;
		SaveDataHandler.SaveSettings();
	}

	public void SetSFXVolume(float newVolume) {
		Settings.INSTANCE.sfxVolume = newVolume;
		SaveDataHandler.SaveSettings();
	}

	public void UseDamageNumbers(bool value) {
		Settings.INSTANCE.showDamageNumbers = value;
		SaveDataHandler.SaveSettings();
	}
}
