using System;
using System.Collections.Generic;
using UnityEngine.Localization;

namespace Monoblok.Train
{
	[Serializable]
	public class AllCartProvider : ITrainCartTargetProvider
	{
		private static readonly LocalizedString phrase = new LocalizedString("Module Targets", "all_cars");

		public IEnumerable<TrainCart> GetCarts(TrainCart origin)
		{
			return origin.Train.Carts;
		}

		public string GetTargetPhrase()
		{
			return phrase.GetLocalizedString();
		}
	}
}
