using Monoblok.DroneSystem;
using Monoblok.SA;
using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok
{
	[CreateAssetMenu(menuName = "Game/Abilities/Drone Knockback")]
	public class DroneKnockbackAbilityConfig : AbilityConfig
	{
		[SerializeField]
		private PlayerDronesRuntimeValue playerDronesValue;

		[field: SerializeField]
		public StatInitializer Radius { get; private set; }

		[field: SerializeField]
		public StatInitializer Distance { get; private set; }

		public PlayerDrones PlayerDrones => playerDronesValue.Value;

		public override Ability GetAbility()
		{
			return new DroneKnockbackAbility(this);
		}
	}
}
