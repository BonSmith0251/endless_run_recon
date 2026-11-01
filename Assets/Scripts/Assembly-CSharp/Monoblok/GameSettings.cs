using System;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Monoblok
{
	public class GameSettings
	{
		private const string MasterVCAPath = "vca:/Master";

		private const string MusicVCAPath = "vca:/Music";

		private const string SFXVCAPath = "vca:/SFX";

		private readonly SaveLoadManager saveLoadManager;

		private readonly GameSettingsSaveService saveService;

		public IEnumerable<Locale> AvailableLocales => LocalizationSettings.AvailableLocales.Locales;

		public Locale Locale { get; private set; }

		public IEnumerable<Resolution> AvailableResolutions => Screen.resolutions;

		public Resolution Resolution { get; private set; }

		public FullScreenMode FullScreenMode { get; private set; }

		public float MasterVolume { get; private set; }

		public float MusicVolume { get; private set; }

		public float SFXVolume { get; private set; }

		public bool VSyncEnabled { get; private set; }

		public event Action<Locale> OnLocaleChanged;

		public event Action<Resolution> OnResolutionChanged;

		public event Action<FullScreenMode> OnFullScreenModeChanged;

		public event Action<float> OnMasterVolumeChanged;

		public event Action<float> OnMusicVolumeChanged;

		public event Action<float> OnSFXVolumeChanged;

		public event Action<bool> OnVSyncChanged;

		public GameSettings(GameSettingsSaveService saveService, SaveLoadManager saveLoadManager)
		{
			this.saveService = saveService;
			this.saveLoadManager = saveLoadManager;
			MasterVolume = saveService.LoadMasterVolume(0.6f);
			MusicVolume = saveService.LoadMusicVolume();
			SFXVolume = saveService.LoadSFXVolume();
			SetVCAVolume("vca:/Master", MasterVolume);
			SetVCAVolume("vca:/Music", MusicVolume);
			SetVCAVolume("vca:/SFX", SFXVolume);
			Resolution = Screen.currentResolution;
			FullScreenMode = Screen.fullScreenMode;
			VSyncEnabled = saveService.LoadVSync(QualitySettings.vSyncCount > 0);
			QualitySettings.vSyncCount = (VSyncEnabled ? 1 : 0);
			string savedLocaleCode = saveService.LoadLocaleCode();
			Locale = AvailableLocales.FirstOrDefault((Locale l) => l.Identifier.Code == savedLocaleCode) ?? LocalizationSettings.SelectedLocale;
			LocalizationSettings.SelectedLocale = Locale;
		}

		public void SetLocale(Locale locale)
		{
			Locale = locale;
			LocalizationSettings.SelectedLocale = locale;
			this.OnLocaleChanged?.Invoke(locale);
		}

		public void SetResolution(Resolution resolution)
		{
			Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode, resolution.refreshRateRatio);
			this.OnResolutionChanged?.Invoke(resolution);
		}

		public void SetFullScreenMode(FullScreenMode fullScreenMode)
		{
			Screen.fullScreenMode = fullScreenMode;
			this.OnFullScreenModeChanged?.Invoke(fullScreenMode);
		}

		public void SetMasterVolume(float volume)
		{
			MasterVolume = Mathf.Clamp01(volume);
			SetVCAVolume("vca:/Master", MasterVolume);
			this.OnMasterVolumeChanged?.Invoke(MasterVolume);
		}

		public void SetMusicVolume(float volume)
		{
			MusicVolume = Mathf.Clamp01(volume);
			SetVCAVolume("vca:/Music", MusicVolume);
			this.OnMusicVolumeChanged?.Invoke(MusicVolume);
		}

		public void SetSFXVolume(float volume)
		{
			SFXVolume = Mathf.Clamp01(volume);
			SetVCAVolume("vca:/SFX", SFXVolume);
			this.OnSFXVolumeChanged?.Invoke(SFXVolume);
		}

		public void SetVSync(bool enabled)
		{
			VSyncEnabled = enabled;
			QualitySettings.vSyncCount = (enabled ? 1 : 0);
			this.OnVSyncChanged?.Invoke(enabled);
		}

		private static void SetVCAVolume(string path, float volume)
		{
			RuntimeManager.GetVCA(path).setVolume(volume);
		}

		public void ResetProgress()
		{
			saveLoadManager.ResetProgression();
		}

		public void SaveAll()
		{
			saveService.SaveMasterVolume(MasterVolume);
			saveService.SaveMusicVolume(MusicVolume);
			saveService.SaveSFXVolume(SFXVolume);
			saveService.SaveLocaleCode(Locale.Identifier.Code);
			saveService.SaveVSync(VSyncEnabled);
		}
	}
}
