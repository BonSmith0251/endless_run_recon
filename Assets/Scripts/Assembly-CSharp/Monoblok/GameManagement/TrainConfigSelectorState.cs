using Monoblok.Input;
using Monoblok.StateMachine;
using Monoblok.Train;

namespace Monoblok.GameManagement
{
	public class TrainConfigSelectorState : IState
	{
		private readonly TrainConfigSelector selector;

		private readonly InputReader inputReader;

		private readonly CameraManager cameraManager;

		public TrainConfigSelectorState(TrainConfigSelector selector, InputReader inputReader, CameraManager cameraManager)
		{
			this.selector = selector;
			this.inputReader = inputReader;
			this.cameraManager = cameraManager;
		}

		public void Enter()
		{
			selector.InitializeSelection();
			inputReader.EnableLocoSelectorControls(enable: true);
			cameraManager.BlendToMainMenuCamera();
		}

		public void Exit()
		{
			selector.Hide();
			inputReader.EnableLocoSelectorControls(enable: false);
		}

		public void Update(float deltaTime)
		{
		}
	}
}
