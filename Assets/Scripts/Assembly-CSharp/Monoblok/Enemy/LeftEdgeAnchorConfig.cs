using System;
using UnityEngine;

namespace Monoblok.Enemy
{
	[Serializable]
	public class LeftEdgeAnchorConfig : ISpawnAnchor
	{
		[SerializeField]
		private float padding = 1f;

		[SerializeField]
		[Range(0f, 1f)]
		private float variation;

		public Vector3 GetAnchor(Camera cam, Transform origin)
		{
			float orthographicSize = cam.orthographicSize;
			float num = orthographicSize * cam.aspect;
			Vector3 position = cam.transform.position;
			float num2 = ((!((double)UnityEngine.Random.value > 0.5)) ? 1 : (-1));
			return position + new Vector3(y: UnityEngine.Random.Range(0f, orthographicSize * variation) * num2, x: 0f - num - padding, z: 0f);
		}
	}
}
