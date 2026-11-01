using Monoblok.Environment;
using Monoblok.SA;
using NaughtyAttributes;
using UnityEngine;

namespace Monoblok.Progression
{
	public class ProgressionManager : MonoBehaviour
	{
		[SerializeField]
		[Expandable]
		private WavePattern wavePattern;

		[SerializeField]
		private Progression progression;

		[SerializeField]
		private EnvironmentController environmentController;

		[Header("Game Events")]
		[SerializeField]
		private GameEvent journeyStartedEvent;

		[SerializeField]
		private GameEvent journeyEndedEvent;

		[SerializeField]
		private GameEvent waveStartedEvent;

		[SerializeField]
		private GameEvent waveEndedEvent;

		public Progression Progression => progression;

		private void OnEnable()
		{
			progression.OnJourneyStarted += HandleJourneyStarted;
			progression.OnJourneyEnded += HandleJourneyEnded;
			progression.OnWaveStarted += HandleWaveStarted;
			progression.OnWaveEnded += HandleWaveEnded;
		}

		private void OnDisable()
		{
			progression.OnJourneyStarted -= HandleJourneyStarted;
			progression.OnJourneyEnded -= HandleJourneyEnded;
			progression.OnWaveStarted -= HandleWaveStarted;
			progression.OnWaveEnded -= HandleWaveEnded;
		}

		private void HandleJourneyStarted()
		{
			if (!(journeyStartedEvent == null))
			{
				journeyStartedEvent.TriggerGameEvent();
			}
		}

		private void HandleJourneyEnded()
		{
			if (!(journeyEndedEvent == null))
			{
				journeyEndedEvent.TriggerGameEvent();
			}
		}

		private void HandleWaveStarted()
		{
			if (!(waveStartedEvent == null))
			{
				waveStartedEvent.TriggerGameEvent();
			}
		}

		private void HandleWaveEnded()
		{
			if (!(waveEndedEvent == null))
			{
				waveEndedEvent.TriggerGameEvent();
			}
		}

		public void Initialize()
		{
			progression.Initialize();
		}

		private void Update()
		{
			progression.UpdateSimulation(Time.deltaTime);
		}

		public void RunNextWave()
		{
			Wave wave = wavePattern.GetWave(progression.CurrentWaveIndex);
			progression.RunWave(wave);
		}

		public void EndWave()
		{
			progression.EndWave();
		}

		public void ResetJourney()
		{
			progression.Initialize();
			environmentController.ResetMembers();
		}

		public ProgressionSaveData GetSaveData()
		{
			return progression.GetSaveData();
		}

		public void LoadSaveData(ProgressionSaveData data)
		{
			progression.LoadSaveData(data);
			environmentController.InitializeMembers();
		}
	}
}
