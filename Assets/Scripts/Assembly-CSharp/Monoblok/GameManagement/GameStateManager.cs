using System;
using Monoblok.Core.UI;
using Monoblok.Input;
using Monoblok.StateMachine;
using Monoblok.Train;
using Monoblok.Upgrades;
using PrimeTween;
using UnityEngine;
using UnityEngine.Playables;

namespace Monoblok.GameManagement
{
	[DefaultExecutionOrder(-10)]
	public class GameStateManager : MonoBehaviour
	{
		public class GameSessionEndedCondition : ICondition
		{
			private readonly GameManager gameManager;

			public event Action OnMet;

			public GameSessionEndedCondition(GameManager gameManager)
			{
				this.gameManager = gameManager;
			}

			public void Initialize()
			{
				gameManager.OnSessionEnded += HandleSessionEnded;
			}

			public void Dispose()
			{
				gameManager.OnSessionEnded -= HandleSessionEnded;
			}

			private void HandleSessionEnded()
			{
				this.OnMet?.Invoke();
			}
		}

		[SerializeField]
		private MainMenu mainMenu;

		[SerializeField]
		private TrainConfigSelector trainConfigSelector;

		[SerializeField]
		private Monoblok.Train.Train train;

		[SerializeField]
		private TrainConfig defaultTrainConfig;

		[SerializeField]
		private CanvasGroup gameCanvasGroup;

		[SerializeField]
		private GameManager gameManager;

		[SerializeField]
		private CameraManager cameraManager;

		[SerializeField]
		private InputReader inputReader;

		[SerializeField]
		private InputManager inputManager;

		[SerializeField]
		private SaveLoadManager saveLoadManager;

		[SerializeField]
		private SettingsManager settingsManager;

		[SerializeField]
		private OneButtonModal saveMigrationModal;

		[SerializeField]
		private UpgradeGraphManager upgradeManager;

		[SerializeField]
		private Curtain curtain;

		[SerializeField]
		private OneButtonModalSequence tutorialSequence;

		[SerializeField]
		private CutscenePlayer cutscenePlayer;

		[SerializeField]
		private PlayableDirector introCutscene;

		[SerializeField]
		private TrainConfigSelectorView trainConfigSelectorView;

		[Space]
		[Header("Config")]
		[SerializeField]
		private bool bypassBootFade;

		private Monoblok.StateMachine.StateMachine stateMachine;

		private void OnEnable()
		{
			Application.wantsToQuit += HandleWantsToQuit;
		}

		private void OnDisable()
		{
			Application.wantsToQuit -= HandleWantsToQuit;
		}

		private bool HandleWantsToQuit()
		{
			Debug.Log("Application wants to quit, saving");
			saveLoadManager.SaveMeta();
			return true;
		}

		private void Awake()
		{
			Debug.Log("Game state manager awake");
			curtain.CutToBlack();
			Initialize();
			Sequence.Create().ChainDelay(2f).ChainCallback(this, delegate(GameStateManager t)
			{
				t.curtain.FadeOut();
			});
		}

		private void Initialize()
		{
			gameCanvasGroup.alpha = 0f;
			gameCanvasGroup.interactable = false;
			gameCanvasGroup.blocksRaycasts = false;
			mainMenu.gameObject.SetActive(value: false);
			settingsManager.Initialize();
			inputManager.Initialize();
			gameManager.Initialize();
			train.Initialize(defaultTrainConfig);
			if (saveLoadManager.HasSave)
			{
				saveLoadManager.LoadGame();
			}
			Tween.Delay(this, 0.1f, delegate(GameStateManager t)
			{
				t.SetupStateMachine();
			});
		}

		private void SetupStateMachine()
		{
			stateMachine = new Monoblok.StateMachine.StateMachine();
			MainMenuState mainMenuState = new MainMenuState(mainMenu, saveLoadManager, cameraManager);
			TrainConfigSelectorState trainConfigSelectorState = new TrainConfigSelectorState(trainConfigSelector, inputReader, cameraManager);
			PlayingGameState playingGameState = new PlayingGameState(gameManager, gameCanvasGroup, inputReader, new TutorialSequenceElement(tutorialSequence), new CutsceneSequenceElement(cutscenePlayer, introCutscene));
			PausedGameState pausedGameState = new PausedGameState(mainMenu);
			UpgradeState upgradeState = new UpgradeState(upgradeManager, inputReader);
			ExitGameState to = new ExitGameState(saveLoadManager);
			stateMachine.AddTransition(mainMenuState, playingGameState, new ResumeClickedCondition(mainMenu), new BlendToPlayingCamera(cameraManager));
			stateMachine.AddTransition(trainConfigSelectorState, upgradeState, new UpgradesClickedCondition(trainConfigSelectorView));
			stateMachine.AddTransition(mainMenuState, trainConfigSelectorState, new NewRunClickedCondition(mainMenu, saveLoadManager));
			stateMachine.AddTransition(mainMenuState, trainConfigSelectorState, new NewRunConfirmedCondition(mainMenu), new CurtainBlend(curtain, delegate
			{
				saveLoadManager.DeleteRun();
				gameManager.ResetGame();
				cameraManager.BlendToMainMenuCamera();
			}, 1f));
			stateMachine.AddTransition(trainConfigSelectorState, playingGameState, new TrainConfigSelectedCondition(trainConfigSelector), new BlendToPlayingCamera(cameraManager));
			stateMachine.AddTransition(trainConfigSelectorState, mainMenuState, new TrainConfigBackClickedCondition(trainConfigSelector));
			stateMachine.AddTransition(playingGameState, pausedGameState, new PausePerformedCondition(inputReader));
			stateMachine.AddTransition(pausedGameState, playingGameState, new ContinuePerformedCondition(mainMenu, inputReader));
			stateMachine.AddTransition(pausedGameState, mainMenuState, new MainMenuButtonClickedCondition(mainMenu), new CurtainBlend(curtain, delegate
			{
				gameManager.ResetGame();
				saveLoadManager.LoadRun();
				gameManager.SyncEndlessModeVisuals();
				Tween.Delay(cameraManager, 0.1f, delegate(CameraManager t)
				{
					t.BlendToMainMenuCamera();
				});
			}, 1f));
			stateMachine.AddTransition(playingGameState, upgradeState, new GameSessionEndedCondition(gameManager));
			stateMachine.AddTransition(upgradeState, trainConfigSelectorState, new ExitUpgradesCondition(upgradeManager));
			stateMachine.AddTransition(mainMenuState, to, new ExitGameClickedCondition(mainMenu));
			stateMachine.AddTransition(pausedGameState, to, new ExitGameClickedCondition(mainMenu));
			gameManager.SyncEndlessModeVisuals();
			stateMachine.SetInitialState(mainMenuState);
		}
	}
}
