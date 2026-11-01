using Monoblok.StateMachine;

namespace Monoblok.GameManagement
{
	public class MainMenuState : IState
	{
		private readonly MainMenu mainMenu;

		private readonly SaveLoadManager saveLoadManager;

		private readonly CameraManager cameraManager;

		public MainMenuState(MainMenu mainMenu, SaveLoadManager saveLoadManager, CameraManager cameraManager)
		{
			this.mainMenu = mainMenu;
			this.saveLoadManager = saveLoadManager;
			this.cameraManager = cameraManager;
		}

		public void Enter()
		{
			MainMenu.MainMenuState state = ((!saveLoadManager.HasRunSave) ? MainMenu.MainMenuState.Initial_NoSavePresent : MainMenu.MainMenuState.Initial_SavePresent);
			mainMenu.SetState(state);
			mainMenu.Show();
			cameraManager.BlendToMainMenuCamera();
		}

		public void Exit()
		{
			mainMenu.Hide();
		}

		public void Update(float deltaTime)
		{
		}
	}
}
