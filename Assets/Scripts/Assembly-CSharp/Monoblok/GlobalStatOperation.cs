using System;
using Monoblok.SA;
using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok
{
	[Serializable]
	public class GlobalStatOperation : ITradeRewardEligibility
	{
		private enum Operation
		{
			LessThan,
			LessThanOrEqual,
			Equal,
			MoreThan,
			MoreThanOrEqual
		}

		[SerializeField]
		private GlobalStatManagerRuntimeValue globalStatManagerValue;

		[SerializeField]
		private StatDefinition statDefinition;

		[SerializeField]
		private Operation operation;

		[SerializeField]
		private float target;

		public bool IsEligible(TradeContext ctx)
		{
			if (!globalStatManagerValue.Value.StatManager.TryGetStat(statDefinition, out var stat))
			{
				return false;
			}
			return operation switch
			{
				Operation.LessThan => stat.Value < target, 
				Operation.LessThanOrEqual => stat.Value <= target, 
				Operation.Equal => stat.Value == target, 
				Operation.MoreThan => stat.Value > target, 
				Operation.MoreThanOrEqual => stat.Value >= target, 
				_ => throw new NotImplementedException("Operation is unknown"), 
			};
		}
	}
}
