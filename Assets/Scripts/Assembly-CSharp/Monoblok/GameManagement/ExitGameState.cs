using Monoblok.StateMachine;
using UnityEngine;

namespace Monoblok.GameManagement
{
	public class ExitGameState : IState
	{
		private readonly SaveLoadManager saveLoadManager;

		public ExitGameState(SaveLoadManager saveLoadManager)
		{
			this.saveLoadManager = saveLoadManager;
		}

		public void Enter()
		{
			saveLoadManager.SaveMeta();
			Application.Quit();
		}

		public void Exit()
		{
		}

		public void Update(float deltaTime)
		{
		}
	}
}
