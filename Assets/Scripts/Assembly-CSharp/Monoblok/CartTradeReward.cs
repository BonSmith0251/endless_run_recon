using Monoblok.Upgrades;
using UnityEngine;

namespace Monoblok
{
	public abstract class CartTradeReward
	{
		private readonly CartTradeRewardConfig config;

		public readonly RuntimePrice Price;

		public CartTradeRewardConfig Config => config;

		public string Description => config.Description;

		public CartTradeReward(CartTradeRewardConfig config, Price basePrice)
		{
			this.config = config;
			Price = new RuntimePrice(basePrice);
			RefreshPrice(basePrice);
		}

		public void RefreshPrice(Price basePrice)
		{
			Price.Amount.SetValue(Mathf.RoundToInt((float)basePrice.Amount * config.PriceMultiplier));
		}

		public abstract void Do();

		public abstract void UnDo();
	}
}
