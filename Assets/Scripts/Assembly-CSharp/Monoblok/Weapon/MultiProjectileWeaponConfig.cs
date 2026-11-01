using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok.Weapon
{
	[CreateAssetMenu(menuName = "Game/Weapon Config/Multi Projectile")]
	public class MultiProjectileWeaponConfig : ProjectileWeaponConfig
	{
		[field: SerializeField]
		public StatInitializer ProjectileCount { get; private set; }

		[field: SerializeField]
		public StatInitializer Spread { get; private set; }

		protected override IntervalWeaponBehavior CreateIntervalBehavior(Weapon weapon)
		{
			return new MultiProjectileBehavior(weapon, this);
		}
	}
}
