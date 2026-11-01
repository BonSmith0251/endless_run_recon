using Monoblok.SA;
using UnityEngine;

namespace Monoblok.Upgrades
{
	[CreateAssetMenu(menuName = "Game/Level Up Reward/Gain Ability")]
	public class GainAbilityRewardConfig : LevelupRewardConfig
	{
		[field: SerializeField]
		public AbilityConfig AbilityConfig { get; private set; }

		[field: SerializeField]
		public AbilityManagerRuntimeValue AbilityManagerValue { get; private set; }

		public override LevelupReward GetReward()
		{
			return new GainAbilityReward(this);
		}
	}
}
