using System;
using System.Collections.Generic;
using UnityEngine.Localization;

namespace Monoblok.Train
{
	[Serializable]
	public class AdjacentCartProvider : ITrainCartTargetProvider
	{
		private static readonly LocalizedString phrase = new LocalizedString("Module Targets", "adjacent_cars");

		public IEnumerable<TrainCart> GetCarts(TrainCart origin)
		{
			return new TrainCart[2] { origin.Parent, origin.Child };
		}

		public string GetTargetPhrase()
		{
			return phrase.GetLocalizedString();
		}
	}
}
