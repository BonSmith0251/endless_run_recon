using System;

namespace Monoblok.Progression
{
	[Serializable]
	public class IncrementalFloat : IIncremental<float>
	{
		public float baseValue;

		public float waveMultiplier;

		public float Get(int waveIndex)
		{
			return baseValue + waveMultiplier * (float)waveIndex;
		}
	}
}
