using FMODUnity;
using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok.Weapon
{
	[CreateAssetMenu(menuName = "Game/Weapon Config/Projectile")]
	public class ProjectileWeaponConfig : IntervalWeaponConfig
	{
		[field: Space]
		[field: Header("Projectile Config")]
		[field: SerializeField]
		public Projectile ProjectilePrefab { get; private set; }

		[field: SerializeField]
		public float SpeedConstant { get; private set; }

		[field: SerializeField]
		public EventReference ImpactReference { get; private set; }

		[field: SerializeField]
		public StatInitializer PierceCount { get; private set; }

		protected override IntervalWeaponBehavior CreateIntervalBehavior(Weapon weapon)
		{
			return new ProjectileBehavior(weapon, this);
		}
	}
}
