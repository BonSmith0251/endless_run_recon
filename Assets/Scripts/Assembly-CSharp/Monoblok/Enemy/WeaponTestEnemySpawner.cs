using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.Pool;

namespace Monoblok.Enemy
{
	public class WeaponTestEnemySpawner : MonoBehaviour
	{
		[SerializeField]
		private EnemyConfig config;

		[SerializeField]
		private int count = 10;

		[SerializeField]
		private float radius = 10f;

		[SerializeField]
		private float respawnDelay = 3f;

		private ObjectPool<Enemy> objectPool;

		private List<Enemy> enemies = new List<Enemy>();

		private void Awake()
		{
			objectPool = new ObjectPool<Enemy>(() => Object.Instantiate(config.Prefab), null, delegate(Enemy enemy)
			{
				Tween.StopAll(enemy.transform);
				enemy.gameObject.SetActive(value: false);
			}, delegate(Enemy enemy)
			{
				if (enemy != null) Object.Destroy(enemy.gameObject);
			});
			for (int i = 0; i < count; i++)
			{
				Enemy enemy2 = Spawn(Random.insideUnitCircle * radius);
				enemy2.OnDied += OnEnemyDied;
				enemies.Add(enemy2);
			}
		}

		private void OnEnemyDied(Enemy enemy)
		{
			Tween.Delay(respawnDelay).OnComplete(this, delegate(WeaponTestEnemySpawner t)
			{
				t.Spawn(Random.insideUnitCircle * radius);
			});
		}

		private Enemy Spawn(Vector2 position)
		{
			Enemy enemy = objectPool.Get();
			enemy.Initialize(config, objectPool);
			enemy.transform.position = position;
			enemy.transform.localScale = Vector3.zero;
			enemy.gameObject.SetActive(value: true);
			Tween.Scale(enemy.transform, 1f, 0.15f, Ease.OutBack);
			return enemy;
		}
	}
}
