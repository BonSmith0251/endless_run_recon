using Monoblok.Movement;
using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok.Weapon
{
	public class MultiProjectileBehavior : ProjectileBehavior
	{
		private readonly MultiProjectileWeaponConfig multiConfig;

		private Stat projectileCount;

		private Stat spread;

		public MultiProjectileBehavior(Weapon weapon, MultiProjectileWeaponConfig config)
			: base(weapon, config)
		{
			multiConfig = config;
		}

		internal override void RegisterStats(WeaponStatBuilder builder)
		{
			base.RegisterStats(builder);
			builder.Add(multiConfig.ProjectileCount, out projectileCount).Add(multiConfig.Spread, out spread);
		}

		protected override void OnTick(ITarget target)
		{
			Vector2 normalized = ((Vector2)target.Position - (Vector2)weapon.origin.position).normalized;
			int num = Mathf.Max(1, (int)projectileCount.Value);
			float num2 = ((num > 1) ? (spread.Value / (float)(num - 1)) : 0f);
			float num3 = ((num > 1) ? ((0f - spread.Value) * 0.5f) : 0f);
			for (int i = 0; i < num; i++)
			{
				float z = num3 + num2 * (float)i;
				Vector2 direction = Quaternion.Euler(0f, 0f, z) * normalized;
				SpawnProjectile(BuildData(direction));
			}
		}
	}
}
