using UnityEngine;

namespace Monoblok.Core
{
	public class SpriteHighlighter : MonoBehaviour
	{
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		private MaterialPropertyBlock mpb;

		private static readonly int AlphaID = Shader.PropertyToID("_OutlineAlpha");

		private static readonly int ColorID = Shader.PropertyToID("_OutlineColor");

		private void Awake()
		{
			mpb = new MaterialPropertyBlock();
		}

		public void StartHighlight(Color color)
		{
			spriteRenderer.GetPropertyBlock(mpb);
			mpb.SetFloat(AlphaID, 1f);
			mpb.SetColor(ColorID, color);
			spriteRenderer.SetPropertyBlock(mpb);
		}

		public void EndHighlight()
		{
			spriteRenderer.GetPropertyBlock(mpb);
			mpb.SetFloat(AlphaID, 0f);
			spriteRenderer.SetPropertyBlock(mpb);
		}
	}
}
