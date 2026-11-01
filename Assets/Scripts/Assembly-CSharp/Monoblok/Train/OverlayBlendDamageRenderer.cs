using UnityEngine;

namespace Monoblok.Train
{
	public class OverlayBlendDamageRenderer : CartDamageRenderer
	{
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		private MaterialPropertyBlock mpb;

		private static readonly int ID_BLEND = Shader.PropertyToID("_OverlayBlend");

		private void Awake()
		{
			mpb = new MaterialPropertyBlock();
		}

		public override void OnHealthChanged(float normalizedHealth)
		{
			spriteRenderer.GetPropertyBlock(mpb);
			mpb.SetFloat(ID_BLEND, 1f - normalizedHealth);
			spriteRenderer.SetPropertyBlock(mpb);
		}
	}
}
