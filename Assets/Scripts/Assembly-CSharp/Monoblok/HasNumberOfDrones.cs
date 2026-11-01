using System;
using Monoblok.DroneSystem;
using Monoblok.SA;
using UnityEngine;

namespace Monoblok
{
	[Serializable]
	public class HasNumberOfDrones : IGameEventSinkCondition
	{
		[SerializeField]
		private PlayerDronesRuntimeValue dronesRuntimeValue;

		[SerializeField]
		[Min(1f)]
		private int targetCount = 1;

		[SerializeField]
		private ComparisonOperator operation;

		public bool IsEligible()
		{
			if (dronesRuntimeValue == null)
			{
				return false;
			}
			PlayerDrones value = dronesRuntimeValue.Value;
			if (value == null)
			{
				return false;
			}
			int count = value.Drones.Count;
			return operation.Evaluate(count, targetCount);
		}
	}
}
