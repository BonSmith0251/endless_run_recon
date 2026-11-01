using System;
using UnityEngine;

namespace Monoblok
{
	[Serializable]
	public class HasAbility : ITradeRewardEligibility
	{
		[SerializeField]
		private AbilityConfig abilityConfig;

		public bool IsEligible(TradeContext ctx)
		{
			return ctx.abilityManager.HasAbility(abilityConfig);
		}
	}
}
