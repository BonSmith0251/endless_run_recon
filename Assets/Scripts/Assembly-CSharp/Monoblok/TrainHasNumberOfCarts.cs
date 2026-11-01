using System;
using Monoblok.SA;
using Monoblok.Train;
using UnityEngine;

namespace Monoblok
{
	[Serializable]
	public class TrainHasNumberOfCarts : IGameEventSinkCondition
	{
		[SerializeField]
		private TrainRuntimeValue trainRuntimeValue;

		[SerializeField]
		[Min(1f)]
		private int targetCount = 1;

		[SerializeField]
		private ComparisonOperator operation;

		public bool IsEligible()
		{
			if (trainRuntimeValue == null)
			{
				return false;
			}
			Monoblok.Train.Train value = trainRuntimeValue.Value;
			if (value == null)
			{
				return false;
			}
			int cartCount = value.CartCount;
			return operation.Evaluate(cartCount, targetCount);
		}
	}
}
