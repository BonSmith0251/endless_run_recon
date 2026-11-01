using Monoblok.Core;
using PrimeTween;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	public class SpawnEnemiesState : EnemyState
	{
		private readonly SpawnEnemiesStateConfig config;

		private PrimeTween.Sequence spawnSequence;

		public SpawnEnemiesState(EnemyBrain brain, SpawnEnemiesStateConfig config)
			: base(brain, config)
		{
			this.config = config;
		}

		protected override void OnEnter()
		{
			EnemyGroup enemyGroup = config.EnemyGroup;
			if (enemyGroup == null || enemyGroup.enemyConfigs == null || enemyGroup.enemyConfigs.Length == 0)
			{
				Finish();
				return;
			}
			int randomInRange = config.MinMaxCount.GetRandomInRange();
			spawnSequence = PrimeTween.Sequence.Create();
			for (int i = 0; i < randomInRange; i++)
			{
				EnemyConfig enemyConfig = enemyGroup.enemyConfigs[Random.Range(0, enemyGroup.enemyConfigs.Length)];
				spawnSequence.ChainCallback(this, delegate(SpawnEnemiesState t)
				{
					Vector2 position = t.GetPosition(config.SpreadRadius);
					t.config.SpawnerRuntimeValue.Value.SpawnSingle(enemyConfig, position);
				});
				spawnSequence.ChainDelay(config.Delay);
			}
			spawnSequence.OnComplete(this, delegate(SpawnEnemiesState t)
			{
				t.Finish();
			});
		}

		protected override void OnExit()
		{
			if (spawnSequence.isAlive)
			{
				spawnSequence.Stop();
			}
		}

		protected override void OnUpdate(float deltaTime)
		{
		}

		private Vector2 GetPosition(float spreadRadius)
		{
			Vector2 vector = Random.insideUnitCircle * spreadRadius;
			return (Vector2)brain.transform.position + vector;
		}
	}
}
