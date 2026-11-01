using System;
using System.Collections.Generic;
using System.IO;
using Monoblok.Enemy;
using Monoblok.GameManagement;
using Monoblok.Inventory;
using Monoblok.Progression;
using Monoblok.SA;
using Monoblok.Train;
using Monoblok.Upgrades;
using NaughtyAttributes;
using UnityEngine;

namespace Monoblok
{
	public class SaveLoadManager : MonoBehaviour
	{
		public enum LoadState
		{
			Empty,
			Succesful
		}

		[SerializeField]
		[Min(0f)]
		[ReadOnly]
		private int currentVersion;

		[SerializeField]
		private bool prettyPrint = true;

		[Header("Currencies")]
		[SerializeField]
		private CurrencyDefinition[] metaCurrencies;

		[SerializeField]
		private CurrencyDefinition[] runCurrencies;

		[Header("References")]
		[SerializeField]
		private TrainRuntimeValue trainRuntimeValue;

		[SerializeField]
		private TrainConfigPool trainConfigPool;

		[SerializeField]
		private TrainConfig defaultTrainConfig;

		[SerializeField]
		private ProgressionManager progressionManager;

		[SerializeField]
		private TrainExperience trainExperience;

		[SerializeField]
		private PlayerWallet wallet;

		[SerializeField]
		private GameTimeManager timeManager;

		[SerializeField]
		private EnemyDeathCounter enemyDeathCounter;

		[SerializeField]
		private CoinCounter coinCollector;

		[SerializeField]
		private UpgradeGraphManager upgradeGraphManager;

		[SerializeField]
		private CartTradeManager cartTradeManager;

		private const string fileName = "SaveGame.ersv";

		public bool HasSave => File.Exists(GetPath());

		public bool HasRunSave
		{
			get
			{
				if (HasSave)
				{
					return ReadFile()?.hasRunSaveData ?? false;
				}
				return false;
			}
		}

		public event Action OnRunSaved;

		public void ResetProgression()
		{
			PlayerPrefs.DeleteKey(PlayingGameState.FIRST_TIME_KEY);
			DeleteSave();
			wallet.ResetAll();
			ResetRunState();
			GameSaveData data = new GameSaveData
			{
				version = currentVersion,
				hasRunSaveData = false
			};
			WriteFile(Serialize(data));
		}

		private void ResetRunState()
		{
			trainRuntimeValue.Value.Initialize(defaultTrainConfig);
			progressionManager.ResetJourney();
			timeManager.ResetTime();
			enemyDeathCounter.ResetCounter();
			coinCollector.ResetTotalCoins();
			cartTradeManager.RemoveAllRewards();
			upgradeGraphManager.LoadSaveData(new UpgradeGraphSaveData
			{
				unlockedNodes = new List<int>()
			});
		}

		[Button(null, EButtonEnableMode.Always)]
		public void SaveMeta()
		{
			GameSaveData gameSaveData = ReadFile() ?? new GameSaveData
			{
				version = currentVersion
			};
			gameSaveData.upgradeGraphSaveData = upgradeGraphManager.GetSaveData();
			gameSaveData.metaCurrencySaveData = GetCurrencySaveData(metaCurrencies);
			WriteFile(Serialize(gameSaveData));
			Debug.Log("Meta progression saved");
		}

		[Button(null, EButtonEnableMode.Always)]
		public LoadState LoadGame()
		{
			if (!TryReadValidFile(out var data))
			{
				return LoadState.Empty;
			}
			ApplyRunSaveData(data.runSaveData, data.hasRunSaveData);
			if (data.metaCurrencySaveData != null)
			{
				wallet.LoadSaveData(data.metaCurrencySaveData);
			}
			upgradeGraphManager.LoadSaveData(data.upgradeGraphSaveData);
			Debug.Log("Game loaded");
			return LoadState.Succesful;
		}

