using System;
using System.Collections.Generic;
using UnityEngine.Localization;

namespace Monoblok.Train
{
	[Serializable]
	public class OriginCartProvider : ITrainCartTargetProvider
	{
		private static readonly LocalizedString phrase = new LocalizedString("Module Targets", "this_car");

		public IEnumerable<TrainCart> GetCarts(TrainCart origin)
		{
			return new TrainCart[1] { origin };
		}

		public string GetTargetPhrase()
		{
			return phrase.GetLocalizedString();
		}
	}
}
