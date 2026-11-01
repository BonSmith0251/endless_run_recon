namespace Monoblok.Enemy.AI
{
	public class IdleState : EnemyState
	{
		private readonly IdleStateConfig config;

		private float t;

		public IdleState(EnemyBrain brain, IdleStateConfig config)
			: base(brain, config)
		{
			this.config = config;
		}

		protected override void OnEnter()
		{
			t = 0f;
		}

		protected override void OnExit()
		{
		}

		protected override void OnUpdate(float deltaTime)
		{
			t += deltaTime;
			if (!(t < config.IdleTime))
			{
				Finish();
			}
		}
	}
}
