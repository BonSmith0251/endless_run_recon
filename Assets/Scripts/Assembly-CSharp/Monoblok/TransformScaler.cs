using System;
using UnityEngine;

namespace Monoblok
{
	[Serializable]
	public class TransformScaler : IScaler
	{
		[SerializeField]
		private Transform transform;

		[SerializeField]
		private Vector2 reference = Vector2.one;

		public void Scale(Vector2 scale)
		{
			if (!(transform == null))
			{
				transform.localScale = scale * reference;
			}
		}
	}
}
