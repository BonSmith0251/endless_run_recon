using ZLinq;

namespace Monoblok.Enemy.AI
{
	public class EnterRandomState : EnemyState
	{
		private EnterRandomStateConfig config;

		private EnemyState innerState;

		private EnemyStateConfig previousConfig;

		public EnterRandomState(EnemyBrain brain, EnterRandomStateConfig config)
			: base(brain, config)
		{
			this.config = config;
		}

		protected override void OnEnter()
		{
			EnemyStateConfig enemyStateConfig = (from c in config.Pool.AsValueEnumerable()
				where c != null && c != previousConfig
				select c).Shuffle().First();
			EnemyState state = enemyStateConfig.GetState(brain);
			previousConfig = enemyStateConfig;
			innerState = state;
			innerState.OnFinished += base.Finish;
			innerState.Enter();
		}

		protected override void OnExit()
		{
			if (innerState != null)
			{
				innerState.OnFinished -= base.Finish;
				innerState.Exit();
				innerState = null;
			}
		}

		protected override void OnUpdate(float deltaTime)
		{
			innerState.Update(deltaTime);
		}
	}
}
