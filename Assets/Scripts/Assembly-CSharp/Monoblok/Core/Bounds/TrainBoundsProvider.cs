using Monoblok.Train;
using UnityEngine;

namespace Monoblok.Core.Bounds
{
	public class TrainBoundsProvider : BoundsProvider
	{
		[SerializeField]
		private Monoblok.Train.Train train;

		public override UnityEngine.Bounds GetBounds()
		{
			return train.Bounds;
		}
	}
}
