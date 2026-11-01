using System.Collections.Generic;
using UnityEngine;

namespace Monoblok.Upgrades
{
	[CreateAssetMenu(menuName = "Game/LevelUp/LevelUp Reward Factory")]
	public class LevelUpgradeFactory : ScriptableObject
	{
		[SerializeField]
		private LevelupRewardConfig[] pool;

		public LevelupReward[] GetUnique(int count)
		{
			count = Mathf.Min(count, pool.Length);
			List<LevelupRewardConfig> list = new List<LevelupRewardConfig>(pool);
			LevelupReward[] array = new LevelupReward[count];
			for (int i = 0; i < count; i++)
			{
				int index = Random.Range(0, list.Count);
				array[i] = list[index].GetReward();
				list.RemoveAt(index);
			}
			return array;
		}
	}
}
