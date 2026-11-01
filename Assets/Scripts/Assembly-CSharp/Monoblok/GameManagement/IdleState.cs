using Monoblok.StateMachine;
using UnityEngine;

namespace Monoblok.GameManagement
{
	public class IdleState : IState
	{
		private readonly IdleStateContext ctx;

		public IdleState(IdleStateContext ctx)
		{
			this.ctx = ctx;
		}

		public void Enter()
		{
			Debug.Log("Enter idle state");
			ctx.stationManager.ExitStation();
			ctx.abilityManager.EnableAbilities(enable: false);
		}

		public void Update(float deltaTime)
		{
		}

		public void Exit()
		{
		}
	}
}
