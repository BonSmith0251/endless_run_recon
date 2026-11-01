using UnityEngine;

namespace Monoblok.Environment
{
	public class TrainTracks : EnvironmentMember
	{
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		[SerializeField]
		private float motionBlurStrength = 1f;

		private MaterialPropertyBlock mpb;

		private static readonly int SCROLLID = Shader.PropertyToID("_OffsetUvX");

		private static readonly int MOTIONBLURID = Shader.PropertyToID("_MotionBlurDist");

		private void Awake()
		{
			mpb = new MaterialPropertyBlock();
		}

		public override void UpdateEnvironmentMember(float worldDistance, float normalizedSpeed)
		{
			float num = (float)spriteRenderer.sprite.texture.width / spriteRenderer.sprite.pixelsPerUnit;
			float value = worldDistance / num;
			spriteRenderer.GetPropertyBlock(mpb);
			mpb.SetFloat(SCROLLID, value);
			mpb.SetFloat(MOTIONBLURID, normalizedSpeed * motionBlurStrength);
			spriteRenderer.SetPropertyBlock(mpb);
		}

		public void SetRenderer(SpriteRenderer renderer)
		{
			spriteRenderer = renderer;
		}
	}
}
