using Monoblok.Input;
using Monoblok.StateMachine;
using Monoblok.Upgrades;

namespace Monoblok.GameManagement
{
	public class UpgradeState : IState
	{
		private readonly UpgradeGraphManager upgradeGraphManager;

		private readonly InputReader inputReader;

		public UpgradeState(UpgradeGraphManager upgradeGraphManager, InputReader inputReader)
		{
			this.upgradeGraphManager = upgradeGraphManager;
			this.inputReader = inputReader;
		}

		public void Enter()
		{
			inputReader.EnableGlobalControls(enable: true);
			upgradeGraphManager.OpenGraph();
		}

		public void Update(float deltaTime)
		{
		}

		public void Exit()
		{
			inputReader.EnableGlobalControls(enable: false);
			upgradeGraphManager.CloseGraph();
		}
	}
}
