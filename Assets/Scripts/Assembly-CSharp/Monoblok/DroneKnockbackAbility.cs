using Monoblok.DroneSystem;
using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok
{
	public class DroneKnockbackAbility : Ability
	{
		private readonly PlayerDrones playerDrones;

		private readonly StatInitializer distanceInitializer;

		private readonly StatInitializer radiusInitializer;

		private readonly Stat distance;

		private readonly Stat radius;

		public DroneKnockbackAbility(DroneKnockbackAbilityConfig config)
			: base(config)
		{
			playerDrones = config.PlayerDrones;
			distanceInitializer = config.Distance;
			radiusInitializer = config.Radius;
			distance = base.StatManager.AcquireStat(distanceInitializer);
			radius = base.StatManager.AcquireStat(radiusInitializer);
		}

		protected override void Execute()
		{
			float value = radius.Value;
			float value2 = distance.Value;
			ShockWaveController.Instance.TriggerShockWave(playerDrones.transform.position, value, 0.3f);
			Vector3 position = playerDrones.transform.position;
			Collider2D[] array = Physics2D.OverlapCircleAll(position, value);
			foreach (Collider2D collider2D in array)
			{
				if (collider2D.TryGetComponent<Knockback>(out var component))
				{
					component.ApplyKnockback((collider2D.transform.position - position).normalized, value2);
				}
			}
		}

		protected override void ReleaseStats()
		{
			base.StatManager.ReleaseStat(distanceInitializer.StatDefinition);
			base.StatManager.ReleaseStat(radiusInitializer.StatDefinition);
		}
	}
}
