using System;
using Monoblok.Core.Bounds;
using Monoblok.SA;
using Monoblok.StatSystem;
using NaughtyAttributes;
using UnityEngine;

namespace Monoblok.DamageSystem
{
	public class Health : DodgeableDamageable
	{
		[SerializeField]
		private StatOrValue maxHealth;

		[SerializeField]
		private HealthRuntimeCollection observedHealths;

		[SerializeField]
		private BoundsProvider boundsProvider;

		private int health;

		private int shield;

		private int lastKnownMaxHealth;

		[ShowNativeProperty]
		public int CurrentHealth => health;

		[ShowNativeProperty]
		public int MaxHealth => (int)(maxHealth?.Value).Value;

		public float NormalizedHealth => (float)CurrentHealth / (float)MaxHealth;

		[ShowNativeProperty]
		public int CurrentShield => shield;

		public bool HasShield => shield > 0;

		public BoundsProvider BoundsProvider => boundsProvider;

		public event Action<int> OnHealed;

		public event Action<int> OnHealthChanged;

		public event Action OnHealthZero;

		public event Action<int> OnShieldChanged;

		public event Action<Damage, bool> OnDamageApplied;

		private void Awake()
		{
			ReregisterAsObserved();
		}

		private void ReregisterAsObserved()
		{
			if (!(observedHealths == null))
			{
				observedHealths.Unregister(this);
				observedHealths.Register(this);
			}
		}

		private void OnEnable()
		{
			if (Application.isPlaying && !(observedHealths == null))
			{
				observedHealths.Register(this);
			}
		}

		private void OnDisable()
		{
			if (Application.isPlaying && !(observedHealths == null))
			{
				observedHealths.Unregister(this);
			}
		}

		public void SetMaxHealth(int maxHealth, bool setCurrentHealth = false)
		{
			this.maxHealth.SetBaseValue(maxHealth);
			if (setCurrentHealth)
			{
				SetHealth(maxHealth);
			}
			lastKnownMaxHealth = maxHealth;
			ReregisterAsObserved();
		}

		public void BindStatManager(StatManager statManager)
		{
			if (!(statManager == null))
			{
				maxHealth.Bind(statManager, delegate
				{
					int num = MaxHealth;
					int num2 = ((lastKnownMaxHealth > 0) ? Mathf.RoundToInt((float)health / (float)lastKnownMaxHealth * (float)num) : health);
					lastKnownMaxHealth = num;
					SetHealth(num2);
				});
				SetHealth(MaxHealth);
				lastKnownMaxHealth = MaxHealth;
				ReregisterAsObserved();
			}
		}

		protected override (bool hitHealth, bool hitShield) HandleDamage(Damage damage)
		{
			if (health <= 0)
			{
				return (hitHealth: false, hitShield: false);
			}
			int num = damage.DamageValue;
			bool flag = shield > 0 && !damage.BypassShield;
			if (flag)
			{
				int num2 = shield - num;
				num = ((num2 < 0) ? (-num2) : 0);
				SetShield(num2);
			}
			this.OnDamageApplied?.Invoke(damage, flag);
			if (num <= 0)
			{
				return (hitHealth: false, hitShield: flag);
			}
			int num3 = health - num;
			SetHealth(num3);
			if (health <= 0)
			{
				this.OnHealthZero?.Invoke();
			}
			return (hitHealth: true, hitShield: flag);
		}

		public void Heal(int value)
		{
			int num = health + value;
			SetHealth(num);
			this.OnHealed?.Invoke(value);
		}

		public void ForceHealth(int value)
		{
			SetHealth(value);
		}

		public void AddShield(int value)
		{
			SetShield(shield + value);
		}

		public void ForceShield(int value)
		{
			SetShield(value);
		}

		private void SetHealth(int target)
		{
			health = Mathf.Clamp(target, 0, (int)(float)maxHealth);
			this.OnHealthChanged?.Invoke(health);
		}

		private void SetShield(int target)
		{
			shield = Mathf.Max(target, 0);
			this.OnShieldChanged?.Invoke(shield);
		}
	}
}
