using UnityEngine;

namespace Monoblok.Environment
{
	public class IsoGrid : EnvironmentMember
	{
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		[SerializeField]
		private float speedMultiplier;

		private MaterialPropertyBlock mpb;

		private static readonly int OFFSET_ID = Shader.PropertyToID("_Offset");

		private void Awake()
		{
			mpb = new MaterialPropertyBlock();
		}

		public override void UpdateEnvironmentMember(float distance, float normalizedSpeed)
		{
			float @float = spriteRenderer.material.GetFloat(OFFSET_ID);
			@float += distance * speedMultiplier;
			spriteRenderer.GetPropertyBlock(mpb);
			mpb.SetFloat(OFFSET_ID, @float);
			spriteRenderer.SetPropertyBlock(mpb);
		}
	}
}
