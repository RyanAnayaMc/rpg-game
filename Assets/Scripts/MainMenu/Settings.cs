using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Settings {
    private static Settings _instance;

    public static Settings INSTANCE {
		get {
			if (_instance is null)
				_instance = new Settings();

			return _instance;
		}
	}

	/// <summary>
	/// The volume setting for music. Please do not use this value. Instead, assign
	/// <see cref="AudioSourceVolumeController"/> to your <see cref="AudioSource"/> to
	/// control the music volume in your scenes.
	/// </summary>
	[SerializeField]
	public float musicVolume;

	/// <summary>
	/// The volume setting for sound effects. Please do not use this value. Instead, assign
	/// <see cref="AudioSourceVolumeController"/> to your <see cref="AudioSource"/>  and tick
	/// the isSFX option to true to control the sound effect volume in your scenes.
	/// </summary>
	[SerializeField]
	public float sfxVolume;

	/// <summary>
	/// Whether or not damage numbers should be shown in battle.
	/// </summary>
	[SerializeField]
	public bool showDamageNumbers;

	/// <summary>
	/// Whether or not dank shit should happen.
	/// </summary>
	[SerializeField]
	public bool doDankShit;

	private Settings() {
		musicVolume = 1;
		sfxVolume = 1;
		showDamageNumbers = true;
		doDankShit = false;
	}
}
