using System;
using Monoblok.SA;
using UnityEngine;

namespace Monoblok
{
	[Serializable]
	public class AllUpgradesUnlocked : IGameEventSinkCondition
	{
		[SerializeField]
		private UpgradeGraphManagerRuntimeValue graphManagerValue;

		public bool IsEligible()
		{
			return graphManagerValue.Value.AllUnlocked();
		}
	}
}
