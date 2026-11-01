using System;
using NaughtyAttributes;
using UnityEngine;

namespace Monoblok.Progression
{
	[Serializable]
	public class TimedWaveFactory : IWaveFactory
	{
		[SerializeField]
		[Expandable]
		private TimedWaveSettings settings;

		[SerializeField]
		private TimedWaveData data;

		public Wave GetWave(int waveIndex)
		{
			float duration = Mathf.Min(settings.BaseDuration + (float)waveIndex * settings.DurationIncrement, settings.MaxDuration);
			return GetTimedWave(waveIndex, duration, data);
		}

		protected virtual TimedWave GetTimedWave(int waveIndex, float duration, TimedWaveData data)
		{
			return new TimedWave(duration, data, waveIndex);
		}
	}
}
