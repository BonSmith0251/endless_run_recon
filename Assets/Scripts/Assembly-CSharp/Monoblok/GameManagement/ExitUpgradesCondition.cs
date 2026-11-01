using System;
using Monoblok.StateMachine;
using Monoblok.Upgrades;

namespace Monoblok.GameManagement
{
	public class ExitUpgradesCondition : ICondition
	{
		private readonly UpgradeGraphManager graphManager;

		public event Action OnMet;

		public ExitUpgradesCondition(UpgradeGraphManager graphManager)
		{
			this.graphManager = graphManager;
		}

		public void Initialize()
		{
			graphManager.OnExitRequested += HandleExitRequested;
		}

		public void Dispose()
		{
			graphManager.OnExitRequested -= HandleExitRequested;
		}

		private void HandleExitRequested()
		{
			this.OnMet?.Invoke();
		}
	}
}
