using UnityEngine;
using UnityEngine.Localization;

namespace Monoblok.Weapon
{
	public abstract class WeaponModifierDefinition : ScriptableObject
	{
		[SerializeField]
		private LocalizedString localizedName;

		public string Name => localizedName.GetLocalizedString();

		public WeaponModifier CreateModifier(Weapon weapon)
		{
			WeaponModifier weaponModifier = CreateModifierInstance(weapon);
			WeaponStatBuilder builder = new WeaponStatBuilder(weapon.statManager);
			weaponModifier.RegisterStats(builder);
			return weaponModifier;
		}

		protected abstract WeaponModifier CreateModifierInstance(Weapon weapon);
	}
}
