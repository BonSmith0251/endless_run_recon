using System.Linq;
using UnityEngine;
using ZLinq;

namespace Monoblok
{
	[CreateAssetMenu(menuName = "Game/Cart Trade Offer Factory")]
	public class CartTradeOfferFactory : ScriptableObject
	{
		[SerializeField]
		private int rewardCount = 2;

		[SerializeField]
		private CartTradeRewardConfig[] rewardPool;

		public CartTradeReward[] GetRewards(TradeContext ctx)
		{
			return (from c in (from c in rewardPool.AsValueEnumerable()
					where c.IsEligible(ctx)
					select c).Shuffle().Take(rewardCount)
				select c.GetReward(ctx.basePrice)).ToArray();
		}

		public CartTradeRewardConfig GetRewardConfigById(string id)
		{
			CartTradeRewardConfig cartTradeRewardConfig = rewardPool.FirstOrDefault((CartTradeRewardConfig c) => c.ID == id);
			if (cartTradeRewardConfig == null)
			{
				Debug.LogError("No cart trade reward config found in pool with id " + id);
			}
			return cartTradeRewardConfig;
		}
	}
}
