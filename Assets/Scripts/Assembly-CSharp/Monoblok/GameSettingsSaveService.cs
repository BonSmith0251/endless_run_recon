namespace Monoblok
{
	public abstract class GameSettingsSaveService
	{
		public abstract float LoadMasterVolume(float defaultValue = 1f);

		public abstract float LoadMusicVolume(float defaultValue = 1f);

		public abstract float LoadSFXVolume(float defaultValue = 1f);

		public abstract string LoadLocaleCode(string defaultValue = "");

		public abstract bool LoadVSync(bool defaultValue = true);

		public abstract void SaveMasterVolume(float value);

		public abstract void SaveMusicVolume(float value);

		public abstract void SaveSFXVolume(float value);

		public abstract void SaveLocaleCode(string value);

		public abstract void SaveVSync(bool value);
	}
}
