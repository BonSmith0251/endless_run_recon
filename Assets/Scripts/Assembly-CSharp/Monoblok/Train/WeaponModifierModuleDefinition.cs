using Monoblok.Weapon;
using UnityEngine;
using UnityEngine.Localization;

namespace Monoblok.Train
{
	[CreateAssetMenu(menuName = "Game/Train/Modules/Weapon Modifier")]
	public class WeaponModifierModuleDefinition : ModuleDefinition
	{
		private static readonly LocalizedString descriptionTemplate = new LocalizedString("Modules", "weapon_line");

		[SerializeField]
		private WeaponModifierModuleBehavior prefab;

		[SerializeReference]
		[SubclassSelector]
		private ITrainCartTargetProvider targetProvider;

		[field: SerializeField]
		public WeaponModifierDefinition ModifierDefinition { get; private set; }

		public override string Description
		{
			get
			{
				if (targetProvider == null || !(ModifierDefinition != null))
				{
					return base.Description;
				}
				return descriptionTemplate.GetLocalizedString(new
				{
					effect = ModifierDefinition.Name,
					target = targetProvider.GetTargetPhrase()
				});
			}
		}

		public override ModuleBehavior CreateBehavior(Transform parent)
		{
			WeaponModifierModuleBehavior weaponModifierModuleBehavior = Object.Instantiate(prefab, parent);
			weaponModifierModuleBehavior.Initialize(base.Sprite, targetProvider, ModifierDefinition);
			return weaponModifierModuleBehavior;
		}
	}
}
