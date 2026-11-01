using System;
using Monoblok.StateMachine;

namespace Monoblok.GameManagement
{
	public class NewRunConfirmedCondition : ICondition
	{
		private readonly MainMenu mainMenu;

		public event Action OnMet;

		public NewRunConfirmedCondition(MainMenu mainMenu)
		{
			this.mainMenu = mainMenu;
		}

		public void Initialize()
		{
			mainMenu.OnNewRunConfirmed += HandleNewRunConfirmed;
		}

		public void Dispose()
		{
			mainMenu.OnNewRunConfirmed -= HandleNewRunConfirmed;
		}

		private void HandleNewRunConfirmed()
		{
			this.OnMet?.Invoke();
		}
	}
}
