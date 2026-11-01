using System;
using Monoblok.DroneSystem;
using Monoblok.Enemy;
using Monoblok.Input;
using Monoblok.Inventory;
using Monoblok.Progression;
using Monoblok.SA;
using Monoblok.StateMachine;
using Monoblok.Train;
using Monoblok.Upgrades;
using UnityEngine;

namespace Monoblok.GameManagement
{
	public class GameManager : MonoBehaviour
	{
		[Header("References")]
		[SerializeField]
		private Monoblok.Train.Train train;

		[SerializeField]
		private MetaCurrencyCalculatorSO metaCurrencyCalculator;

		[SerializeField]
		private PlayerWallet wallet;

		[SerializeField]
		private StationManager stationManager;

		[SerializeField]
		private ProgressionManager progressionManager;

		[SerializeField]
		private EnemySpawner enemySpawner;

		[SerializeField]
		private DropCoins dropCoins;

		[SerializeField]
		private JourneyAdvancer journeyAdvancer;

		[SerializeField]
		private EndGameScreen endGameScreen;

		[SerializeField]
		private CameraManager cameraManager;

		[SerializeField]
		private Curtain curtain;

		[SerializeField]
		private Shop shop;

		[SerializeField]
		private WinLoseConditionManager winLoseConditionManager;

		[SerializeField]
		private UpgradeGraphManager upgradeGraphManager;

		[SerializeField]
		private InputReader inputReader;

		[SerializeField]
		private SaveLoadManager saveLoadManager;

		[SerializeField]
		private InventoryManager inventoryManager;

		[SerializeField]
		private CartMergeManager mergeManager;

		[SerializeField]
		private PlayerDrones playerDrones;

		[SerializeField]
		private AbilityManager abilityManager;

		[SerializeField]
		private GameTimeManager gameTimeManager;

		[SerializeField]
		private EnemyDeathCounter enemyDeathCounter;

		[SerializeField]
		private CoinCounter coinCollector;

		[SerializeField]
		private EndlessModeTransition endlessModeTransition;

		[SerializeField]
		private CartTradeManager cartTradeManager;

		[Header("Canvas elements")]
		[SerializeField]
		private GameObject waveCanvasGO;

		[SerializeField]
		private GameObject stationCanvasGO;

		[Header("Game Events")]
		[SerializeField]
		private GameEvent runCompletedEvent;

		[SerializeField]
		private GameEvent runLostEvent;

		[SerializeField]
		private GameEvent sunSoftWinEvent;

		private Monoblok.StateMachine.StateMachine stateMachine;

		private EndGameState endGameState;

		private bool gameInProgress;

		private bool initialized;

		public event Action OnGameStarted;

		public event Action OnGameReset;

		public event Action<EndGameSummary> OnGameEnded;

		public event Action OnSessionEnded;

		public void Initialize()
		{
			if (!initialized)
			{
				stateMachine = new Monoblok.StateMachine.StateMachine();
				abilityManager.Initialize();
				inventoryManager.Initialize();
				upgradeGraphManager.Initialize();
				progressionManager.Initialize();
				waveCanvasGO.SetActive(value: false);
				stationCanvasGO.SetActive(value: false);
				IdleState idleState = new IdleState(new IdleStateContext(train, stationManager, winLoseConditionManager, progressionManager, inventoryManager, mergeManager, abilityManager));
				JourneyState journeyState = new JourneyState(inputReader, progressionManager, abilityManager, saveLoadManager, waveCanvasGO);
				StationState stationState = new StationState(inputReader, stationManager, stationCanvasGO);
				endGameState = new EndGameState(endGameScreen, wallet, inputReader);
				new EndSessionState(this);
				stateMachine.AddTransition(idleState, journeyState, new GameStartedCondition(this));
				stateMachine.AddTransition(journeyState, stationState, new JourneyEndedCondition(progressionManager.Progression), new BlendToStationCamera(cameraManager));
				stateMachine.AddTransition(stationState, journeyState, new JourneyAdvancedCondition(journeyAdvancer), new BlendToPlayingCamera(cameraManager));
				stateMachine.AddTransition(journeyState, endGameState, new GameEndedCondition(this), new BlendChain(new GlobalControlsBlend(inputReader, enable: false), new TimeScaleBlend(0f)));
				BlendChain completable = new BlendChain(new TimeScaleBlend(1f), new CurtainBlend(curtain, delegate
				{
					ResetGame();
					EndSession();
				}, 1f));
				stateMachine.AddTransition(endGameState, idleState, new EndGameScreenDismissedCondition(endGameScreen), completable);
				BlendChain completable2 = new BlendChain(new TimeScaleBlend(1f, 0f), new EndlessModeBlend(endlessModeTransition), new BlendToStationCamera(cameraManager));
				stateMachine.AddTransition(endGameState, stationState, new EndlessModeClickedCondition(endGameScreen), completable2);
				stateMachine.AddTransition(stationState, idleState, new GameResetCondition(this));
				stateMachine.AddTransition(journeyState, idleState, new GameResetCondition(this));
				stateMachine.SetInitialState(idleState);
				endGameScreen.OnEndlessClicked += HandleEndlessModeClicked;
				initialized = true;
			}
		}

