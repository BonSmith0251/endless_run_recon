using PrimeTween;

namespace Monoblok.Enemy.AI
{
	public class JumpUp : EnemyState
	{
		private readonly float height;

		public JumpUp(EnemyBrain brain, JumpUpConfig config)
			: base(brain, config)
		{
			height = config.Height;
		}

		protected override void OnEnter()
		{
			brain.BoxCollider.enabled = false;
			Tween.PositionY(brain.transform, height, 0.5f, Ease.InBack).OnComplete(this, delegate(JumpUp t)
			{
				t.Finish();
			});
		}

		protected override void OnExit()
		{
			brain.BoxCollider.enabled = true;
		}

		protected override void OnUpdate(float deltaTime)
		{
		}
	}
}
