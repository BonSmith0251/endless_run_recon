using System;
using UnityEngine;

namespace Monoblok.Progression
{
	[Serializable]
	public class IncrementalInt : IIncremental<int>
	{
		public int baseValue;

		public float waveMultiplier;

		public int Get(int waveIndex)
		{
			return Mathf.RoundToInt((float)baseValue + waveMultiplier * (float)waveIndex);
		}
	}
}
