using Monoblok.DamageSystem;
using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok.Train
{
	public class KnockbackModuleBehavior : ModuleBehavior
	{
		[SerializeField]
		private CustomSizeKnockback knockback;

		private StatInitializer damageInitializer;

		private float cooldownTime;

		private float t;

		private bool cooldown;

		public void Initialize(Sprite sprite, float radius, float cooldownTime, StatInitializer damageInitializer)
		{
			spriteRenderer.sprite = sprite;
			knockback.radius = radius;
			this.cooldownTime = cooldownTime;
			this.damageInitializer = damageInitializer;
			t = 0f;
		}

		protected override void OnModuleEnabled()
		{
			base.Slot.Cart.Train.Health.OnDamaged += HandleDamage;
			knockback.OnTargetHit += HandleTargetHit;
			statManager.AcquireStat(damageInitializer);
		}

		protected override void OnModuleDisabled()
		{
			base.Slot.Cart.Train.Health.OnDamaged -= HandleDamage;
			knockback.OnTargetHit -= HandleTargetHit;
			statManager.ReleaseStat(damageInitializer.StatDefinition);
		}

		private void HandleDamage(Damage damage)
		{
			if (!cooldown)
			{
				knockback.TriggerKnockback();
				cooldown = true;
			}
		}

		private void HandleTargetHit(Collider2D collision)
		{
			if (collision.TryGetComponent<IDamageable>(out var component))
			{
				int baseDamage = (int)statManager.GetStatValue(damageInitializer.StatDefinition);
				component.Damage(new Damage(baseDamage, 0, Damage.DamageType.Contact));
			}
		}

		private void Update()
		{
			if (cooldown)
			{
				t += Time.deltaTime;
				if (!(t < cooldownTime))
				{
					t = 0f;
					cooldown = false;
				}
			}
		}
	}
}
