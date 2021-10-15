using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// Class with methods and data pertaining to the options. These methods probably should
/// not be called via script and instead called via Sliders or Toggles.
/// </summary>
public class OptionsMenuController : MonoBehaviour {
	public Slider musicVolumeSlider;
	public Slider sfxVolumeSlider;
	public Toggle damageNumberToggle;
	public Toggle dankShitToggle;

	// Read settings from file
	public void Awake() {
		SaveDataHandler.LoadSettings();
	}

	// Change settings values to those read from file
	public void Start() {
		musicVolumeSlider.value = Settings.INSTANCE.musicVolume;
		sfxVolumeSlider.value = Settings.INSTANCE.sfxVolume;
		damageNumberToggle.isOn = Settings.INSTANCE.showDamageNumbers;
		dankShitToggle.isOn = Settings.INSTANCE.doDankShit;
	}
	public void SetMusicVolume(float newVolume) {
		Settings.INSTANCE.musicVolume = newVolume;
	}

	public void SetSFXVolume(float newVolume) {
		Settings.INSTANCE.sfxVolume = newVolume;
	}

	public void UseDamageNumbers(bool value) {
		Settings.INSTANCE.showDamageNumbers = value;
	}

	public void DoDankShit(bool value) {
		Settings.INSTANCE.doDankShit = value;
	}

	public void SaveSettings() {
		SaveDataHandler.SaveSettings();
	}
}
