using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Monoblok.Train
{
	[Serializable]
	public class ProximityCartProvider : ITrainCartTargetProvider
	{
		private static readonly LocalizedString phrase = new LocalizedString("Module Targets", "proximity");

		[SerializeField]
		[Min(0f)]
		private int range = 1;

		public IEnumerable<TrainCart> GetCarts(TrainCart origin)
		{
			List<TrainCart> list = new List<TrainCart>();
			list.Add(origin);
			TrainCart child = origin.Child;
			for (int i = 0; i < range; i++)
			{
				if (!(child != null))
				{
					break;
				}
				list.Add(child);
				child = child.Child;
			}
			TrainCart parent = origin.Parent;
			for (int j = 0; j < range; j++)
			{
				if (!(parent != null))
				{
					break;
				}
				list.Add(parent);
				parent = parent.Parent;
			}
			return list;
		}

		public string GetTargetPhrase()
		{
			return phrase.GetLocalizedString(new { range });
		}
	}
}
