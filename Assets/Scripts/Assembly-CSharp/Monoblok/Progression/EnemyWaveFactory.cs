using System;
using Monoblok.Enemy;
using Monoblok.SA;
using NaughtyAttributes;
using UnityEngine;

namespace Monoblok.Progression
{
	[Serializable]
	public class EnemyWaveFactory : TimedWaveFactory
	{
		[SerializeField]
		private EnemySpawnerRuntimeValue spawner;

		[SerializeField]
		private SpawnSchedule schedule;

		[SerializeField]
		[Expandable]
		private EnemyWaveSettings enemyWaveSettings;

		protected override TimedWave GetTimedWave(int waveIndex, float duration, TimedWaveData data)
		{
			float spawnRate = enemyWaveSettings.BaseSpawnRate + enemyWaveSettings.SpawnRateMultiplier * (float)waveIndex;
			return new EnemyWave(spawner.Value, schedule, spawnRate, duration, data, waveIndex);
		}
	}
}
