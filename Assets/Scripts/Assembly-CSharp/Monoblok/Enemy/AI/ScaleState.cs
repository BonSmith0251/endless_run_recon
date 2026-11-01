using PrimeTween;

namespace Monoblok.Enemy.AI
{
	public class ScaleState : EnemyState
	{
		private readonly ScaleStateConfig config;

		public ScaleState(EnemyBrain brain, ScaleStateConfig config)
			: base(brain, config)
		{
			this.config = config;
		}

		protected override void OnEnter()
		{
			Tween.Scale(brain.transform, config.TargetScale, config.Duration, Ease.Linear).OnComplete(this, delegate(ScaleState t)
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
