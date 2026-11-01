using PrimeTween;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	public class ReappearState : EnemyState
	{
		private readonly ReappearStateConfig config;

		private readonly Camera cam;

		public ReappearState(EnemyBrain brain, ReappearStateConfig config)
			: base(brain, config)
		{
			this.config = config;
			cam = Camera.main;
		}

		protected override void OnEnter()
		{
			Vector3 pos = config.Anchor.GetAnchor(cam, brain.transform);
			Sequence.Create().Chain(Tween.Scale(brain.transform, 0f, 0.15f)).ChainCallback(brain, delegate(EnemyBrain t)
			{
				t.transform.position = pos;
			})
				.Chain(Tween.Scale(brain.transform, 1f, 0.15f))
				.OnComplete(this, delegate(ReappearState t)
				{
					t.Finish();
				});
		}

		protected override void OnExit()
		{
		}

		protected override void OnUpdate(float deltaTime)
		{
		}
	}
}
