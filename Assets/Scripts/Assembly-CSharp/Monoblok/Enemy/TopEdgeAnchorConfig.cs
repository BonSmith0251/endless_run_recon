using System;
using UnityEngine;

namespace Monoblok.Enemy
{
	[Serializable]
	public class TopEdgeAnchorConfig : ISpawnAnchor
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
			float num3 = UnityEngine.Random.Range(0f, num * variation);
			return position + new Vector3(num3 * num2, orthographicSize + padding, 0f);
		}
	}
}
