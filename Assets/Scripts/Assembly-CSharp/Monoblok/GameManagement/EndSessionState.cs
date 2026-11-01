using Monoblok.StateMachine;

namespace Monoblok.GameManagement
{
	public class EndSessionState : IState
	{
		private readonly GameManager gameManager;

		public EndSessionState(GameManager gameManager)
		{
			this.gameManager = gameManager;
		}

		public void Enter()
		{
		}

		public void Update(float deltaTime)
		{
		}

		public void Exit()
		{
			gameManager.EndSession();
		}
	}
}
