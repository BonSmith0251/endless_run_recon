using UnityEngine;

namespace Monoblok.Upgrades
{
	[CreateAssetMenu(menuName = "Game/Upgrade/Mock")]
	public class MockUpgradeData : UpgradeData
	{
		public override IUpgrade GetUpgrade()
		{
			return null;
		}
	}
}
