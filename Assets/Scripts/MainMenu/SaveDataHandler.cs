using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveDataHandler {
	static string settingsPath = Application.persistentDataPath + "/settings.rpg";

	public static void SaveSettings() {
		BinaryFormatter binFormatter = new BinaryFormatter();

		
		FileStream fileStream = new FileStream(settingsPath, FileMode.Create);

		binFormatter.Serialize(fileStream, Settings.INSTANCE);
		fileStream.Close();
	}

	public static void LoadSettings() {

	}

	internal static Settings LoadSettingsAndReturn() {
		if (File.Exists(settingsPath)) {
			BinaryFormatter binFormatter = new BinaryFormatter();
			FileStream fileStream = new FileStream(settingsPath, FileMode.Open);

			Settings settings = binFormatter.Deserialize(fileStream) as Settings;

			return settings;
		} else return Settings.INSTANCE;
	}
}
