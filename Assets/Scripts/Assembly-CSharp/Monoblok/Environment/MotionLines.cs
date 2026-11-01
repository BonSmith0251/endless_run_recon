using NaughtyAttributes;
using UnityEngine;

namespace Monoblok.Environment
{
	public class MotionLines : EnvironmentMember
	{
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		[SerializeField]
		[MinMaxSlider(0f, 1f)]
		private Vector2 minMaxStepEdge = new Vector2(0.6f, 1f);

		private MaterialPropertyBlock mpb;

		private static readonly int SCROLL_ID = Shader.PropertyToID("_Offset");

		private static readonly int STEP_EDGE_ID = Shader.PropertyToID("_StepEdge");

		private void Awake()
		{
			mpb = new MaterialPropertyBlock();
		}

		public override void UpdateEnvironmentMember(float worldDistance, float normalizedSpeed)
		{
			float width = spriteRenderer.sprite.rect.width;
			float value = worldDistance / width;
			spriteRenderer.GetPropertyBlock(mpb);
			mpb.SetFloat(SCROLL_ID, value);
			float value2 = Mathf.Lerp(minMaxStepEdge.y, minMaxStepEdge.x, normalizedSpeed);
			mpb.SetFloat(STEP_EDGE_ID, value2);
			spriteRenderer.SetPropertyBlock(mpb);
		}
	}
}
