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
	public Toggle raytracingToggle;
	public Toggle fpsToggle;
	public GameObject fpsCounter;

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
		raytracingToggle.isOn = Settings.INSTANCE.useRaytracing;
		fpsToggle.isOn = Settings.INSTANCE.showFPS;
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

	public void UseRaytracing(bool value) {
		QualitySettings.SetQualityLevel(value ? 0 : 1);
		Settings.INSTANCE.useRaytracing = value;
	}

	public void ShowFPS(bool value) {
		Settings.INSTANCE.showFPS = value;
		fpsCounter.SetActive(value);
	}

	public void SaveSettings() {
		SaveDataHandler.SaveSettings();
	}
}
