using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok.Weapon
{
	[CreateAssetMenu(menuName = "Game/Weapon Modifier/Knockback")]
	public class KnockbackWeaponModifierDefinition : WeaponModifierDefinition
	{
		[field: SerializeField]
		public StatInitializer KnockbackDistance { get; private set; }

		protected override WeaponModifier CreateModifierInstance(Weapon weapon)
		{
			return new KnockbackWeaponModifier(weapon, this);
		}
	}
}
