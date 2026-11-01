using System;
using Monoblok.Input;
using Monoblok.StateMachine;

namespace Monoblok.GameManagement
{
	public class PausePerformedCondition : ICondition
	{
		private readonly InputReader inputReader;

		public event Action OnMet;

		public PausePerformedCondition(InputReader inputReader)
		{
			this.inputReader = inputReader;
		}

		public void Initialize()
		{
			inputReader.OnPausePerformed += OnPausePerformed;
		}

		private void OnPausePerformed()
		{
			this.OnMet?.Invoke();
		}

		public void Dispose()
		{
			inputReader.OnPausePerformed -= OnPausePerformed;
		}
	}
}
