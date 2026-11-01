using Monoblok.Weapon;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	public class SetWeaponState : EnemyState
	{
		private readonly bool enableWeapon;

		private readonly Vector2 direction;

		public SetWeaponState(EnemyBrain brain, SetWeaponStateConfig config)
			: base(brain, config)
		{
			enableWeapon = config.EnableWeapon;
			direction = config.Direction;
		}

		protected override void OnEnter()
		{
			WeaponHandler weaponHandler = brain.WeaponHandler;
			if (weaponHandler != null)
			{
				if (enableWeapon)
				{
					Debug.Log("Enabling weapon for enemy: " + brain.name);
					weaponHandler.EnableWeapon();
				}
				else
				{
					Debug.Log("Disabling weapon for enemy: " + brain.name);
					weaponHandler.DisableWeapon();
				}
				if (weaponHandler.TargetProvider is DirectionTargetProvider directionTargetProvider)
				{
					directionTargetProvider.SetDirection(direction);
				}
			}
			Finish();
		}

		protected override void OnExit()
		{
		}

		protected override void OnUpdate(float deltaTime)
		{
		}
	}
}
