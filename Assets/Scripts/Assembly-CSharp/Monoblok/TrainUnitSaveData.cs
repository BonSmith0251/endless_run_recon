using System;
using System.Collections.Generic;

namespace Monoblok
{
	[Serializable]
	public class TrainUnitSaveData
	{
		public List<TrainCartSaveData> carts;

		public int mergeCounter;
	}
}
