using System;
using Monoblok.Progression;
using UnityEngine;

namespace Monoblok
{
	[Serializable]
	public class WaveIndexOperation : IGameEventSinkCondition
	{
		[SerializeField]
		private Monoblok.Progression.Progression progression;

		[SerializeField]
		private int targetIdex;

		[SerializeField]
		private ComparisonOperator operation;

		public bool IsEligible()
		{
			if (progression == null)
			{
				return false;
			}
			int currentWaveIndex = progression.CurrentWaveIndex;
			return operation.Evaluate(currentWaveIndex, targetIdex);
		}
	}
}
