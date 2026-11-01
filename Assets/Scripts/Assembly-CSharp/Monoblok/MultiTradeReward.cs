using Monoblok.Upgrades;
using ZLinq;

namespace Monoblok
{
	public class MultiTradeReward : CartTradeReward
	{
		private readonly MultiTradeRewardConfig config;

		private readonly CartTradeReward[] innerRewards;

		public MultiTradeReward(MultiTradeRewardConfig config, Price basePrice)
			: base(config, basePrice)
		{
			this.config = config;
			innerRewards = (from c in config.Configs.AsValueEnumerable()
				select c.GetReward(basePrice)).ToArray();
		}

		public override void Do()
		{
			CartTradeReward[] array = innerRewards;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Do();
			}
		}

		public override void UnDo()
		{
			CartTradeReward[] array = innerRewards;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].UnDo();
			}
		}
	}
}
