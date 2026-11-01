using System;
using Monoblok.Enemy;
using Monoblok.SA;
using NaughtyAttributes;
using UnityEngine;

namespace Monoblok.Progression
{
	[Serializable]
	public class BossWaveFactory : IWaveFactory
	{
		[SerializeField]
		private EnemyConfig bossConfig;

		[SerializeField]
		[Expandable]
		[AllowNesting]
		private SpawnPatternConfig pattern;

		[SerializeField]
		private EnemySpawnerRuntimeValue spawnerValue;

		[SerializeField]
		private BossIntroductionRuntimeValue introductionValue;

		[SerializeField]
		private BossCelebrationRuntimeValue celebrationValue;

		[SerializeField]
		private BossWaveData data;

		public Wave GetWave(int waveIndex)
		{
			return new BossWave(bossConfig, pattern, spawnerValue.Value, introductionValue.Value, celebrationValue.Value, data, waveIndex);
		}
	}
}
