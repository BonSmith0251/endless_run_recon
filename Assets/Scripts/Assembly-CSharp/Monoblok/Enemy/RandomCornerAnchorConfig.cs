using System;
using UnityEngine;

namespace Monoblok.Enemy
{
	[Serializable]
	public class RandomCornerAnchorConfig : ISpawnAnchor
	{
		[SerializeField]
		private float padding = 1f;

		[SerializeField]
		private float radius = 10f;

		public Vector3 GetAnchor(Camera cam, Transform origin)
		{
			float orthographicSize = cam.orthographicSize;
			float num = orthographicSize * cam.aspect;
			Vector3 position = cam.transform.position;
			float num2 = ((UnityEngine.Random.Range(0, 2) == 0) ? 1f : (-1f));
			Vector3 vector = position + new Vector3(num * num2, orthographicSize * num2, 0f);
			if (UnityEngine.Random.Range(0, 2) == 0)
			{
				return vector + new Vector3((0f - num2) * radius, num2 * padding, 0f);
			}
			return vector + new Vector3(num2 * padding, (0f - num2) * radius, 0f);
		}
	}
}
