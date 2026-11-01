using System;
using System.Collections.Generic;

namespace Monoblok
{
	[Serializable]
	public class RunSaveData
	{
		public TrainSaveData trainSaveData;

		public ProgressionSaveData progressionSaveData;

		public TrainXPSaveData trainXPSaveData;

		public List<CurrencySaveData> currencySaveDatas;

		public float timeSpent;

		public int enemyDeathCount;

		public int totalCoinsCollected;

		public List<CartTradeRewardSaveData> cartTradeRewardSaveData;
	}
}
