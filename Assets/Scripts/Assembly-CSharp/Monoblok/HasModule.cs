using System;
using Monoblok.Train;
using UnityEngine;
using ZLinq;

namespace Monoblok
{
	[Serializable]
	public class HasModule : ITradeRewardEligibility
	{
		[SerializeField]
		private ModuleDefinition moduleDefinition;

		public bool IsEligible(TradeContext ctx)
		{
			return (from m in ctx.train.Carts.AsValueEnumerable().SelectMany((TrainCart c) => c.ModuleManager.Modules)
				where m.Definition == moduleDefinition
				select m).Any();
		}
	}
}
