using Monoblok.DamageSystem;
using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok.Weapon
{
	public class KnockbackWeaponModifier : WeaponModifier
	{
		private readonly KnockbackWeaponModifierDefinition config;

		private Stat knockbackDistance;

		public KnockbackWeaponModifier(Weapon weapon, KnockbackWeaponModifierDefinition config)
			: base(weapon)
		{
			this.config = config;
		}

		internal override void RegisterStats(WeaponStatBuilder builder)
		{
			builder.Add(config.KnockbackDistance, out knockbackDistance);
		}

		internal override void UnregisterStats(WeaponStatBuilder builder)
		{
			builder.Remove(config.KnockbackDistance);
		}

		protected override void HandleHit(IDamageable target, Damage damage)
		{
			if (target is Component component && component.TryGetComponent<IKnockback>(out var component2))
			{
				Vector3 normalized = (component.transform.position - weapon.origin.position).normalized;
				component2.ApplyKnockback(normalized, knockbackDistance.Value);
			}
		}
	}
}
