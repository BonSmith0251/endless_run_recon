using Monoblok.Input;
using Monoblok.StateMachine;
using UnityEngine;

namespace Monoblok.GameManagement
{
	public class StationState : IState
	{
		private readonly InputReader inputReader;

		private readonly StationManager stationManager;

		private readonly GameObject stationCanvasGO;

		public StationState(InputReader inputReader, StationManager stationManager, GameObject stationCanvasGO)
		{
			this.inputReader = inputReader;
			this.stationManager = stationManager;
			this.stationCanvasGO = stationCanvasGO;
		}

		public void Enter()
		{
			inputReader.EnableStationControls(enable: true);
			inputReader.EnableGlobalControls(enable: true);
			stationCanvasGO.SetActive(value: true);
			stationManager.EnterStation();
		}

		public void Update(float deltaTime)
		{
		}

		public void Exit()
		{
			inputReader.EnableStationControls(enable: false);
			stationManager.ExitStation().OnComplete(this, delegate(StationState t)
			{
				t.stationCanvasGO.SetActive(value: false);
			});
		}
	}
}
