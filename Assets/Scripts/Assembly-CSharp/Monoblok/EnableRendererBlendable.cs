using UnityEngine;

namespace Monoblok
{
	public class EnableRendererBlendable : EndlessModeBlendable
	{
		[SerializeField]
		private Renderer _renderer;

		[SerializeField]
		private bool targetState;

		public override void Blend()
		{
			_renderer.enabled = targetState;
		}

		public override void UnBlend()
		{
			_renderer.enabled = !targetState;
		}
	}
}
