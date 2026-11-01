using System;
using UnityEngine;
using ZLinq;

namespace Monoblok
{
	[Serializable]
	public class Any : ITradeRewardEligibility
	{
		[SerializeReference]
		[SubclassSelector]
		private ITradeRewardEligibility[] inner;

		public bool IsEligible(TradeContext ctx)
		{
			return inner.AsValueEnumerable().Any((ITradeRewardEligibility e) => e.IsEligible(ctx));
		}
	}
}
