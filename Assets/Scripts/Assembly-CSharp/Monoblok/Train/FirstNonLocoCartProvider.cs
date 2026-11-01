using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Localization;

namespace Monoblok.Train
{
	[Serializable]
	public class FirstNonLocoCartProvider : ITrainCartTargetProvider
	{
		private static readonly LocalizedString phrase = new LocalizedString("Module Targets", "first_car");

		public IEnumerable<TrainCart> GetCarts(TrainCart origin)
		{
			TrainCart loco = origin.Train.Locomotive;
			TrainCart trainCart = origin.Train.Carts.Where((TrainCart c) => c != loco).First();
			return new TrainCart[1] { trainCart };
		}

		public string GetTargetPhrase()
		{
			return phrase.GetLocalizedString();
		}
	}
}
