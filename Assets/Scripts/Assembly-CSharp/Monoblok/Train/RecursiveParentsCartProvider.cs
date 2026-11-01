using System;
using System.Collections.Generic;
using UnityEngine.Localization;

namespace Monoblok.Train
{
	[Serializable]
	public class RecursiveParentsCartProvider : ITrainCartTargetProvider
	{
		private static readonly LocalizedString phrase = new LocalizedString("Module Targets", "cars_ahead");

		public IEnumerable<TrainCart> GetCarts(TrainCart origin)
		{
			List<TrainCart> list = new List<TrainCart>();
			TrainCart parent = origin.Parent;
			while (parent != null)
			{
				list.Add(parent);
				parent = parent.Parent;
			}
			return list;
		}

		public string GetTargetPhrase()
		{
			return phrase.GetLocalizedString();
		}
	}
}
