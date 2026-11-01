using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok.Weapon
{
	[CreateAssetMenu(menuName = "Game/Weapon Config/Melee")]
	public class MeleeWeaponConfig : WeaponConfig, IDamageWeaponConfig
	{
		[field: SerializeField]
		public StatInitializer BaseDamage { get; private set; }

		[field: SerializeField]
		public StatInitializer CritChance { get; private set; }

		[field: SerializeField]
		public MeleeWeapon WeaponPrefab { get; private set; }

		[field: SerializeField]
		public StatInitializer Speed { get; private set; }

		[field: SerializeField]
		public StatInitializer Count { get; private set; }

		[field: SerializeField]
		public float StaggerDuration { get; private set; } = 0.2f;


		[field: Range(0f, 1f)]
		[field: SerializeField]
		public float StaggerSpeedMultiplier { get; private set; } = 0.3f;


		[field: SerializeField]
		public float StaggerRecoverDuration { get; private set; } = 0.15f;


		protected override WeaponBehavior CreateBehaviorInstance(Weapon weapon)
		{
			return new MeleeWeaponBehavior(weapon, this);
		}
	}
}
