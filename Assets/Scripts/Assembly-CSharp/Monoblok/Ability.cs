using System;
using FMODUnity;
using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok
{
	public abstract class Ability : IDisposable
	{
		private readonly AbilityConfig config;

		private readonly Stat cooldown;

		private readonly Stat maxCharges;

		private int currentCharges;

		private float cooldownTimer;

		protected StatManager StatManager { get; }

		public AbilityConfig Config => config;

		public string AbilityName => config.AbilityName;

		public string AbilityDescription => config.AbilityDescription;

		public Sprite Icon => config.Icon;

		public bool HasCharges => currentCharges > 0;

		public bool IsOnCooldown => cooldownTimer > 0f;

		public int CurrentCharges => currentCharges;

		public int MaxCharges => (int)maxCharges.Value;

		public float CooldownProgress
		{
			get
			{
				if (!(cooldown.Value > 0f))
				{
					return 1f;
				}
				return 1f - cooldownTimer / cooldown.Value;
			}
		}

		public float CooldownRemaining => Mathf.Max(cooldownTimer, 0f);

		public event Action OnChargesChanged;

		public event Action<bool> OnCooldownChanged;

		public event Action OnChargeRestored;

		protected Ability(AbilityConfig config)
		{
			this.config = config;
			StatManager = config.StatManager;
			cooldown = StatManager.AcquireStat(config.Cooldown);
			maxCharges = StatManager.AcquireStat(config.MaxCharges);
			cooldown.OnValueChanged += delegate
			{
				cooldownTimer = 0f;
			};
			maxCharges.OnValueChanged += delegate
			{
				RestoreCharge();
			};
			currentCharges = MaxCharges;
		}

		public bool TryUse()
		{
			if (!HasCharges)
			{
				return false;
			}
			currentCharges--;
			this.OnChargesChanged?.Invoke();
			if (config.PerformEvent != null)
			{
				config.PerformEvent.TriggerGameEvent();
			}
			Execute();
			RuntimeManager.PlayOneShot(config.PerformRef);
			if (currentCharges == 0)
			{
				cooldownTimer = cooldown.Value;
				this.OnCooldownChanged?.Invoke(obj: true);
			}
			return true;
		}

		public void Tick(float deltaTime)
		{
			if (IsOnCooldown)
			{
				cooldownTimer -= deltaTime;
				if (cooldownTimer <= 0f)
				{
					cooldownTimer = 0f;
					RestoreCharge();
				}
			}
		}

		private void RestoreCharge()
		{
			int num = MaxCharges;
			if (currentCharges < num)
			{
				currentCharges = num;
				this.OnChargeRestored?.Invoke();
				this.OnChargesChanged?.Invoke();
				this.OnCooldownChanged?.Invoke(obj: false);
			}
		}

		protected abstract void Execute();

		public void Dispose()
		{
			StatManager.ReleaseStat(config.Cooldown.StatDefinition);
			StatManager.ReleaseStat(config.MaxCharges.StatDefinition);
			ReleaseStats();
		}

		protected virtual void ReleaseStats()
		{
		}

		public virtual void CancelAbility()
		{
		}

		public void Restore()
		{
			RestoreCharge();
			cooldownTimer = 0f;
		}
	}
}
