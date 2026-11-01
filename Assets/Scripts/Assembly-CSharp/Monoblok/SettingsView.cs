using System;
using System.Collections.Generic;
using System.Linq;
using Monoblok.Core.UI;
using Monoblok.Input;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Monoblok
{
	[RequireComponent(typeof(CanvasGroup))]
	public class SettingsView : MonoBehaviour
	{
		private enum SettingsState
		{
			Default,
			Controls
		}

		[Serializable]
		public class DisplayModeOption
		{
			[field: SerializeField]
			public FullScreenMode Mode { get; private set; }

			[field: SerializeField]
			public LocalizedString Label { get; private set; }
		}

		private SettingsState state;

		[SerializeField]
		private CanvasGroup canvasGroup;

		[SerializeField]
		private TMP_Dropdown languageDropdown;

		[SerializeField]
		private TMP_Dropdown resolutionDropdown;

		[SerializeField]
		private TMP_Dropdown displayModeDropdown;

		[SerializeField]
		private TMP_Dropdown vSyncDropdown;

		[SerializeField]
		private Slider masterVolumeSlider;

		[SerializeField]
		private Slider musicVolumeSlider;

		[SerializeField]
		private Slider sfxVolumeSlider;

		[SerializeField]
		private Button deleteSaveButton;

		[SerializeField]
		private YesNoModal confirmDeleteSaveModal;

		[SerializeField]
		private Button controlsButton;

		[SerializeField]
		private Button backButton;

		[Space]
		[SerializeField]
		private GameObject mainView;

		[SerializeField]
		private GameObject controlsView;

		[Space]
		[Header("Display Mode Options")]
		[SerializeField]
		private List<DisplayModeOption> displayModeOptions = new List<DisplayModeOption>();

		[Space]
		[Header("VSync Options")]
		[SerializeField]
		private LocalizedString vSyncOnLabel;

		[SerializeField]
		private LocalizedString vSyncOffLabel;

		private const int VSyncOnIndex = 0;

		private const int VSyncOffIndex = 1;

		public IReadOnlyList<FullScreenMode> DisplayModes => displayModeOptions.Select((DisplayModeOption o) => o.Mode).ToList();

		public event Action<int> OnLanguageIndexChanged;

		public event Action<int> OnResolutionIndexChanged;

		public event Action<int> OnDisplayModeIndexChanged;

		public event Action<bool> OnVSyncChanged;

		public event Action<float> OnMasterVolumeChanged;

		public event Action<float> OnMusicVolumeChanged;

		public event Action<float> OnSFXVolumeChanged;

		public event Action OnDeleteSaveConfirmed;

		public event Action OnCloseClicked;

		private void OnValidate()
		{
			if (!(canvasGroup != null))
			{
				canvasGroup = GetComponent<CanvasGroup>();
			}
		}

		private void OnEnable()
		{
			languageDropdown.onValueChanged.AddListener(delegate(int index)
			{
				this.OnLanguageIndexChanged?.Invoke(index);
			});
			resolutionDropdown.onValueChanged.AddListener(delegate(int index)
			{
				this.OnResolutionIndexChanged?.Invoke(index);
			});
			displayModeDropdown.onValueChanged.AddListener(delegate(int index)
			{
				this.OnDisplayModeIndexChanged?.Invoke(index);
			});
			vSyncDropdown.onValueChanged.AddListener(delegate(int index)
			{
				this.OnVSyncChanged?.Invoke(index == 0);
			});
			masterVolumeSlider.onValueChanged.AddListener(delegate(float value)
			{
				this.OnMasterVolumeChanged?.Invoke(value);
			});
			musicVolumeSlider.onValueChanged.AddListener(delegate(float value)
			{
				this.OnMusicVolumeChanged?.Invoke(value);
			});
			sfxVolumeSlider.onValueChanged.AddListener(delegate(float value)
			{
				this.OnSFXVolumeChanged?.Invoke(value);
			});
			deleteSaveButton.onClick.AddListener(HandleDeleteSaveClicked);
			controlsButton.onClick.AddListener(HandleControlsClicked);
			backButton.onClick.AddListener(HandleBackClicked);
		}

		private void OnDisable()
		{
			languageDropdown.onValueChanged.RemoveAllListeners();
			resolutionDropdown.onValueChanged.RemoveAllListeners();
			displayModeDropdown.onValueChanged.RemoveAllListeners();
			vSyncDropdown.onValueChanged.RemoveAllListeners();
			masterVolumeSlider.onValueChanged.RemoveAllListeners();
			musicVolumeSlider.onValueChanged.RemoveAllListeners();
			sfxVolumeSlider.onValueChanged.RemoveAllListeners();
			deleteSaveButton.onClick.RemoveAllListeners();
			controlsButton.onClick.RemoveAllListeners();
			backButton.onClick.RemoveAllListeners();
		}

		private void Awake()
		{
			Hide();
		}

		private void HandleBackClicked()
		{
			if (state == SettingsState.Controls)
			{
				SetState(SettingsState.Default);
			}
			else
			{
				this.OnCloseClicked?.Invoke();
			}
		}

		private void SetState(SettingsState state)
		{
			switch (state)
			{
			case SettingsState.Default:
				controlsView.SetActive(value: false);
				mainView.SetActive(value: true);
				FocusFirst();
				break;
			case SettingsState.Controls:
				controlsView.SetActive(value: true);
				mainView.SetActive(value: false);
				break;
			}
			this.state = state;
		}

		private void HandleDeleteSaveClicked()
		{
			EnableAllSelectables(enabled: false);
			confirmDeleteSaveModal.Show(delegate
			{
				this.OnDeleteSaveConfirmed?.Invoke();
				EnableAllSelectables(enabled: true);
				FocusFirst();
			}, delegate
			{
				EnableAllSelectables(enabled: true);
				FocusFirst();
			});
		}

		private void EnableAllSelectables(bool enabled)
		{
			Selectable[] componentsInChildren = mainView.GetComponentsInChildren<Selectable>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].interactable = enabled;
			}
			backButton.interactable = enabled;
		}

		private void FocusFirst()
		{
			if (InputManager.CurrentDevice == InputManager.InputDeviceType.Gamepad)
			{
				EventSystem.current.SetSelectedGameObject(mainView.GetComponentInChildren<Selectable>().gameObject);
			}
		}

		private void HandleControlsClicked()
		{
			SetState(SettingsState.Controls);
		}

		public void Show(bool midGame)
		{
			base.gameObject.SetActive(value: true);
			canvasGroup.alpha = 1f;
			canvasGroup.blocksRaycasts = true;
			canvasGroup.interactable = true;
			deleteSaveButton.interactable = !midGame;
			languageDropdown.interactable = !midGame;
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
			canvasGroup.alpha = 0f;
			canvasGroup.blocksRaycasts = false;
			canvasGroup.interactable = false;
		}

		public void InitializeLanguages(IEnumerable<string> options, int currentIndex)
		{
			languageDropdown.ClearOptions();
			languageDropdown.AddOptions(options.ToList());
			languageDropdown.SetValueWithoutNotify(currentIndex);
			languageDropdown.RefreshShownValue();
		}

		public void InitializeResolutions(IEnumerable<string> options, int currentIndex)
		{
			resolutionDropdown.ClearOptions();
			resolutionDropdown.AddOptions(options.ToList());
			resolutionDropdown.SetValueWithoutNotify(currentIndex);
			resolutionDropdown.RefreshShownValue();
		}

		public void InitializeDisplayModes(int currentIndex)
		{
			displayModeDropdown.ClearOptions();
			displayModeDropdown.AddOptions(displayModeOptions.Select((DisplayModeOption o) => o.Label.GetLocalizedString()).ToList());
			displayModeDropdown.SetValueWithoutNotify(currentIndex);
			displayModeDropdown.RefreshShownValue();
		}

		public void SetLanguageIndex(int index)
		{
			languageDropdown.SetValueWithoutNotify(index);
		}

		public void SetResolutionIndex(int index)
		{
			resolutionDropdown.SetValueWithoutNotify(index);
		}

		public void SetDisplayModeIndex(int index)
		{
			displayModeDropdown.SetValueWithoutNotify(index);
		}

		public void InitializeVSync(bool enabled)
		{
			vSyncDropdown.ClearOptions();
			vSyncDropdown.AddOptions(new List<string>
			{
				vSyncOnLabel.GetLocalizedString(),
				vSyncOffLabel.GetLocalizedString()
			});
			vSyncDropdown.SetValueWithoutNotify((!enabled) ? 1 : 0);
			vSyncDropdown.RefreshShownValue();
		}

		public void SetVSync(bool enabled)
		{
			vSyncDropdown.SetValueWithoutNotify((!enabled) ? 1 : 0);
		}

		public void InitializeMasterVolume(float value)
		{
			InitializeSlider(masterVolumeSlider, value);
		}

		public void InitializeMusicVolume(float value)
		{
			InitializeSlider(musicVolumeSlider, value);
		}

		public void InitializeSFXVolume(float value)
		{
			InitializeSlider(sfxVolumeSlider, value);
		}

		public void SetMasterVolume(float value)
		{
			masterVolumeSlider.SetValueWithoutNotify(value);
		}

		public void SetMusicVolume(float value)
		{
			musicVolumeSlider.SetValueWithoutNotify(value);
		}

		public void SetSFXVolume(float value)
		{
			sfxVolumeSlider.SetValueWithoutNotify(value);
		}

		private static void InitializeSlider(Slider slider, float value)
		{
			slider.minValue = 0f;
			slider.maxValue = 1f;
			slider.wholeNumbers = false;
			slider.SetValueWithoutNotify(value);
		}
	}
}
