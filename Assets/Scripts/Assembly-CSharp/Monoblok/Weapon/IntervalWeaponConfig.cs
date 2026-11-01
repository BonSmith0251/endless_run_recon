using FMODUnity;
using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok.Weapon
{
	public abstract class IntervalWeaponConfig : WeaponConfig, IDamageWeaponConfig
	{
		[field: SerializeField]
		public StatInitializer BaseDamage { get; private set; }

		[field: SerializeField]
		public StatInitializer CritChance { get; private set; }

		[field: SerializeField]
		public StatInitializer FireRate { get; private set; }

		[field: SerializeField]
		public EventReference SFXRef { get; private set; }

		protected sealed override WeaponBehavior CreateBehaviorInstance(Weapon weapon)
		{
			return CreateIntervalBehavior(weapon);
		}

		protected abstract IntervalWeaponBehavior CreateIntervalBehavior(Weapon weapon);
	}
}
