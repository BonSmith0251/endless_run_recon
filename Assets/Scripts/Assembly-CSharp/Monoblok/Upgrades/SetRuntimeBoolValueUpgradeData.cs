using Monoblok.SA;
using UnityEngine;

namespace Monoblok.Upgrades
{
	[CreateAssetMenu(menuName = "Game/Upgrade/Set Runtime Bool Value")]
	public class SetRuntimeBoolValueUpgradeData : UpgradeData
	{
		[SerializeField]
		private BoolRuntimeValue runtimeValue;

		[SerializeField]
		private bool targetState;

		public override IUpgrade GetUpgrade()
		{
			return new SetRuntimeBoolValueUpgrade(runtimeValue, targetState);
		}
	}
}
