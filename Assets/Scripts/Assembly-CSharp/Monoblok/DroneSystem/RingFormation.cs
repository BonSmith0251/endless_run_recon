using System;
using UnityEngine;

namespace Monoblok.DroneSystem
{
	[Serializable]
	public class RingFormation : IDroneFormation
	{
		[SerializeField]
		private float radius;

		public Vector3[] GetPositions(Vector3 origin, int count)
		{
			Vector3[] array = new Vector3[count];
			if (count == 1)
			{
				array[0] = origin;
				return array;
			}
			for (int i = 0; i < count; i++)
			{
				float f = (float)i * (MathF.PI * 2f / (float)count);
				array[i] = origin + new Vector3(Mathf.Cos(f) * radius, Mathf.Sin(f) * radius, 0f);
			}
			return array;
		}
	}
}
