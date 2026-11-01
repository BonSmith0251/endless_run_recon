using Monoblok.EffectSystem;
using UnityEngine;

namespace Monoblok.Weapon
{
	[CreateAssetMenu(menuName = "Game/Weapon Modifier/Stat Modifier")]
	public class StatModifierWeaponModifierDefinition : WeaponModifierDefinition
	{
		[field: SerializeField]
		public StatModifierEffectDefinition Effect { get; private set; }

		protected override WeaponModifier CreateModifierInstance(Weapon weapon)
		{
			return new EffectOnHitWeaponModifier<float>(weapon, Effect);
		}
	}
}
