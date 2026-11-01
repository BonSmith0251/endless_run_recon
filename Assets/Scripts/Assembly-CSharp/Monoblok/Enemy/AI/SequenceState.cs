using UnityEngine;

namespace Monoblok.Enemy.AI
{
	public class SequenceState : EnemyState
	{
		private SequenceStateConfig config;

		private EnemyState currentState;

		private int index;

		public SequenceState(EnemyBrain brain, SequenceStateConfig config)
			: base(brain, config)
		{
			this.config = config;
		}

		protected override void OnEnter()
		{
			index = 0;
			currentState = null;
			EnterNext();
		}

		protected override void OnExit()
		{
			Debug.Log("SequenceState OnExit, currentState=" + (currentState?.ToString() ?? "null"));
			ExitCurrent();
		}

		protected override void OnUpdate(float deltaTime)
		{
			if (currentState != null)
			{
				currentState.Update(deltaTime);
			}
		}

		private void EnterNext()
		{
			Debug.Log("SequenceState: EnterNext");
			ExitCurrent();
			if (index >= config.Sequence.Length)
			{
				Finish();
				return;
			}
			currentState = config.Sequence[index].GetState(brain);
			currentState.OnFinished += EnterNext;
			index++;
			currentState.Enter();
		}

		private void ExitCurrent()
		{
			if (currentState != null)
			{
				currentState.OnFinished -= EnterNext;
				currentState.Exit();
				currentState = null;
			}
		}
	}
}
