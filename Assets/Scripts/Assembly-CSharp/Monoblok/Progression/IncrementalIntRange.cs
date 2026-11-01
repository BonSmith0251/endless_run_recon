using System;
using Monoblok.Core;
using UnityEngine;

namespace Monoblok.Progression
{
	[Serializable]
	public class IncrementalIntRange : IIncremental<int>
	{
		public Vector2Int baseValue;

		public float waveMultiplier;

		public int Get(int waveIndex)
		{
			int x = Mathf.RoundToInt((float)baseValue.x + waveMultiplier * (float)waveIndex);
			int y = Mathf.RoundToInt((float)baseValue.y + waveMultiplier * (float)waveIndex);
			return new Vector2Int(x, y).GetRandomInRange();
		}
	}
}
