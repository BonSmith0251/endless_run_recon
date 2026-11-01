using UnityEngine;

namespace Monoblok.Environment
{
	public class MoveAxis : EnvironmentMember
	{
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		[SerializeField]
		private float speed = 0.2f;

		private MaterialPropertyBlock mpb;

		private static readonly int SCROLLIDX = Shader.PropertyToID("_OffsetUvX");

		private void Awake()
		{
			mpb = new MaterialPropertyBlock();
		}

		public override void UpdateEnvironmentMember(float worldDistance, float normalizedSpeed)
		{
			spriteRenderer.GetPropertyBlock(mpb);
			float @float = mpb.GetFloat(SCROLLIDX);
			@float += normalizedSpeed * speed;
			mpb.SetFloat(SCROLLIDX, @float);
			spriteRenderer.SetPropertyBlock(mpb);
		}
	}
}
