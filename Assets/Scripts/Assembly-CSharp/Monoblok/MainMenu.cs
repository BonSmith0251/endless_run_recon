using System;
using System.Linq;
using Monoblok.Core.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Monoblok
{
	public class MainMenu : MonoBehaviour
	{
		public enum MainMenuState
		{
			Initial_SavePresent,
			Initial_NoSavePresent,
			Paused
		}

		[Header("Menu Buttons")]
		[SerializeField]
		private Button newRunButton;

		[SerializeField]
		private Button continueButton;

		[SerializeField]
		private Button optionsButton;

		[SerializeField]
		private Button mainMenuButton;

		[SerializeField]
		private Button exitButton;

		[SerializeField]
		private Button creditsButton;

		[SerializeField]
		private YesNoModal confirmNewRunModal;

		[Space]
		[Header("Other")]
		[SerializeField]
		private SettingsView settingsView;

		[SerializeField]
		private Credits credits;

		private MainMenuState state;

		public event Action OnNewRunClicked;

		public event Action OnResumeRunClicked;

		public event Action OnMainMenuClicked;

		public event Action OnExitClicked;

		public event Action OnNewRunConfirmed;

		private void OnEnable()
		{
			newRunButton.onClick.AddListener(HandleNewRunClicked);
			continueButton.onClick.AddListener(delegate
			{
				this.OnResumeRunClicked?.Invoke();
			});
			mainMenuButton.onClick.AddListener(delegate
			{
				this.OnMainMenuClicked?.Invoke();
			});
			optionsButton.onClick.AddListener(OpenSettings);
			creditsButton.onClick.AddListener(OpenCredits);
			exitButton.onClick.AddListener(delegate
			{
				this.OnExitClicked?.Invoke();
			});
		}

		private void OnDisable()
		{
			newRunButton.onClick.RemoveAllListeners();
			continueButton.onClick.RemoveAllListeners();
			mainMenuButton.onClick.RemoveAllListeners();
			optionsButton.onClick.RemoveAllListeners();
			creditsButton.onClick.RemoveAllListeners();
			exitButton.onClick.RemoveAllListeners();
		}

		private void OpenCredits()
		{
			credits.gameObject.SetActive(value: true);
			credits.OnBackCLicked += HandleCreditsBackClicked;
			base.gameObject.SetActive(value: false);
		}

		private void HandleCreditsBackClicked()
		{
			credits.gameObject.SetActive(value: false);
			credits.OnBackCLicked -= HandleCreditsBackClicked;
			base.gameObject.SetActive(value: true);
		}

		private void OpenSettings()
		{
			bool midGame = state == MainMenuState.Paused;
			settingsView.Show(midGame);
			settingsView.OnCloseClicked += OnCloseClicked;
			base.gameObject.SetActive(value: false);
		}

		private void OnCloseClicked()
		{
			settingsView.Hide();
			settingsView.OnCloseClicked -= OnCloseClicked;
			base.gameObject.SetActive(value: true);
		}

		private void HandleNewRunClicked()
		{
			switch (state)
			{
			case MainMenuState.Initial_SavePresent:
				confirmNewRunModal.Show(delegate
				{
					this.OnNewRunConfirmed?.Invoke();
				}, delegate
				{
					FocusFirst();
				});
				break;
			case MainMenuState.Initial_NoSavePresent:
				this.OnNewRunClicked?.Invoke();
				break;
			case MainMenuState.Paused:
				throw new InvalidOperationException("User shouldn't be able to click new run when paused");
			default:
				throw new NotSupportedException("Action not supported");
			}
		}

		public void SetState(MainMenuState newState)
		{
			state = newState;
			switch (state)
			{
			case MainMenuState.Initial_SavePresent:
				mainMenuButton.gameObject.SetActive(value: false);
				newRunButton.gameObject.SetActive(value: true);
				continueButton.gameObject.SetActive(value: true);
				exitButton.gameObject.SetActive(value: true);
				break;
			case MainMenuState.Initial_NoSavePresent:
				mainMenuButton.gameObject.SetActive(value: false);
				newRunButton.gameObject.SetActive(value: true);
				continueButton.gameObject.SetActive(value: false);
				exitButton.gameObject.SetActive(value: true);
				break;
			case MainMenuState.Paused:
				mainMenuButton.gameObject.SetActive(value: true);
				newRunButton.gameObject.SetActive(value: false);
				continueButton.gameObject.SetActive(value: true);
				exitButton.gameObject.SetActive(value: false);
				break;
			}
		}

		public void Show()
		{
			base.gameObject.SetActive(value: true);
			FocusFirst();
		}

		public void FocusFirst()
		{
			Selectable selectable = GetComponentsInChildren<Selectable>().FirstOrDefault((Selectable s) => s.interactable);
			EventSystem.current.SetSelectedGameObject(selectable.gameObject);
		}

		public void Hide()
		{
			settingsView.Hide();
			base.gameObject.SetActive(value: false);
		}
	}
}
