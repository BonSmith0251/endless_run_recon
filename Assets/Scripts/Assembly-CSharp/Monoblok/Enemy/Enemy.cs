using System;
using Monoblok.DamageSystem;
using Monoblok.EffectSystem;
using Monoblok.Enemy.AI;
using Monoblok.Movement;
using Monoblok.SA;
using Monoblok.StatSystem;
using UnityEngine;
using UnityEngine.Pool;

namespace Monoblok.Enemy
{
	public class Enemy : MonoBehaviour
	{
		[SerializeField]
		private BoxCollider2D boxCollider;

		[SerializeField]
		private Health health;

		[SerializeField]
		private EnemyBrain brain;

		[SerializeField]
		private TransformBasedMovement movement;

		[SerializeField]
		private SpriteRenderer spriteRenderer;

		[SerializeField]
		private StatManager statManager;

		[SerializeField]
		private EnemyRuntimeCollection allEnemies;

		[SerializeField]
		private EnemyGameEvent diedGameEvent;

		[SerializeField]
		private EffectHandler effectHandler;

		[SerializeField]
		private TriggerDamageDealer damageDealer;

		private EnemyConfig config;

		private int coin;

		private int xp;

		private float metaCoinChance;

		private int metaCoinAmount;

		private bool isDead;

		private ObjectPool<Enemy> pool;

		private bool isReleased;

		public EnemyConfig Config => config;

		public Health Health => health;

		public bool IsDead => isDead;

		public int Coin => coin;

		public int XP => xp;

		public float MetaCoinChance => metaCoinChance;

		public int MetaCoinAmount => metaCoinAmount;

		public SpriteRenderer SpriteRenderer => spriteRenderer;

		public event Action<Enemy> OnDied;

		public event Action<Enemy> OnReleased;

		private void OnValidate()
		{
			if (damageDealer == null)
			{
				damageDealer = GetComponent<TriggerDamageDealer>();
			}
			if (boxCollider == null)
			{
				boxCollider = GetComponent<BoxCollider2D>();
			}
		}

		private void OnEnable()
		{
			health.OnHealthZero += Die;
			allEnemies.Register(this);
		}

		private void OnDisable()
		{
			health.OnHealthZero -= Die;
			allEnemies.Unregister(this);
		}

		public void DisableCollider()
		{
			if (!(boxCollider == null))
			{
				boxCollider.enabled = false;
			}
		}

		public void Initialize(EnemyConfig config, ObjectPool<Enemy> pool = null)
		{
			Initialize(config, 0, null, pool);
		}

		public void Initialize(EnemyConfig config, int loopCount, EnemyScalingConfig scaling, ObjectPool<Enemy> pool = null)
		{
			this.config = config;
			if (boxCollider != null)
			{
				boxCollider.enabled = true;
			}
			isDead = false;
			isReleased = false;
			effectHandler.ClearEffects();
			statManager.Initialize(config.Stats);
			int maxHealth = ((scaling != null) ? scaling.GetHealth(config, loopCount) : config.Health);
			int value = ((scaling != null) ? scaling.GetShield(config, loopCount) : config.Shield);
			int damage = ((scaling != null) ? scaling.GetDamage(config, loopCount) : config.Damage);
			if (damageDealer != null)
			{
				damageDealer.Initialize(damage);
			}
			health.SetMaxHealth(maxHealth, setCurrentHealth: true);
			health.ForceShield(value);
			coin = ((scaling != null) ? scaling.GetCoins(config, loopCount) : config.Coins);
			xp = ((scaling != null) ? scaling.GetXP(config, loopCount) : config.XP);
			metaCoinChance = config.MetaCoinChance;
			metaCoinAmount = config.MetaCoinAmount;
			brain.Initialize(config.BehaviorConfig);
			this.pool = pool;
		}

		private void Die()
		{
			if (!isDead)
			{
				isDead = true;
				brain.Stop();
				diedGameEvent.TriggerGameEvent(this);
				this.OnDied?.Invoke(this);
				ReleaseToPool();
			}
		}

		public void ReleaseToPool()
		{
			if (!isReleased)
			{
				isReleased = true;
				brain.Stop();
				this.OnReleased?.Invoke(this);
				if (pool != null)
				{
					ObjectPool<Enemy> objectPool = pool;
					pool = null;
					objectPool.Release(this);
				}
				else
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}
		}
	}
}
