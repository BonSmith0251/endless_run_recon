using Monoblok.DroneSystem;
using Monoblok.SA;
using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok
{
	[CreateAssetMenu(menuName = "Game/Abilities/Drone Dash")]
	public class DroneDashAbilityConfig : AbilityConfig
	{
		[SerializeField]
		private PlayerDronesRuntimeValue playerDronesValue;

		[field: SerializeField]
		public StatInitializer DashDistance { get; private set; }

		public PlayerDrones PlayerDrones => playerDronesValue.Value;

		public override Ability GetAbility()
		{
			return new DroneDashAbility(this);
		}
	}
}
