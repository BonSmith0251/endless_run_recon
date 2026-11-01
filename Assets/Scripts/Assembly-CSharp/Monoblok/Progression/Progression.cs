using System;
using NaughtyAttributes;
using UnityEngine;

namespace Monoblok.Progression
{
	[CreateAssetMenu(menuName = "Game/Progression/Progression Asset")]
	public class Progression : ScriptableObject
	{
		private enum State
		{
			Idle,
			RampingUp,
			Wave,
			RampingDown
		}

		[SerializeField]
		private AnimationCurve rampUpCurve;

		[SerializeField]
		private AnimationCurve rampDownCurve;

		[SerializeField]
		private float rampUpDuration = 3f;

		[SerializeField]
		private float rampDownDuration = 3f;

		[SerializeField]
		private float peakSpeed = 10f;

		private float rampDownArea;

		private int stationsReached;

		private float globalDistance;

		private float speed;

		private float journeyDistance;

		private float phaseTime;

		[SerializeField]
		private int waveIndex;

		private Wave currentWave;

		private State state;

		private bool isRunning;

		public int StationsReached => stationsReached;

		[ShowNativeProperty]
		public float GlobalDistance => globalDistance;

		[ShowNativeProperty]
		public float Speed => speed;

		[ShowNativeProperty]
		public float JourneyDistance => journeyDistance;

		public float NormalizedSpeed => speed / peakSpeed;

		public bool IsRunning => isRunning;

		public bool WaveInProgress => state == State.Wave;

		public int CurrentWaveIndex => waveIndex;

		public float RampDownDistance => rampDownArea * rampDownDuration * peakSpeed;

		public event Action OnInitialized;

		public event Action OnJourneyStarted;

		public event Action OnJourneyEnded;

		public event Action OnWaveStarted;

		public event Action OnWaveEnded;

		private void OnEnable()
		{
			CacheIntegrals();
		}

		private void OnValidate()
		{
			CacheIntegrals();
		}

		public void Initialize()
		{
			if (currentWave != null)
			{
				currentWave.OnComplete -= HandleWaveComplete;
				currentWave.EndWave();
			}
			stationsReached = 0;
			globalDistance = 0f;
			journeyDistance = 0f;
			phaseTime = 0f;
			waveIndex = 0;
			isRunning = false;
			state = State.Idle;
			speed = 0f;
			currentWave = null;
			this.OnInitialized?.Invoke();
		}

		public void RunWave(Wave wave)
		{
			currentWave = wave;
			journeyDistance = 0f;
			phaseTime = 0f;
			state = State.RampingUp;
			isRunning = true;
			this.OnJourneyStarted?.Invoke();
		}

		public void EndWave()
		{
			if (currentWave != null)
			{
				currentWave.EndWave();
			}
		}

		public void UpdateSimulation(float deltaTime)
		{
			if (isRunning)
			{
				switch (state)
				{
				case State.RampingUp:
					UpdateRampUp(deltaTime);
					break;
				case State.Wave:
					UpdateWave(deltaTime);
					break;
				case State.RampingDown:
					UpdateRampDown(deltaTime);
					break;
				}
			}
		}

		private void CacheIntegrals()
		{
			if (rampDownCurve != null)
			{
				rampDownArea = IntegrateCurve(rampDownCurve);
			}
		}

		private float IntegrateCurve(AnimationCurve curve, int samples = 1024)
		{
			float num = 0f;
			float num2 = 1f / (float)samples;
			for (int i = 0; i < samples; i++)
			{
				float time = ((float)i + 0.5f) * num2;
				num += curve.Evaluate(time) * num2;
			}
			return num;
		}

		private void UpdateRampUp(float deltaTime)
		{
			phaseTime += deltaTime;
			speed = rampUpCurve.Evaluate(Mathf.Clamp01(phaseTime / rampUpDuration)) * peakSpeed;
			Accumulate(deltaTime);
			if (phaseTime >= rampUpDuration)
			{
				BeginWave();
			}
		}

		private void UpdateWave(float deltaTime)
		{
			speed = peakSpeed;
			Accumulate(deltaTime);
			currentWave.UpdateWave(deltaTime);
		}

		private void UpdateRampDown(float deltaTime)
		{
			phaseTime += deltaTime;
			speed = rampDownCurve.Evaluate(Mathf.Clamp01(phaseTime / rampDownDuration)) * peakSpeed;
			Accumulate(deltaTime);
			if (phaseTime >= rampDownDuration)
			{
				EndJourney();
			}
		}

		private void Accumulate(float deltaTime)
		{
			float num = speed * deltaTime;
			journeyDistance += num;
			globalDistance += num;
		}

		private void BeginWave()
		{
			state = State.Wave;
			currentWave.OnComplete += HandleWaveComplete;
			currentWave.StartWave();
			this.OnWaveStarted?.Invoke();
		}

		private void HandleWaveComplete()
		{
			currentWave.OnComplete -= HandleWaveComplete;
			state = State.RampingDown;
			phaseTime = 0f;
			this.OnWaveEnded?.Invoke();
		}

		private void EndJourney()
		{
			speed = 0f;
			stationsReached++;
			waveIndex++;
			isRunning = false;
			state = State.Idle;
			this.OnJourneyEnded?.Invoke();
		}

		public ProgressionSaveData GetSaveData()
		{
			return new ProgressionSaveData
			{
				globalDistance = globalDistance,
				waveCount = waveIndex,
				stationsReached = stationsReached
			};
		}

		public void LoadSaveData(ProgressionSaveData saveData)
		{
			waveIndex = saveData.waveCount;
			stationsReached = saveData.stationsReached;
		}
	}
}
