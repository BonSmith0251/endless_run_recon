namespace Monoblok.Upgrades
{
	public abstract class LevelupReward
	{
		private readonly LevelupRewardConfig config;

		private int upgradeCount;

		public string RewardName => config.RewardName;

		public string Description => config.Description;

		public LevelupReward(LevelupRewardConfig config)
		{
			this.config = config;
		}

		public abstract void Gain();

		public abstract void Lose();

		public bool CanUpgrade()
		{
			return upgradeCount < config.MaxUpgradeCount;
		}

		public virtual void OnUpgrade()
		{
		}
	}
}
