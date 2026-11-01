using System;
using Monoblok.StateMachine;

namespace Monoblok.GameManagement
{
	public class ExitGameClickedCondition : ICondition
	{
		private readonly MainMenu mainMenu;

		public event Action OnMet;

		public ExitGameClickedCondition(MainMenu mainMenu)
		{
			this.mainMenu = mainMenu;
		}

		public void Initialize()
		{
			mainMenu.OnExitClicked += HandleExitClicked;
		}

		public void Dispose()
		{
			mainMenu.OnExitClicked -= HandleExitClicked;
		}

		private void HandleExitClicked()
		{
			this.OnMet?.Invoke();
		}
	}
}
