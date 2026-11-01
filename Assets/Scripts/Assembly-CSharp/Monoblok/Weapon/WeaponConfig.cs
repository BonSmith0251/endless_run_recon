using System.Collections.Generic;
using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok.Weapon
{
	public abstract class WeaponConfig : ScriptableObject
	{
		[field: Header("Base Config")]
		[field: SerializeField]
		public StatInitializer Range { get; private set; }

		[field: SerializeField]
		public List<WeaponModifierDefinition> Modifiers { get; private set; } = new List<WeaponModifierDefinition>();


		public IWeaponBehavior CreateBehavior(Weapon weapon)
		{
			WeaponBehavior weaponBehavior = CreateBehaviorInstance(weapon);
			WeaponStatBuilder builder = new WeaponStatBuilder(weapon.statManager);
			weaponBehavior.RegisterStats(builder);
			return weaponBehavior;
		}

		protected abstract WeaponBehavior CreateBehaviorInstance(Weapon weapon);
	}
}
