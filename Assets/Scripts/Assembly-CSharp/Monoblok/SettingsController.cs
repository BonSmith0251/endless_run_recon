using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;

namespace Monoblok
{
	public class SettingsController
	{
		private readonly GameSettings model;

		private readonly SettingsView view;

		private readonly List<Locale> locales;

		private readonly List<Resolution> resolutions;

		private readonly List<FullScreenMode> displayModes;

		public SettingsController(GameSettings model, SettingsView view)
		{
			this.model = model;
			this.view = view;
			locales = model.AvailableLocales.ToList();
			resolutions = model.AvailableResolutions.ToList();
			displayModes = view.DisplayModes.ToList();
			view.InitializeLanguages(locales.Select((Locale l) => l.LocaleName), locales.IndexOf(model.Locale));
			view.InitializeResolutions(resolutions.Select((Resolution r) => r.width + " x " + r.height + " @ " + r.refreshRateRatio.value.ToString("F2") + "Hz"), resolutions.IndexOf(model.Resolution));
			view.InitializeDisplayModes(displayModes.IndexOf(model.FullScreenMode));
			view.InitializeVSync(model.VSyncEnabled);
			view.InitializeMasterVolume(model.MasterVolume);
			view.InitializeMusicVolume(model.MusicVolume);
			view.InitializeSFXVolume(model.SFXVolume);
			view.OnLanguageIndexChanged += OnLanguageIndexChanged;
			view.OnResolutionIndexChanged += OnResolutionIndexChanged;
			view.OnDisplayModeIndexChanged += OnDisplayModeIndexChanged;
			view.OnVSyncChanged += OnVSyncChanged;
			view.OnMasterVolumeChanged += OnMasterVolumeChanged;
			view.OnMusicVolumeChanged += OnMusicVolumeChanged;
			view.OnSFXVolumeChanged += OnSFXVolumeChanged;
			view.OnDeleteSaveConfirmed += OnDeleteSaveConfirmed;
			view.OnCloseClicked += OnSaveClicked;
			model.OnLocaleChanged += OnLocaleChanged;
			model.OnResolutionChanged += OnResolutionChanged;
			model.OnFullScreenModeChanged += OnFullScreenModeChanged;
			model.OnVSyncChanged += OnModelVSyncChanged;
			model.OnMasterVolumeChanged += OnModelMasterVolumeChanged;
			model.OnMusicVolumeChanged += OnModelMusicVolumeChanged;
			model.OnSFXVolumeChanged += OnModelSFXVolumeChanged;
		}

		public void Dispose()
		{
			view.OnLanguageIndexChanged -= OnLanguageIndexChanged;
			view.OnResolutionIndexChanged -= OnResolutionIndexChanged;
			view.OnDisplayModeIndexChanged -= OnDisplayModeIndexChanged;
			view.OnVSyncChanged -= OnVSyncChanged;
			view.OnMasterVolumeChanged -= OnMasterVolumeChanged;
			view.OnMusicVolumeChanged -= OnMusicVolumeChanged;
			view.OnSFXVolumeChanged -= OnSFXVolumeChanged;
			view.OnDeleteSaveConfirmed -= OnDeleteSaveConfirmed;
			view.OnCloseClicked -= OnSaveClicked;
			model.OnLocaleChanged -= OnLocaleChanged;
			model.OnResolutionChanged -= OnResolutionChanged;
			model.OnFullScreenModeChanged -= OnFullScreenModeChanged;
			model.OnVSyncChanged -= OnModelVSyncChanged;
			model.OnMasterVolumeChanged -= OnModelMasterVolumeChanged;
			model.OnMusicVolumeChanged -= OnModelMusicVolumeChanged;
			model.OnSFXVolumeChanged -= OnModelSFXVolumeChanged;
		}

		private void OnLanguageIndexChanged(int index)
		{
			model.SetLocale(locales[index]);
		}

		private void OnResolutionIndexChanged(int index)
		{
			model.SetResolution(resolutions[index]);
		}

		private void OnDisplayModeIndexChanged(int index)
		{
			model.SetFullScreenMode(displayModes[index]);
		}

		private void OnVSyncChanged(bool enabled)
		{
			model.SetVSync(enabled);
		}

		private void OnMasterVolumeChanged(float value)
		{
			model.SetMasterVolume(value);
		}

		private void OnMusicVolumeChanged(float value)
		{
			model.SetMusicVolume(value);
		}

		private void OnSFXVolumeChanged(float value)
		{
			model.SetSFXVolume(value);
		}

		private void OnDeleteSaveConfirmed()
		{
			model.ResetProgress();
		}

		private void OnSaveClicked()
		{
			model.SaveAll();
		}

		private void OnLocaleChanged(Locale locale)
		{
			view.SetLanguageIndex(locales.IndexOf(locale));
			view.InitializeDisplayModes(displayModes.IndexOf(model.FullScreenMode));
			view.InitializeVSync(model.VSyncEnabled);
		}

		private void OnResolutionChanged(Resolution resolution)
		{
			view.SetResolutionIndex(resolutions.IndexOf(resolution));
		}

		private void OnFullScreenModeChanged(FullScreenMode mode)
		{
			view.SetDisplayModeIndex(displayModes.IndexOf(mode));
		}

		private void OnModelVSyncChanged(bool enabled)
		{
			view.SetVSync(enabled);
		}

		private void OnModelMasterVolumeChanged(float value)
		{
			view.SetMasterVolume(value);
		}

		private void OnModelMusicVolumeChanged(float value)
		{
			view.SetMusicVolume(value);
		}

		private void OnModelSFXVolumeChanged(float value)
		{
			view.SetSFXVolume(value);
		}
	}
}
