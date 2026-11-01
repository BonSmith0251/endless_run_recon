using System;
using System.Collections.Generic;
using UnityEngine.Localization;

namespace Monoblok.Train
{
	[Serializable]
	public class RecursiveChildrenCartProvider : ITrainCartTargetProvider
	{
		private static readonly LocalizedString phrase = new LocalizedString("Module Targets", "cars_behind");

		public IEnumerable<TrainCart> GetCarts(TrainCart origin)
		{
			List<TrainCart> list = new List<TrainCart>();
			TrainCart child = origin.Child;
			while (child != null)
			{
				list.Add(child);
				child = child.Child;
			}
			return list;
		}

		public string GetTargetPhrase()
		{
			return phrase.GetLocalizedString();
		}
	}
}
