using UnityEngine;

namespace Monoblok
{
	public class PlayerPrefsGameSettingsSaveService : GameSettingsSaveService
	{
		private const string KeyMasterVolume = "Settings_MasterVolume";

		private const string KeyMusicVolume = "Settings_MusicVolume";

		private const string KeySFXVolume = "Settings_SFXVolume";

		private const string KeyLocaleCode = "Settings_LocaleCode";

		private const string KeyVSync = "Settings_VSync";

		public override float LoadMasterVolume(float defaultValue = 1f)
		{
			return PlayerPrefs.GetFloat("Settings_MasterVolume", defaultValue);
		}

		public override float LoadMusicVolume(float defaultValue = 1f)
		{
			return PlayerPrefs.GetFloat("Settings_MusicVolume", defaultValue);
		}

		public override float LoadSFXVolume(float defaultValue = 1f)
		{
			return PlayerPrefs.GetFloat("Settings_SFXVolume", defaultValue);
		}

		public override string LoadLocaleCode(string defaultValue = "")
		{
			return PlayerPrefs.GetString("Settings_LocaleCode", defaultValue);
		}

		public override bool LoadVSync(bool defaultValue = true)
		{
			return PlayerPrefs.GetInt("Settings_VSync", defaultValue ? 1 : 0) == 1;
		}

		public override void SaveMasterVolume(float value)
		{
			PlayerPrefs.SetFloat("Settings_MasterVolume", value);
			PlayerPrefs.Save();
		}

		public override void SaveMusicVolume(float value)
		{
			PlayerPrefs.SetFloat("Settings_MusicVolume", value);
			PlayerPrefs.Save();
		}

		public override void SaveSFXVolume(float value)
		{
			PlayerPrefs.SetFloat("Settings_SFXVolume", value);
			PlayerPrefs.Save();
		}

		public override void SaveLocaleCode(string value)
		{
			PlayerPrefs.SetString("Settings_LocaleCode", value);
			PlayerPrefs.Save();
		}

		public override void SaveVSync(bool value)
		{
			PlayerPrefs.SetInt("Settings_VSync", value ? 1 : 0);
			PlayerPrefs.Save();
		}
	}
}
