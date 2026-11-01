using Monoblok.Weapon;
using UnityEngine;

namespace Monoblok.Train
{
	public class WeaponModuleBehavior : ModuleBehavior
	{
		[SerializeField]
		private WeaponHandler weaponHandler;

		private WeaponConfig weaponConfig;

		public void Initialize(WeaponConfig config, Sprite sprite)
		{
			weaponConfig = config;
			spriteRenderer.sprite = sprite;
		}

		protected override void OnAttached()
		{
			weaponHandler.Initialize(weaponConfig);
			base.Slot.Cart.RegisterWeaponHandler(weaponHandler);
		}

		protected override void OnDetached()
		{
			base.Slot.Cart.UnregisterWeaponHandler(weaponHandler);
		}

		protected override void OnModuleEnabled()
		{
			weaponHandler.SetCanShoot(canShoot: true);
			weaponHandler.EnableWeapon();
		}

		protected override void OnModuleDisabled()
		{
			weaponHandler.SetCanShoot(canShoot: false);
			weaponHandler.DisableWeapon();
		}
	}
}
