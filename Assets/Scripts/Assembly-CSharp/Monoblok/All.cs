using System;
using UnityEngine;
using ZLinq;

namespace Monoblok
{
	[Serializable]
	public class All : ITradeRewardEligibility
	{
		[SerializeReference]
		[SubclassSelector]
		private ITradeRewardEligibility[] inner;

		public bool IsEligible(TradeContext ctx)
		{
			return inner.AsValueEnumerable().All((ITradeRewardEligibility e) => e.IsEligible(ctx));
		}
	}
}
