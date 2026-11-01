using Monoblok.StatSystem;
using Monoblok.Upgrades;

namespace Monoblok
{
	public class GlobalStatModifierTradeReward : CartTradeReward
	{
		private readonly GlobalStatModifierTradeRewardConfig config;

		private Stat stat;

		public GlobalStatModifierTradeReward(GlobalStatModifierTradeRewardConfig config, Price basePrice)
			: base(config, basePrice)
		{
			this.config = config;
		}

		public override void Do()
		{
			config.GlobalStatModifierValue.Value.StatManager.TryGetStat(config.StatDefinition, out stat);
			if (stat != null)
			{
				stat.AddModifier(config.ModifierFactory.GetModifier(this));
			}
		}

		public override void UnDo()
		{
			stat.RemoveModifiersFromSource(this);
		}
	}
}