		[Button(null, EButtonEnableMode.Always)]
		public void SaveRun()
		{
			GameSaveData gameSaveData = ReadFile() ?? new GameSaveData
			{
				version = currentVersion
			};
			gameSaveData.runSaveData = BuildRunSaveData();
			gameSaveData.hasRunSaveData = true;
			WriteFile(Serialize(gameSaveData));
			this.OnRunSaved?.Invoke();
			Debug.Log("Run Saved");
		}

		[Button(null, EButtonEnableMode.Always)]
		public void LoadRun()
		{
			if (TryReadValidFile(out var data))
			{
				ApplyRunSaveData(data.runSaveData, data.hasRunSaveData);
				Debug.Log("Run loaded");
			}
		}

		[Button(null, EButtonEnableMode.Always)]
		public void DeleteRun()
		{
			GameSaveData gameSaveData = ReadFile();
			if (gameSaveData != null)
			{
				gameSaveData.hasRunSaveData = false;
				WriteFile(Serialize(gameSaveData));
				Debug.Log("Run deleted");
			}
		}

		[Button(null, EButtonEnableMode.Always)]
		public void DeleteSave()
		{
			if (HasSave)
			{
				File.Delete(GetPath());
				Debug.Log("Save deleted");
			}
		}

		private RunSaveData BuildRunSaveData()
		{
			return new RunSaveData
			{
				trainSaveData = trainRuntimeValue.Value.GetSaveData(),
				progressionSaveData = progressionManager.GetSaveData(),
				trainXPSaveData = trainExperience.GetSaveData(),
				currencySaveDatas = GetCurrencySaveData(runCurrencies),
				timeSpent = timeManager.SaveData(),
				enemyDeathCount = enemyDeathCounter.GetSaveData(),
				totalCoinsCollected = coinCollector.GetSaveData(),
				cartTradeRewardSaveData = cartTradeManager.GetSaveData()
			};
		}

		private List<CurrencySaveData> GetCurrencySaveData(IEnumerable<CurrencyDefinition> definitions)
		{
			List<CurrencySaveData> list = new List<CurrencySaveData>();
			foreach (CurrencyDefinition definition in definitions)
			{
				int count = wallet.GetCount(definition);
				CurrencySaveData item = new CurrencySaveData
				{
					id = definition.ID,
					amount = count
				};
				list.Add(item);
			}
			return list;
		}

		private void ApplyRunSaveData(RunSaveData data, bool hasRun)
		{
			if (!hasRun)
			{
				Debug.LogWarning("Trying to load run, but no run save data exists.");
				return;
			}
			TrainConfig configById = trainConfigPool.GetConfigById(data.trainSaveData.configId);
			trainRuntimeValue.Value.LoadSaveData(data.trainSaveData, configById);
			progressionManager.LoadSaveData(data.progressionSaveData);
			trainExperience.LoadSaveData(data.trainXPSaveData);
			wallet.LoadSaveData(data.currencySaveDatas);
			timeManager.LoadData(data.timeSpent);
			enemyDeathCounter.LoadSaveData(data.enemyDeathCount);
			coinCollector.LoadSaveData(data.totalCoinsCollected);
			cartTradeManager.LoadSaveData(data.cartTradeRewardSaveData);
		}

		private bool TryReadValidFile(out GameSaveData data)
		{
			data = ReadFile();
			if (data == null)
			{
				return false;
			}
			if (data.version != currentVersion)
			{
				Debug.LogWarning($"Save version mismatch (expected {currentVersion}, got {data.version}).");
				return false;
			}
			return true;
		}

		private GameSaveData ReadFile()
		{
			string path = GetPath();
			if (!File.Exists(path))
			{
				Debug.LogWarning("No save data found at " + path);
				return null;
			}
			return JsonUtility.FromJson<GameSaveData>(File.ReadAllText(path));
		}

		private string GetPath()
		{
			return Path.Combine(Application.persistentDataPath, "SaveGame.ersv");
		}

		private void WriteFile(string json)
		{
			File.WriteAllText(GetPath(), json);
		}

		private string Serialize(GameSaveData data)
		{
			return JsonUtility.ToJson(data, prettyPrint);
		}
	}
}
