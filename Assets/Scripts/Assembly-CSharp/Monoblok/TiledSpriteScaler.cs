using System;
using UnityEngine;

namespace Monoblok
{
	[Serializable]
	public class TiledSpriteScaler : IScaler
	{
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		[SerializeField]
		private bool ignoreY;

		[SerializeField]
		private Vector2 mult = Vector2.one;

		public void Scale(Vector2 scale)
		{
			if (!(spriteRenderer == null))
			{
				scale *= mult;
				spriteRenderer.size = new Vector2(scale.x, ignoreY ? spriteRenderer.size.y : scale.y);
			}
		}
	}
}
