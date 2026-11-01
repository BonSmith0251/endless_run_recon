using System.Collections.Generic;

namespace Monoblok.Enemy.AI
{
	public class SpawnAddsToKillState : EnemyState
	{
		private readonly SpawnAddsToKillStateConfig config;

		private readonly List<Enemy> enemies = new List<Enemy>();

		public SpawnAddsToKillState(EnemyBrain brain, SpawnAddsToKillStateConfig config)
			: base(brain, config)
		{
			this.config = config;
		}

		protected override void OnEnter()
		{
			enemies.Clear();
			SpawnAddsToKillStateConfig.AddGroup[] groups = config.Groups;
			foreach (SpawnAddsToKillStateConfig.AddGroup addGroup in groups)
			{
				Enemy enemy = config.SpawnerValue.Value.SpawnSingle(addGroup.config, addGroup.pattern);
				enemies.Add(enemy);
				enemy.OnDied += HandleEnemyDied;
			}
		}

		private void HandleEnemyDied(Enemy enemy)
		{
			enemy.OnDied -= HandleEnemyDied;
			enemies.Remove(enemy);
			if (enemies.Count == 0)
			{
				Finish();
			}
		}

		protected override void OnExit()
		{
			foreach (Enemy enemy in enemies)
			{
				if (!(enemy == null))
				{
					enemy.OnDied -= HandleEnemyDied;
				}
			}
		}

		protected override void OnUpdate(float deltaTime)
		{
		}
	}
}
