using UnityEngine;

namespace Monoblok
{
	public class ChargeSignal : MonoBehaviour
	{
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		public void Initialize(float distance, Vector2 direction)
		{
			Vector3 localScale = spriteRenderer.transform.localScale;
			localScale.x = distance;
			spriteRenderer.transform.localScale = localScale;
			spriteRenderer.transform.right = direction.normalized;
		}
	}
}
