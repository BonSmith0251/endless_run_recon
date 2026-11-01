using System;
using Monoblok.SA;
using Monoblok.Train;
using UnityEngine;

namespace Monoblok
{
	[Serializable]
	public class MatchTrainConfig : IGameEventSinkCondition
	{
		[SerializeField]
		private TrainRuntimeValue trainRuntimeValue;

		[SerializeField]
		private TrainConfig targetConfig;

		public bool IsEligible()
		{
			if (trainRuntimeValue == null || targetConfig == null)
			{
				return false;
			}
			Monoblok.Train.Train value = trainRuntimeValue.Value;
			if (value == null)
			{
				return false;
			}
			return value.Config == targetConfig;
		}
	}
}
