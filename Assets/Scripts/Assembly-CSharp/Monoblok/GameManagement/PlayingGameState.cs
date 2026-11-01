using Monoblok.Core;
using Monoblok.Input;
using Monoblok.StateMachine;
using UnityEngine;

namespace Monoblok.GameManagement
{
	public class PlayingGameState : IState
	{
		private readonly CanvasGroup gameCanvasGroup;

		private readonly GameManager gameManager;

		private readonly InputReader inputReader;

		public static readonly string FIRST_TIME_KEY = "First_Time";

		private Sequence firstTimeSequence;

		public PlayingGameState(GameManager gameManager, CanvasGroup gameCanvasGroup, InputReader inputReader, TutorialSequenceElement tutorialElement, CutsceneSequenceElement introCutsceneElement)
		{
			this.gameManager = gameManager;
			this.gameCanvasGroup = gameCanvasGroup;
			this.inputReader = inputReader;
			ISequenceElement[] elements = new ISequenceElement[2] { introCutsceneElement, tutorialElement };
			firstTimeSequence = new Sequence(elements);
		}

		public void Enter()
		{
			Debug.Log("Enter playing state");
			if (!PlayerPrefs.HasKey(FIRST_TIME_KEY) || PlayerPrefs.GetInt(FIRST_TIME_KEY) == 0)
			{
				inputReader.EnableCutsceneControls(enable: true);
				firstTimeSequence.OnComplete += HandleFirstTimeSequenceComplete;
				firstTimeSequence.Run();
			}
			else
			{
				StartGame();
			}
		}

		private void HandleFirstTimeSequenceComplete()
		{
			inputReader.EnableCutsceneControls(enable: false);
			firstTimeSequence.OnComplete -= HandleFirstTimeSequenceComplete;
			PlayerPrefs.SetInt(FIRST_TIME_KEY, 1);
			StartGame();
		}

		private void StartGame()
		{
			gameCanvasGroup.gameObject.SetActive(value: true);
			gameCanvasGroup.alpha = 1f;
			gameCanvasGroup.interactable = true;
			gameCanvasGroup.blocksRaycasts = true;
			gameManager.StartGame();
			inputReader.EnableGlobalControls(enable: true);
			InputDeviceChangeSelector componentInChildren = gameCanvasGroup.GetComponentInChildren<InputDeviceChangeSelector>(includeInactive: false);
			if (componentInChildren != null)
			{
				componentInChildren.OnEnable();
			}
		}

		public void Update(float deltaTime)
		{
		}

		public void Exit()
		{
			gameCanvasGroup.alpha = 0f;
			gameCanvasGroup.interactable = false;
			gameCanvasGroup.blocksRaycasts = false;
		}
	}
}
