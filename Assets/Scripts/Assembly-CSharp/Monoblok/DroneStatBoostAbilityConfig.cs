using Monoblok.SA;
using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok
{
	[CreateAssetMenu(menuName = "Game/Abilities/Drone Stat Boost")]
	public class DroneStatBoostAbilityConfig : AbilityConfig
	{
		[field: SerializeField]
		public PlayerDronesRuntimeValue PlayerDrones { get; private set; }

		[field: SerializeField]
		public StatModifierFactory ModifierFactory { get; private set; }

		[field: SerializeField]
		public StatInitializer Duration { get; private set; }

		public override Ability GetAbility()
		{
			return new DroneStatBoostAbility(this);
		}
	}
}
