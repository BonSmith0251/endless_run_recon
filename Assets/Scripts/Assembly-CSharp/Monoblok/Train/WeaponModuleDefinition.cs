using Monoblok.Weapon;
using UnityEngine;

namespace Monoblok.Train
{
	[CreateAssetMenu(menuName = "Game/Train/Modules/Weapon")]
	public class WeaponModuleDefinition : ModuleDefinition
	{
		[SerializeField]
		private WeaponModuleBehavior prefab;

		[field: SerializeField]
		public WeaponConfig Config { get; private set; }

		public override ModuleBehavior CreateBehavior(Transform parent)
		{
			WeaponModuleBehavior weaponModuleBehavior = Object.Instantiate(prefab, parent);
			weaponModuleBehavior.Initialize(Config, base.Sprite);
			return weaponModuleBehavior;
		}
	}
}
