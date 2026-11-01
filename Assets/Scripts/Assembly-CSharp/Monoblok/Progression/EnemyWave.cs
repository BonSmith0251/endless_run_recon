using Monoblok.Enemy;

namespace Monoblok.Progression
{
	public class EnemyWave : TimedWave
	{
		private readonly EnemySpawner spawner;

		private readonly SpawnSchedule schedule;

		private readonly float spawnRate;

		private float spawnTime;

		public EnemyWave(EnemySpawner spawner, SpawnSchedule schedule, float spawnRate, float duration, TimedWaveData data, int waveIndex)
			: base(duration, data, waveIndex)
		{
			this.spawner = spawner;
			this.schedule = schedule;
			this.spawnRate = spawnRate;
		}

		protected override void OnStartWave()
		{
			base.OnStartWave();
		}

		protected override void OnUpdateWave(float deltaTime)
		{
			base.OnUpdateWave(deltaTime);
			spawnTime += deltaTime;
			float num = 1f / spawnRate;
			while (spawnTime >= num && !base.IsComplete)
			{
				EnemyGroup enemyGroup = schedule.Get(spawner.GetPatternWave());
				spawner.SpawnGroup(enemyGroup);
				spawnTime -= num;
			}
		}

		protected override void OnWaveComplete()
		{
			base.OnWaveComplete();
			spawner.Clear();
		}
	}
}
