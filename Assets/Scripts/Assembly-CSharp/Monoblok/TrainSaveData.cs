using System;
using System.Collections.Generic;

namespace Monoblok
{
	[Serializable]
	public class TrainSaveData
	{
		public TrainCartSaveData loco;

		public List<TrainUnitSaveData> units;

		public int hp;

		public string configId;
	}
}
