using System;
using Monoblok.StateMachine;

namespace Monoblok.GameManagement
{
	public class GameEndedCondition : ICondition
	{
		private readonly GameManager gameManager;

		public event Action OnMet;

		public GameEndedCondition(GameManager gameManager)
		{
			this.gameManager = gameManager;
		}

		public void Initialize()
		{
			gameManager.OnGameEnded += HandleGameEnded;
		}

		public void Dispose()
		{
			gameManager.OnGameEnded -= HandleGameEnded;
		}

		private void HandleGameEnded(EndGameSummary summary)
		{
			this.OnMet?.Invoke();
		}
	}
}
