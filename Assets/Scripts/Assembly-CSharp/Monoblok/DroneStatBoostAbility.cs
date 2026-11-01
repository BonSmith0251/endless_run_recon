using System.Collections.Generic;
using Monoblok.DroneSystem;
using Monoblok.StatSystem;
using PrimeTween;

namespace Monoblok
{
	public class DroneStatBoostAbility : Ability
	{
		private readonly PlayerDrones playerDrones;

		private readonly StatModifierFactory modifierFactory;

		private readonly StatInitializer durationInitializer;

		private readonly Stat duration;

		private Tween executeTween;

		public DroneStatBoostAbility(DroneStatBoostAbilityConfig config)
			: base(config)
		{
			playerDrones = config.PlayerDrones.Value;
			modifierFactory = config.ModifierFactory;
			durationInitializer = config.Duration;
			duration = base.StatManager.AcquireStat(durationInitializer);
		}

		protected override void Execute()
		{
			IReadOnlyList<Drone> drones = playerDrones.Drones;
			float value = duration.Value;
			foreach (Drone item in drones)
			{
				item.WeaponHandler.StatManager.AddGlobalModifier(this, modifierFactory);
				item.TriggerStatBoostAnimation(value);
			}
			executeTween = Tween.Delay(value).OnComplete(this, delegate(DroneStatBoostAbility t)
			{
				t.RemoveModifiers();
			});
		}

		private void RemoveModifiers()
		{
			foreach (Drone drone in playerDrones.Drones)
			{
				drone.WeaponHandler.StatManager.RemoveGlobalModifier(this);
			}
		}

		protected override void ReleaseStats()
		{
			base.StatManager.ReleaseStat(durationInitializer.StatDefinition);
		}

		public override void CancelAbility()
		{
			if (!executeTween.isAlive)
			{
				return;
			}
			executeTween.Complete();
			foreach (Drone drone in playerDrones.Drones)
			{
				if (!(drone == null))
				{
					drone.StopStatBoostAnimation();
				}
			}
		}
	}
}
