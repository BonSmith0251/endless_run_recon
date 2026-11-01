using System;
using System.Collections.Generic;
using UnityEngine.Localization;

namespace Monoblok.Train
{
	[Serializable]
	public class LocoCartProvider : ITrainCartTargetProvider
	{
		private static readonly LocalizedString phrase = new LocalizedString("Module Targets", "locomotive");

		public IEnumerable<TrainCart> GetCarts(TrainCart origin)
		{
			return new TrainCart[1] { origin.Train.Locomotive };
		}

		public string GetTargetPhrase()
		{
			return phrase.GetLocalizedString();
		}
	}
}
