namespace Monoblok.Upgrades
{
	public class RewardOffer
	{
		public class Builder
		{
			private RewardOffer offer;

			public Builder()
			{
				offer = new RewardOffer();
			}

			public Builder WithNewRewards(LevelupReward[] newRewards)
			{
				offer.newRewards = newRewards;
				return this;
			}

			public Builder WithUpgrades(LevelupReward[] upgrades)
			{
				offer.upgrades = upgrades;
				return this;
			}

			public RewardOffer Build()
			{
				RewardOffer result = offer;
				offer = null;
				return result;
			}
		}

		private LevelupReward[] newRewards;

		private LevelupReward[] upgrades;

		public LevelupReward[] NewRewards => newRewards;

		public LevelupReward[] Upgrades => upgrades;
	}
}
