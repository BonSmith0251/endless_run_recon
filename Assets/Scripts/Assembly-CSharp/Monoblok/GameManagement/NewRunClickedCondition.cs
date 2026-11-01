using System;
using Monoblok.StateMachine;

namespace Monoblok.GameManagement
{
	public class NewRunClickedCondition : ICondition
	{
		private readonly MainMenu mainMenu;

		private readonly SaveLoadManager saveLoadManager;

		public event Action OnMet;

		public NewRunClickedCondition(MainMenu mainMenu, SaveLoadManager saveLoadManager)
		{
			this.mainMenu = mainMenu;
			this.saveLoadManager = saveLoadManager;
		}

		public void Initialize()
		{
			mainMenu.OnNewRunClicked += OnNewRunClicked;
		}

		public void Dispose()
		{
			mainMenu.OnNewRunClicked -= OnNewRunClicked;
		}

		private void OnNewRunClicked()
		{
			if (!saveLoadManager.HasRunSave)
			{
				this.OnMet?.Invoke();
			}
		}
	}
}
