using System.Collections.Generic;
using Monoblok.Progression;
using PrimeTween;
using UnityEngine;
using UnityEngine.Pool;

namespace Monoblok.Enemy
{
	public class EnemySpawner : MonoBehaviour
	{
		private sealed class SpawnGroupState
		{
			public EnemySpawner Spawner;

			public EnemyConfig[] Configs;

			public SpawnPoint[] SpawnPoints;

			public Sequence Seq;

			public int NextIndex;

			public void SpawnNext()
			{
				Spawner.SpawnSingle(Configs[NextIndex], SpawnPoints[NextIndex].position);
				NextIndex++;
			}

			public void OnGroupComplete()
			{
				Spawner.spawnSeqs.Remove(Seq);
			}
		}

		[SerializeField]
		private EnemyScalingConfig scalingConfig;

		[SerializeField]
		private Monoblok.Progression.Progression progression;

		[Tooltip("Wave at which enemy scaling starts stepping up once per lap.")]
		[SerializeField]
		private int loopStartWave;

		[Tooltip("Number of waves per scaling lap once looping begins.")]
		[SerializeField]
		private int loopLength = 20;

		private Camera mainCamera;

		private Dictionary<EnemyConfig, ObjectPool<Enemy>> pools = new Dictionary<EnemyConfig, ObjectPool<Enemy>>();

		private List<Enemy> enemies = new List<Enemy>();

		private List<Sequence> spawnSeqs = new List<Sequence>();

		private void Awake()
		{
			mainCamera = Camera.main;
		}

		private ObjectPool<Enemy> GetOrCreatePool(EnemyConfig config)
		{
			if (!pools.TryGetValue(config, out var value))
			{
				ObjectPool<Enemy> objectPool = new ObjectPool<Enemy>(() => Object.Instantiate(config.Prefab), delegate(Enemy enemy)
				{
					enemy.gameObject.SetActive(value: true);
				}, delegate(Enemy enemy)
				{
					Tween.StopAll(enemy.transform);
					enemy.gameObject.SetActive(value: false);
				}, delegate(Enemy enemy)
				{
					if (enemy != null) Object.Destroy(enemy.gameObject);
				});
				pools[config] = objectPool;
				return objectPool;
			}
			return value;
		}

		private void OnEnemyReleased(Enemy enemy)
		{
			enemy.OnReleased -= OnEnemyReleased;
			enemies.Remove(enemy);
		}

		public int GetPatternWave()
		{
			ComputeLoop(out var patternWave, out var _);
			return patternWave;
		}

		private void ComputeLoop(out int patternWave, out int loopCount)
		{
			int currentWaveIndex = progression.CurrentWaveIndex;
			if (loopLength <= 0 || currentWaveIndex < loopStartWave + loopLength)
			{
				patternWave = currentWaveIndex;
				loopCount = 0;
			}
			else
			{
				int num = currentWaveIndex - loopStartWave;
				loopCount = num / loopLength;
				patternWave = loopStartWave + num % loopLength;
			}
		}

		public void SpawnGroup(EnemyGroup enemyGroup)
		{
			if (enemyGroup == null)
			{
				return;
			}
			SpawnPoint[] spawnPoints = enemyGroup.spawnPattern.GetSpawnPoints(mainCamera, base.transform, enemyGroup.enemyConfigs.Length);
			SpawnGroupState spawnGroupState = new SpawnGroupState
			{
				Spawner = this,
				Configs = enemyGroup.enemyConfigs,
				SpawnPoints = spawnPoints
			};
			Sequence sequence = Sequence.Create();
			for (int i = 0; i < enemyGroup.enemyConfigs.Length; i++)
			{
				sequence.ChainDelay(spawnPoints[i].delay);
				sequence.ChainCallback(spawnGroupState, delegate(SpawnGroupState t)
				{
					t.SpawnNext();
				});
			}
			spawnGroupState.Seq = sequence;
			sequence.OnComplete(spawnGroupState, delegate(SpawnGroupState t)
			{
				t.OnGroupComplete();
			});
			spawnSeqs.Add(sequence);
		}

		public Enemy SpawnSingle(EnemyConfig config, Vector2 position)
		{
			if (!progression.WaveInProgress)
			{
				return null;
			}
			ObjectPool<Enemy> orCreatePool = GetOrCreatePool(config);
			Enemy enemy = orCreatePool.Get();
			ComputeLoop(out var _, out var loopCount);
			enemy.transform.position = position;
			enemy.transform.localScale = Vector3.zero;
			enemy.Initialize(config, loopCount, scalingConfig, orCreatePool);
			enemy.OnReleased += OnEnemyReleased;
			Tween.Scale(enemy.transform, 1f, 0.3f, Ease.OutBack);
			enemies.Add(enemy);
			return enemy;
		}

		public Enemy SpawnSingle(EnemyConfig config, SpawnPatternConfig spawnPatternConfig)
		{
			SpawnPoint spawnPoint = spawnPatternConfig.GetSpawnPoints(mainCamera, base.transform, 1)[0];
			return SpawnSingle(config, spawnPoint.position);
		}

		public void Clear()
		{
			foreach (Sequence spawnSeq in spawnSeqs)
			{
				spawnSeq.Stop();
			}
			spawnSeqs.Clear();
			for (int num = enemies.Count - 1; num >= 0; num--)
			{
				Enemy enemy = enemies[num];
				if ((bool)enemy)
				{
					enemy.DisableCollider();
					enemy.OnReleased -= OnEnemyReleased;
					Tween.Scale(enemy.transform, 0f, 0.3f, Ease.InBack).OnComplete(enemy, delegate(Enemy t)
					{
						t.ReleaseToPool();
					});
				}
			}
			enemies.Clear();
		}
	}
}
