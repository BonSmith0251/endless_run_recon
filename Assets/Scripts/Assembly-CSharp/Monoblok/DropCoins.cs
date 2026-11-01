using System.Collections.Generic;
using Monoblok.Enemy;
using Monoblok.Progression;
using Monoblok.SA;
using Monoblok.StatSystem;
using NaughtyAttributes;
using PrimeTween;
using UnityEngine;
using UnityEngine.Pool;

namespace Monoblok
{
	public class DropCoins : MonoBehaviour
	{
		private struct CoinJump
		{
			public Vector3 Start;

			public Vector3 Apex;

			public Vector3 End;

			public float Duration;

			public float Elapsed;
		}

		[SerializeField]
		private EnemyGameEvent enemyDiedEvent;

		[SerializeField]
		private CoinCollectable prefab;

		[SerializeField]
		private CoinCollectable metaCoinPrefab;

		[SerializeField]
		private Monoblok.Progression.Progression progression;

		[SerializeField]
		private GlobalStatManager globalStatManager;

		[SerializeField]
		private StatOrValue coinMultiplier;

		[SerializeField]
		private float coinLifetime = 5f;

		[SerializeField]
		[Min(0f)]
		private float coinLifetimeJitter = 0.5f;

		[SerializeField]
		[Min(0f)]
		private float clearCoinsJitter = 0.15f;

		[SerializeField]
		private GameArea gameArea;

		[SerializeField]
		[Min(0f)]
		private int coinPoolPrewarmCount = 30;

		[SerializeField]
		[Min(0f)]
		private int metaCoinPoolPrewarmCount = 5;

		private const float JumpDuration = 0.15f;

		private const float JumpHeight = 0.5f;

		private ObjectPool<CoinCollectable> pool;

		private ObjectPool<CoinCollectable> metaCoinPool;

		private List<CoinCollectable> activeCoins = new List<CoinCollectable>();

		private Dictionary<CoinCollectable, CoinJump> jumpSequences = new Dictionary<CoinCollectable, CoinJump>();

		private List<CoinCollectable> jumpUpdateBuffer = new List<CoinCollectable>();

		private void OnEnable()
		{
			enemyDiedEvent.OnTriggered += HandleEnemyDied;
			progression.OnWaveEnded += HandleWaveEnded;
		}

		private void OnDisable()
		{
			enemyDiedEvent.OnTriggered -= HandleEnemyDied;
			progression.OnWaveEnded -= HandleWaveEnded;
		}

		[Button(null, EButtonEnableMode.Always)]
		private void StressTest()
		{
			for (int i = 0; i < 1000; i++)
			{
				SpawnCoin(pool, gameArea.GetRandomPosition());
			}
		}

		private void Awake()
		{
			pool = new ObjectPool<CoinCollectable>(() => Object.Instantiate(prefab), delegate(CoinCollectable coin)
			{
				coin.gameObject.SetActive(value: true);
			}, HandleCoinReleased, delegate(CoinCollectable coin)
			{
				if (coin != null)
					Object.Destroy(coin.gameObject);
			});
			metaCoinPool = new ObjectPool<CoinCollectable>(() => Object.Instantiate(metaCoinPrefab), delegate(CoinCollectable coin)
			{
				coin.gameObject.SetActive(value: true);
			}, HandleCoinReleased, delegate(CoinCollectable coin)
			{
				if (coin != null)
					Object.Destroy(coin.gameObject);
			});
			coinMultiplier.Bind(globalStatManager.StatManager);
			Prewarm(pool, coinPoolPrewarmCount);
			Prewarm(metaCoinPool, metaCoinPoolPrewarmCount);
		}

		private void HandleCoinReleased(CoinCollectable coin)
		{
			coin.gameObject.SetActive(value: false);
			activeCoins.Remove(coin);
			jumpSequences.Remove(coin);
		}

		private static void Prewarm(ObjectPool<CoinCollectable> sourcePool, int count)
		{
			CoinCollectable[] array = new CoinCollectable[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = sourcePool.Get();
			}
			for (int j = 0; j < count; j++)
			{
				sourcePool.Release(array[j]);
			}
		}

