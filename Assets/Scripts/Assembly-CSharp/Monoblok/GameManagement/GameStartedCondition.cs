using System;
using Monoblok.StateMachine;

namespace Monoblok.GameManagement
{
	public class GameStartedCondition : ICondition
	{
		private readonly GameManager gameManager;

		public event Action OnMet;

		public GameStartedCondition(GameManager gameManager)
		{
			this.gameManager = gameManager;
		}

		public void Initialize()
		{
			gameManager.OnGameStarted += HandleGameStarted;
		}

		public void Dispose()
		{
			gameManager.OnGameStarted -= HandleGameStarted;
		}

		private void HandleGameStarted()
		{
			this.OnMet?.Invoke();
		}
	}
}
