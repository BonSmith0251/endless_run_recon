using UnityEngine;

namespace Monoblok.Upgrades
{
	[CreateAssetMenu(menuName = "Game/LevelUp/Branch")]
	public class LevelUpgradeBranch : ScriptableObject
	{
		[SerializeField]
		private LevelupRewardConfig[] sequence;

		public bool TryGetNext(int currentIndex, out LevelupRewardConfig config)
		{
			config = null;
			int num = currentIndex + 1;
			if (num < 0)
			{
				return false;
			}
			if (sequence.Length <= num)
			{
				return false;
			}
			return sequence[num];
		}
	}
}
