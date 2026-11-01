using UnityEngine;

namespace Monoblok.Upgrades
{
	public abstract class LevelupRewardConfig : ScriptableObject
	{
		[field: SerializeField]
		public string RewardName { get; private set; }

		[field: SerializeField]
		public string Description { get; private set; }

		[field: SerializeField]
		public int MaxUpgradeCount { get; private set; }

		public abstract LevelupReward GetReward();
	}
}
