using System;
using Monoblok.StateMachine;

namespace Monoblok.GameManagement
{
	public class EndGameScreenDismissedCondition : ICondition
	{
		private readonly EndGameScreen screen;

		public event Action OnMet;

		public EndGameScreenDismissedCondition(EndGameScreen screen)
		{
			this.screen = screen;
		}

		public void Initialize()
		{
			screen.OnUpgradesClicked += HandleDismissed;
		}

		public void Dispose()
		{
			screen.OnUpgradesClicked -= HandleDismissed;
		}

		private void HandleDismissed()
		{
			this.OnMet?.Invoke();
		}
	}
}
