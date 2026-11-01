using UnityEngine;

namespace Monoblok.Environment
{
	public class TrainTracksBlendable : EndlessModeBlendable
	{
		[SerializeField]
		private TrainTracks tracks;

		[SerializeField]
		private SpriteRenderer defaultRenderer;

		[SerializeField]
		private SpriteRenderer endlessRenderer;

		public override void Blend()
		{
			tracks.SetRenderer(endlessRenderer);
		}

		public override void UnBlend()
		{
			tracks.SetRenderer(defaultRenderer);
		}
	}
}
