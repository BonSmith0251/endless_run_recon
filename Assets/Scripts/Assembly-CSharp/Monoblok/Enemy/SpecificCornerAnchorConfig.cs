using System;
using Monoblok.Isometric;
using UnityEngine;

namespace Monoblok.Enemy
{
	[Serializable]
	public class SpecificCornerAnchorConfig : ISpawnAnchor
	{
		public enum Corner
		{
			BL,
			BR,
			TR,
			TL
		}

		[SerializeField]
		private Corner corner;

		[SerializeField]
		private float padding;

		[SerializeField]
		[Range(0f, 1f)]
		private float variation;

		public Vector3 GetAnchor(Camera cam, Transform origin)
		{
			float orthographicSize = cam.orthographicSize;
			float num = orthographicSize * cam.aspect;
			Vector3 position = cam.transform.position;
			float num2 = ((corner == Corner.BL || corner == Corner.TL) ? (-1f) : 1f);
			float num3 = ((corner == Corner.BL || corner == Corner.BR) ? (-1f) : 1f);
			object obj = corner switch
			{
				Corner.BL => Iso.Down, 
				Corner.BR => Iso.Right, 
				Corner.TR => Iso.Up, 
				Corner.TL => Iso.Left, 
				_ => Iso.Down, 
			};
			float num4 = ((UnityEngine.Random.value > 0.5f) ? (-1f) : 1f);
			float num5 = UnityEngine.Random.Range(0f, Mathf.Min(num, orthographicSize) * variation);
			Vector3 vector = (Vector2)obj * (num5 * num4);
			return position + new Vector3(num2 * (num + padding), num3 * (orthographicSize + padding), 0f) + vector;
		}
	}
}
