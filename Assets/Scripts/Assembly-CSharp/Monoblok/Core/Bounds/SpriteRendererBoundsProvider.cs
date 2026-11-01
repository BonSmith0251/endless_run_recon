using UnityEngine;

namespace Monoblok.Core.Bounds
{
	public class SpriteRendererBoundsProvider : BoundsProvider
	{
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		public override UnityEngine.Bounds GetBounds()
		{
			if (spriteRenderer == null)
			{
				return default(UnityEngine.Bounds);
			}
			return spriteRenderer.bounds;
		}
	}
}
