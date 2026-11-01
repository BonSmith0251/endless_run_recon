namespace Monoblok.Enemy.AI
{
	public class RepeatStates : EnemyState
	{
		public enum RepeatMode
		{
			Yoyo,
			Loop
		}

		private RepeatStatesConfig config;

		private EnemyState currentState;

		private int index;

		private int direction = 1;

		private int cyclesCompleted;

		public RepeatStates(EnemyBrain brain, RepeatStatesConfig config)
			: base(brain, config)
		{
			this.config = config;
		}

		protected override void OnEnter()
		{
			index = 0;
			direction = 1;
			cyclesCompleted = 0;
			EnterCurrent();
		}

		private void EnterCurrent()
		{
			currentState = config.States[index].GetState(brain);
			currentState.OnFinished += HandleStateFinished;
			currentState.Enter();
		}

		private void HandleStateFinished()
		{
			ExitCurrent();
			if (!AdvanceIndex())
			{
				Finish();
			}
			else
			{
				EnterCurrent();
			}
		}

		private bool AdvanceIndex()
		{
			int num = config.States.Length;
			if (num <= 1)
			{
				index = 0;
			}
			else
			{
				switch (config.Mode)
				{
				case RepeatMode.Loop:
					index = (index + 1) % num;
					break;
				case RepeatMode.Yoyo:
				{
					int num2 = index + direction;
					if (num2 < 0 || num2 >= num)
					{
						direction = -direction;
						num2 = index + direction;
					}
					index = num2;
					break;
				}
				}
			}
			if (index != 0)
			{
				return true;
			}
			cyclesCompleted++;
			if (config.RepeatCount >= 0)
			{
				return cyclesCompleted < config.RepeatCount;
			}
			return true;
		}

		private void ExitCurrent()
		{
			if (currentState != null)
			{
				currentState.OnFinished -= HandleStateFinished;
				currentState.Exit();
				currentState = null;
			}
		}

		protected override void OnExit()
		{
			ExitCurrent();
		}

		protected override void OnUpdate(float deltaTime)
		{
			if (currentState != null)
			{
				currentState.Update(deltaTime);
			}
		}
	}
}
