namespace Monoblok.Upgrades
{
	public class GainAbilityReward : LevelupReward
	{
		private readonly GainAbilityRewardConfig config;

		private Ability ability;

		public GainAbilityReward(GainAbilityRewardConfig config)
			: base(config)
		{
			this.config = config;
		}

		public override void Gain()
		{
			ability = config.AbilityConfig.GetAbility();
			config.AbilityManagerValue.Value.RegisterAbility(ability);
		}

		public override void Lose()
		{
			config.AbilityManagerValue.Value.UnregisterAbility(ability);
		}
	}
}
