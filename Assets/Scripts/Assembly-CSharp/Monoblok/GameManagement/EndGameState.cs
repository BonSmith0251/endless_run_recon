using Monoblok.Input;
using Monoblok.Inventory;
using Monoblok.StateMachine;

namespace Monoblok.GameManagement
{
	public class EndGameState : IState
	{
		private readonly EndGameScreen endGameScreen;

		private readonly PlayerWallet wallet;

		private readonly InputReader inputReader;

		private EndGameSummary summary;

		public EndGameState(EndGameScreen endGameScreen, PlayerWallet wallet, InputReader inputReader)
		{
			this.endGameScreen = endGameScreen;
			this.wallet = wallet;
			this.inputReader = inputReader;
		}

		public void UpdateSummary(EndGameSummary summary)
		{
			this.summary = summary;
		}

		public void Enter()
		{
			inputReader.EnableDroneControls(enable: false);
			inputReader.EnableTrainControls(enable: false);
			bool showEndless = summary.state == EndGameSummary.State.SoftWin;
			endGameScreen.Show(showEndless);
			endGameScreen.Initialize(summary);
		}

		public void Update(float deltaTime)
		{
		}

		public void Exit()
		{
			endGameScreen.Hide();
		}
	}
}
