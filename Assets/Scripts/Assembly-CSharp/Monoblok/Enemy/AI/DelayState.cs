namespace Monoblok.Enemy.AI
{
	public class DelayState : EnemyState
	{
		private readonly DelayStateConfig config;

		private float elapsedTime;

		public DelayState(EnemyBrain brain, DelayStateConfig config)
			: base(brain, config)
		{
			this.config = config;
		}

		protected override void OnEnter()
		{
			elapsedTime = 0f;
		}

		protected override void OnExit()
		{
		}

		protected override void OnUpdate(float deltaTime)
		{
			elapsedTime += deltaTime;
			if (elapsedTime >= config.Duration)
			{
				Finish();
			}
		}
	}
}
