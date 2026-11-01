using Monoblok.Upgrades;
using UnityEngine;
using ZLinq;

namespace Monoblok
{
	public abstract class CartTradeRewardConfig : IDScriptableObject
	{
		[SerializeReference]
		[SubclassSelector]
		private ITradeRewardEligibility[] eligibilities;

		[SerializeField]
		public float PriceMultiplier;

		public abstract string Description { get; }

		public abstract CartTradeReward GetReward(Price basePrice);

		public bool IsEligible(TradeContext ctx)
		{
			if (eligibilities.AsValueEnumerable().All((ITradeRewardEligibility e) => e.IsEligible(ctx)))
			{
				return CheckAdditionalEligibilities(ctx);
			}
			return false;
		}

		protected virtual bool CheckAdditionalEligibilities(TradeContext ctx)
		{
			return true;
		}
	}
}
