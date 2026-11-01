using Monoblok.Upgrades;
using NaughtyAttributes;
using UnityEngine;
using ZLinq;

namespace Monoblok
{
	[CreateAssetMenu(menuName = "Game/Cart Trade/Config/Multi")]
	public class MultiTradeRewardConfig : CartTradeRewardConfig
	{
		[field: SerializeField]
		[field: Expandable]
		public CartTradeRewardConfig[] Configs { get; private set; }

		public override string Description => (from c in Configs.AsValueEnumerable()
			select c.Description).JoinToString("\n");

		public override CartTradeReward GetReward(Price basePrice)
		{
			return new MultiTradeReward(this, basePrice);
		}

		protected override bool CheckAdditionalEligibilities(TradeContext ctx)
		{
			return Configs.AsValueEnumerable().All((CartTradeRewardConfig c) => c.IsEligible(ctx));
		}
	}
}
