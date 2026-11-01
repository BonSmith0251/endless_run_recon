using UnityEngine;

namespace Monoblok.Core
{
	public class ForceTiledHeight : MonoBehaviour
	{
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		private void OnValidate()
		{
			if (!(spriteRenderer == null))
			{
				float y = spriteRenderer.sprite.rect.height / spriteRenderer.sprite.pixelsPerUnit;
				Vector2 size = spriteRenderer.size;
				size.y = y;
				spriteRenderer.size = size;
			}
		}
	}
}
