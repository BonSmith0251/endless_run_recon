using Monoblok.Upgrades;

namespace Monoblok
{
	public class IncreaseRuntimeValueReward : CartTradeReward
	{
		private readonly IncreaseRuntimeValueRewardConfig config;

		public IncreaseRuntimeValueReward(IncreaseRuntimeValueRewardConfig config, Price basePrice)
			: base(config, basePrice)
		{
			this.config = config;
		}

		public override void Do()
		{
			float value = config.RuntimeValue.Value + config.Step;
			config.RuntimeValue.SetValue(value);
		}

		public override void UnDo()
		{
			float value = config.RuntimeValue.Value - config.Step;
			config.RuntimeValue.SetValue(value);
		}
	}
}
