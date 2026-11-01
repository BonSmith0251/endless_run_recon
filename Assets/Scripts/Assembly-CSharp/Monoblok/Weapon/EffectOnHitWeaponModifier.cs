using System;
using Monoblok.DamageSystem;
using Monoblok.EffectSystem;
using UnityEngine;

namespace Monoblok.Weapon
{
	public class EffectOnHitWeaponModifier<TPayload> : WeaponModifier
	{
		private readonly EffectDefinition<TPayload> effect;

		private Func<TPayload> resolvePayload;

		public EffectOnHitWeaponModifier(Weapon weapon, EffectDefinition<TPayload> effect)
			: base(weapon)
		{
			this.effect = effect;
		}

		internal override void RegisterStats(WeaponStatBuilder builder)
		{
			resolvePayload = effect.RegisterStats(builder);
		}

		internal override void UnregisterStats(WeaponStatBuilder builder)
		{
			effect.UnregisterStats(builder);
		}

		protected override void HandleHit(IDamageable target, Damage damage)
		{
			if (target is Component component && component.TryGetComponent<EffectHandler>(out var component2))
			{
				component2.AddEffect(effect, weapon, resolvePayload());
			}
		}
	}
}
