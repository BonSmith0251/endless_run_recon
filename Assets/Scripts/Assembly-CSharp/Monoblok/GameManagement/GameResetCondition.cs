using System;
using Monoblok.StateMachine;

namespace Monoblok.GameManagement
{
	public class GameResetCondition : ICondition
	{
		private readonly GameManager gameManager;

		public event Action OnMet;

		public GameResetCondition(GameManager gameManager)
		{
			this.gameManager = gameManager;
		}

		public void Initialize()
		{
			gameManager.OnGameReset += HandleGameReset;
		}

		public void Dispose()
		{
			gameManager.OnGameReset -= HandleGameReset;
		}

		private void HandleGameReset()
		{
			this.OnMet?.Invoke();
		}
	}
}
