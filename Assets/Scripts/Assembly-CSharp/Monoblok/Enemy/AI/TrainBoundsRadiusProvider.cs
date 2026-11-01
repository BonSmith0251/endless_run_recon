using System;
using Monoblok.SA;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[Serializable]
	public class TrainBoundsRadiusProvider : IRadiusProvider
	{
		[SerializeField]
		private TrainRuntimeValue trainRuntimeValue;

		[SerializeField]
		private float padding;

		public float GetRadius()
		{
			return trainRuntimeValue.Value.Bounds.extents.x + padding;
		}
	}
}
