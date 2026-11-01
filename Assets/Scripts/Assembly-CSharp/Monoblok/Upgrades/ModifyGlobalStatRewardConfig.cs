using Monoblok.SA;
using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok.Upgrades
{
	[CreateAssetMenu(menuName = "Game/Level Up Reward/Modify Global Stat")]
	public class ModifyGlobalStatRewardConfig : LevelupRewardConfig
	{
		[field: SerializeField]
		public GlobalStatManagerRuntimeValue StatManagerValue { get; private set; }

		[field: SerializeField]
		public StatModifierDefinition ModifierDefinition { get; private set; }

		public override LevelupReward GetReward()
		{
			return new ModifyGlobalStatReward(this);
		}
	}
}
