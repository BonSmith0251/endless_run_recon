using Monoblok.Core;
using UnityEngine;

namespace Monoblok.Enemy
{
	[CreateAssetMenu(menuName = "Game/Enemy/Spawn Group")]
	public class SpawnGroup : ScriptableObject
	{
		[SerializeField]
		private bool mixEnemies;

		[SerializeField]
		private EnemyEntry[] enemies;

		[SerializeField]
		private Vector2Int count = new Vector2Int(1, 1);

		private float[] _weights;

		private void OnEnable()
		{
			_weights = new float[enemies.Length];
		}

		public EnemyGroup Get()
		{
			int randomInRange = count.GetRandomInRange();
			EnemyEntry enemyEntry = PickEntry();
			EnemyConfig[] array = new EnemyConfig[randomInRange];
			if (mixEnemies)
			{
				for (int i = 0; i < randomInRange; i++)
				{
					array[i] = PickEntry().config;
				}
			}
			else
			{
				for (int j = 0; j < randomInRange; j++)
				{
					array[j] = enemyEntry.config;
				}
			}
			return new EnemyGroup
			{
				enemyConfigs = array,
				spawnPattern = enemyEntry.pattern
			};
		}

		private EnemyEntry PickEntry()
		{
			float num = 0f;
			for (int i = 0; i < enemies.Length; i++)
			{
				_weights[i] = Mathf.Max(0f, enemies[i].weight);
				num += _weights[i];
			}
			if (num <= 0f)
			{
				return enemies[0];
			}
			float num2 = Random.Range(0f, num);
			float num3 = 0f;
			for (int j = 0; j < enemies.Length; j++)
			{
				num3 += _weights[j];
				if (num2 <= num3)
				{
					return enemies[j];
				}
			}
			return enemies[enemies.Length - 1];
		}
	}
}
