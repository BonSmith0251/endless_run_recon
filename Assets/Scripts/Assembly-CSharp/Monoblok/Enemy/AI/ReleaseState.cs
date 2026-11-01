namespace Monoblok.Enemy.AI
{
	public class ReleaseState : EnemyState
	{
		public ReleaseState(EnemyBrain brain, ReleaseStateConfig config)
			: base(brain, config)
		{
		}

		protected override void OnEnter()
		{
			brain.Enemy.ReleaseToPool();
		}

		protected override void OnUpdate(float deltaTime)
		{
		}

		protected override void OnExit()
		{
		}
	}
}