		private void HandleEnemyDied(Monoblok.Enemy.Enemy enemy)
		{
			int num = Mathf.RoundToInt((float)enemy.Coin * coinMultiplier.Value);
			for (int i = 0; i < num; i++)
			{
				SpawnCoin(pool, enemy.transform.position);
			}
			if (enemy.MetaCoinAmount > 0 && Random.value <= enemy.MetaCoinChance)
			{
				for (int j = 0; j < enemy.MetaCoinAmount; j++)
				{
					SpawnCoin(metaCoinPool, enemy.transform.position);
				}
			}
		}

		private void SpawnCoin(ObjectPool<CoinCollectable> sourcePool, Vector3 position)
		{
			CoinCollectable coinCollectable = sourcePool.Get();
			float lifetime = Mathf.Max(0f, coinLifetime + Random.Range(0f - coinLifetimeJitter, coinLifetimeJitter));
			coinCollectable.Initialize(sourcePool, lifetime);
			coinCollectable.transform.position = position;
			activeCoins.Add(coinCollectable);
			if (!gameArea.Bounds.Contains(position))
			{
				Vector3 center = gameArea.Bounds.center;
				Vector3 vector = gameArea.Bounds.ClosestPoint(position);
				Vector3 normalized = (center - vector).normalized;
				position += normalized * 5f;
			}
			Vector3 end = (Vector2)position + Random.insideUnitCircle * 3f;
			coinCollectable.EnableCollider(enable: false);
			Vector3 position2 = coinCollectable.transform.position;
			Vector3 apex = new Vector3((position2.x + end.x) / 2f, Mathf.Max(position2.y, end.y) + 0.5f, end.z);
			jumpSequences[coinCollectable] = new CoinJump
			{
				Start = position2,
				Apex = apex,
				End = end,
				Duration = 0.15f,
				Elapsed = 0f
			};
		}

		private void Update()
		{
			if (jumpSequences.Count == 0)
			{
				return;
			}
			jumpUpdateBuffer.Clear();
			jumpUpdateBuffer.AddRange(jumpSequences.Keys);
			foreach (CoinCollectable item in jumpUpdateBuffer)
			{
				if (jumpSequences.TryGetValue(item, out var value))
				{
					value.Elapsed += Time.deltaTime;
					float num = value.Duration / 2f;
					Vector3 position;
					if (value.Elapsed < num)
					{
						float t = ((num > 0f) ? (value.Elapsed / num) : 1f);
						position = Vector3.LerpUnclamped(value.Start, value.Apex, EaseOutQuad(t));
					}
					else
					{
						float t2 = ((num > 0f) ? Mathf.Clamp01((value.Elapsed - num) / num) : 1f);
						position = Vector3.LerpUnclamped(value.Apex, value.End, EaseInQuad(t2));
					}
					item.transform.position = position;
					if (value.Elapsed >= value.Duration)
					{
						item.transform.position = value.End;
						item.EnableCollider(enable: true);
						jumpSequences.Remove(item);
					}
					else
					{
						jumpSequences[item] = value;
					}
				}
			}
		}

		private static float EaseOutQuad(float t)
		{
			return t * (2f - t);
		}

		private static float EaseInQuad(float t)
		{
			return t * t;
		}

		private void HandleWaveEnded()
		{
			ClearCoins();
		}

		public void ClearCoins()
		{
			for (int num = activeCoins.Count - 1; num >= 0; num--)
			{
				CoinCollectable coinCollectable = activeCoins[num];
				if ((bool)coinCollectable)
				{
					jumpSequences.Remove(coinCollectable);
					coinCollectable.CancelExpiration();
					float startDelay = Random.Range(0f, clearCoinsJitter);
					Tween.Scale(coinCollectable.transform, 0f, 0.3f, Ease.InBack, 1, CycleMode.Restart, startDelay).OnComplete(coinCollectable, delegate(CoinCollectable t)
					{
						t.ReleaseToPool();
					});
				}
			}
			activeCoins.Clear();
			jumpSequences.Clear();
		}
	}
}
