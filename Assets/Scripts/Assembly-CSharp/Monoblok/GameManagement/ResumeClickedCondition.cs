using System;
using Monoblok.StateMachine;

namespace Monoblok.GameManagement
{
	public class ResumeClickedCondition : ICondition
	{
		private readonly MainMenu mainMenu;

		public event Action OnMet;

		public ResumeClickedCondition(MainMenu mainMenu)
		{
			this.mainMenu = mainMenu;
		}

		public void Initialize()
		{
			mainMenu.OnResumeRunClicked += OnResumeRunClicked;
		}

		private void OnResumeRunClicked()
		{
			this.OnMet?.Invoke();
		}

		public void Dispose()
		{
			mainMenu.OnResumeRunClicked -= OnResumeRunClicked;
		}
	}
}
