using Monoblok.EffectSystem;
using UnityEngine;

namespace Monoblok.Weapon
{
	[CreateAssetMenu(menuName = "Game/Weapon Modifier/Damage Over Time")]
	public class DamageOverTimeWeaponModifierDefinition : WeaponModifierDefinition
	{
		[field: SerializeField]
		public DamageOverTimeEffectDefinition Effect { get; private set; }

		protected override WeaponModifier CreateModifierInstance(Weapon weapon)
		{
			return new EffectOnHitWeaponModifier<DamageOverTimePayload>(weapon, Effect);
		}
	}
}
