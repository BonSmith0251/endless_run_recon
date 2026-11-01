using UnityEngine;

namespace Monoblok
{
	public class SettingsManager : MonoBehaviour
	{
		[SerializeField]
		private SaveLoadManager saveLoadManager;

		[SerializeField]
		private SettingsView settingsView;

		private SettingsController controller;

		public void Initialize()
		{
			controller = new SettingsController(new GameSettings(new PlayerPrefsGameSettingsSaveService(), saveLoadManager), settingsView);
		}

		private void OnDestroy()
		{
			controller.Dispose();
		}
	}
}
