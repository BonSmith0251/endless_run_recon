using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Localization;

namespace Monoblok.Train
{
	[Serializable]
	public class LastCartProvider : ITrainCartTargetProvider
	{
		private static readonly LocalizedString phrase = new LocalizedString("Module Targets", "last_car");

		public IEnumerable<TrainCart> GetCarts(TrainCart origin)
		{
			TrainCart trainCart = origin.Train.Carts.OrderBy((TrainCart c) => c.Index).Last();
			return new TrainCart[1] { trainCart };
		}

		public string GetTargetPhrase()
		{
			return phrase.GetLocalizedString();
		}
	}
}
