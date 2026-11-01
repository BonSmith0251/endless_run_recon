using System;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[Serializable]
	public class RadiusProvider : IRadiusProvider
	{
		[SerializeField]
		private float radius = 1f;

		public float GetRadius()
		{
			return radius;
		}
	}
}
