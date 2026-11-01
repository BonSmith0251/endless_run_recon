using System;
using Monoblok.Progression;
using Monoblok.SA;
using UnityEngine;

namespace Monoblok.GameManagement
{
	public class WinLoseConditionManager : MonoBehaviour
	{
		[SerializeField]
		private Monoblok.Progression.Progression progression;

		[SerializeField]
		private TrainRuntimeValue trainRuntimeValue;

		[SerializeField]
		private int waveCount = 20;

		private IWinOrLoseCondition softWinCondition;

		private IWinOrLoseCondition loseCondition;

		public bool IsEndlessModeActive => progression.CurrentWaveIndex >= waveCount;

		public event Action OnSoftWin;

		public event Action OnLose;

		private void OnDestroy()
		{
			DisposeConditions();
		}

		public void Initialize()
		{
			DisposeConditions();
			if (progression.CurrentWaveIndex < waveCount)
			{
				softWinCondition = new WaveReachedCondition(progression, waveCount);
				softWinCondition.OnConditionMet += OnSoftWinConditionMet;
				softWinCondition.Initialize();
			}
			loseCondition = new TrainDestroyedCondition(trainRuntimeValue);
			loseCondition.OnConditionMet += OnLoseConditionMet;
			loseCondition.Initialize();
			Debug.Log("Initialized winlose");
		}

		private void OnSoftWinConditionMet()
		{
			this.OnSoftWin?.Invoke();
			softWinCondition.OnConditionMet -= OnSoftWinConditionMet;
			softWinCondition.Dispose();
		}

		private void OnLoseConditionMet()
		{
			Debug.Log("Lose condition met");
			this.OnLose?.Invoke();
		}

		private void DisposeConditions()
		{
			softWinCondition?.Dispose();
			loseCondition?.Dispose();
			softWinCondition = null;
			loseCondition = null;
		}
	}
}
