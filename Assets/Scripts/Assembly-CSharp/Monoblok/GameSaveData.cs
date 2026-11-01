using System;
using System.Collections.Generic;
using Monoblok.Upgrades;

namespace Monoblok
{
	[Serializable]
	public class GameSaveData
	{
		private const int InvalidVersion = -1;

		public int version;

		public bool hasRunSaveData;

		public RunSaveData runSaveData;

		public UpgradeGraphSaveData upgradeGraphSaveData;

		public List<CurrencySaveData> metaCurrencySaveData;

		public GameSaveData()
		{
			version = -1;
		}
	}
}
