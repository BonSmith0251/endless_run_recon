using System;
using Monoblok.StateMachine;

namespace Monoblok.GameManagement
{
	public class MainMenuButtonClickedCondition : ICondition
	{
		private readonly MainMenu mainMenu;

		public event Action OnMet;

		public MainMenuButtonClickedCondition(MainMenu mainMenu)
		{
			this.mainMenu = mainMenu;
		}

		public void Initialize()
		{
			mainMenu.OnMainMenuClicked += HandleMainMenuClicked;
		}

		public void Dispose()
		{
			mainMenu.OnMainMenuClicked -= HandleMainMenuClicked;
		}

		private void HandleMainMenuClicked()
		{
			this.OnMet?.Invoke();
		}
	}
}
