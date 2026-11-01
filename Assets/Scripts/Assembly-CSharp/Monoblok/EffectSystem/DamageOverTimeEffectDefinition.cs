using System;
using Monoblok.StatSystem;
using Monoblok.Weapon;
using UnityEngine;

namespace Monoblok.EffectSystem
{
	[CreateAssetMenu(menuName = "Game/Effect/Damage Over Time")]
	public class DamageOverTimeEffectDefinition : EffectDefinition<DamageOverTimePayload>
	{
		[SerializeField]
		private StatInitializer damagePerTick;

		[SerializeField]
		private StatInitializer tickInterval;

		public override Func<DamageOverTimePayload> RegisterStats(WeaponStatBuilder builder)
		{
			builder.Add(damagePerTick, out var damagePerTickStat).Add(tickInterval, out var tickIntervalStat);
			return () => new DamageOverTimePayload(damagePerTickStat.Value, tickIntervalStat.Value);
		}

		public override void UnregisterStats(WeaponStatBuilder builder)
		{
			builder.Remove(damagePerTick).Remove(tickInterval);
		}

		public override EffectInstance CreateInstance(object source, DamageOverTimePayload payload)
		{
			return new DamageOverTimeEffect(this, source, payload.DamagePerTick, payload.TickInterval);
		}
	}
}
