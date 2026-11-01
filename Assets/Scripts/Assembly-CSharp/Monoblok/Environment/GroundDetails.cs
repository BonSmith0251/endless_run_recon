using Monoblok.Isometric;
using UnityEngine;

namespace Monoblok.Environment
{
	public class GroundDetails : EnvironmentMember
	{
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		[SerializeField]
		private float motionBlurStrength = 1f;

		private MaterialPropertyBlock mpb;

		private static readonly int SCROLLIDX = Shader.PropertyToID("_OffsetUvX");

		private static readonly int SCROLLIDY = Shader.PropertyToID("_OffsetUvY");

		private static readonly int TILING_ID = Shader.PropertyToID("_TilingVector");

		private static readonly int MOTIONBLUR_ID = Shader.PropertyToID("_BlurAmount");

		private void Awake()
		{
			mpb = new MaterialPropertyBlock();
		}

		public override void UpdateEnvironmentMember(float worldDistance, float normalizedSpeed)
		{
			spriteRenderer.GetPropertyBlock(mpb);
			Vector2 vector = spriteRenderer.material.GetVector(TILING_ID);
			float @float = spriteRenderer.material.GetFloat(SCROLLIDX);
			float float2 = spriteRenderer.material.GetFloat(SCROLLIDY);
			Vector2 vector2 = new Vector2(@float, float2);
			vector2 += -Iso.Left * (vector * worldDistance);
			mpb.SetFloat(SCROLLIDX, vector2.x);
			mpb.SetFloat(SCROLLIDY, vector2.y);
			mpb.SetFloat(MOTIONBLUR_ID, normalizedSpeed * motionBlurStrength);
			spriteRenderer.SetPropertyBlock(mpb);
		}
	}
}
