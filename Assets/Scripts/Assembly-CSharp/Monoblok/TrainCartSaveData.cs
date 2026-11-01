using System;
using System.Collections.Generic;

namespace Monoblok
{
	[Serializable]
	public class TrainCartSaveData
	{
		public string id;

		public List<ModuleSaveData> modules;
	}
}