		public void StartGame()
		{
			Debug.Log("Start game");
			if (!gameInProgress)
			{
				gameInProgress = true;
				shop.Initialize();
				gameTimeManager.StartTime();
				cartTradeManager.RemoveAllRewards();
				winLoseConditionManager.Initialize();
				winLoseConditionManager.OnSoftWin += HandleSoftWin;
				winLoseConditionManager.OnLose += HandleLose;
				SyncEndlessModeVisuals();
				this.OnGameStarted?.Invoke();
				Debug.Log("Game started");
			}
		}

		public void SyncEndlessModeVisuals()
		{
			if (winLoseConditionManager.IsEndlessModeActive)
			{
				endlessModeTransition.ExecuteBlend();
			}
		}

		public void EndSession()
		{
			saveLoadManager.SaveMeta();
			saveLoadManager.DeleteRun();
			this.OnSessionEnded?.Invoke();
		}

		public void ResetGame()
		{
			Debug.Log("Game reset");
			dropCoins.ClearCoins();
			enemySpawner.Clear();
			playerDrones.ResetPosition();
			progressionManager.ResetJourney();
			mergeManager.CancelSelection();
			mergeManager.ClearConnections();
			wallet.ResetCurrency("Coin");
			cartTradeManager.RemoveAllRewards();
			train.Initialize(train.Config);
			winLoseConditionManager.OnSoftWin -= HandleSoftWin;
			winLoseConditionManager.OnLose -= HandleLose;
			gameTimeManager.ResetTime();
			enemyDeathCounter.ResetCounter();
			coinCollector.ResetTotalCoins();
			endlessModeTransition.UndoBlend();
			gameInProgress = false;
			this.OnGameReset?.Invoke();
		}

		private void HandleSoftWin()
		{
			EndGameSummary summary = BuildSummary(winOrLose: true);
			EndGame(summary);
			sunSoftWinEvent.TriggerGameEvent();
		}

		private void HandleLose()
		{
			EndGameSummary summary = BuildSummary(winOrLose: false);
			EndGame(summary);
			runLostEvent.TriggerGameEvent();
		}

		private EndGameSummary BuildSummary(bool winOrLose)
		{
			gameTimeManager.StopTime();
			return new EndGameSummary
			{
				state = ((!winOrLose) ? EndGameSummary.State.Lost : EndGameSummary.State.SoftWin),
				gainedLevels = train.TrainLevel,
				metaCurrency = metaCurrencyCalculator.Calculate(),
				coinsCollected = coinCollector.TotalCoinsCollected,
				timeSpent = gameTimeManager.TimeSpent,
				stationsReached = progressionManager.Progression.StationsReached,
				enemiesDefeated = enemyDeathCounter.Counter
			};
		}

		private void EndGame(EndGameSummary summary)
		{
			if (gameInProgress)
			{
				progressionManager.EndWave();
				endGameState.UpdateSummary(summary);
				gameTimeManager.StopTime();
				runCompletedEvent.TriggerGameEvent();
				this.OnGameEnded?.Invoke(summary);
			}
		}

		private void HandleEndlessModeClicked()
		{
			gameTimeManager.StartTime();
		}
	}
}
