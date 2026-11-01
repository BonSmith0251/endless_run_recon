using System;
using Monoblok.StateMachine;

namespace Monoblok.GameManagement
{
	public class EndlessModeClickedCondition : ICondition
	{
		private readonly EndGameScreen screen;

		public event Action OnMet;

		public EndlessModeClickedCondition(EndGameScreen screen)
		{
			this.screen = screen;
		}

		public void Initialize()
		{
			screen.OnEndlessClicked += HandleClicked;
		}

		public void Dispose()
		{
			screen.OnEndlessClicked -= HandleClicked;
		}

		private void HandleClicked()
		{
			this.OnMet?.Invoke();
		}
	}
}
