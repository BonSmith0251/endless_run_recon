namespace Monoblok.Enemy.AI
{
	public class WaitForAnimationState : EnemyState
	{
		public WaitForAnimationState(EnemyBrain brain, WaitForAnimationStateConfig config)
			: base(brain, config)
		{
		}

		protected override void OnEnter()
		{
			enterAnimationState.OwnedEvents.OnEnd = base.Finish;
		}

		protected override void OnExit()
		{
		}

		protected override void OnUpdate(float deltaTime)
		{
		}
	}
}
