using System;
using Monoblok.Input;
using Monoblok.StateMachine;

namespace Monoblok.GameManagement
{
	public class ContinuePerformedCondition : ICondition
	{
		private readonly MainMenu mainMenu;

		private readonly InputReader inputReader;

		public event Action OnMet;

		public ContinuePerformedCondition(MainMenu mainMenu, InputReader inputReader)
		{
			this.mainMenu = mainMenu;
			this.inputReader = inputReader;
		}

		public void Initialize()
		{
			inputReader.OnPausePerformed += OnContinuePerformed;
			mainMenu.OnResumeRunClicked += OnContinuePerformed;
		}

		private void OnContinuePerformed()
		{
			this.OnMet?.Invoke();
		}

		public void Dispose()
		{
			inputReader.OnPausePerformed -= OnContinuePerformed;
			mainMenu.OnResumeRunClicked -= OnContinuePerformed;
		}
	}
}
