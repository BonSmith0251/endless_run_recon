using Monoblok.Input;
using Monoblok.Progression;
using Monoblok.StateMachine;
using UnityEngine;

namespace Monoblok.GameManagement
{
	public class JourneyState : IState
	{
		private readonly InputReader inputReader;

		private readonly ProgressionManager progressionManager;

		private readonly AbilityManager abilityManager;

		private readonly SaveLoadManager saveLoadManager;

		private readonly GameObject waveCanvasGO;

		public JourneyState(InputReader inputReader, ProgressionManager progressionManager, AbilityManager abilityManager, SaveLoadManager saveLoadManager, GameObject wavecanvasGO)
		{
			this.inputReader = inputReader;
			this.progressionManager = progressionManager;
			this.abilityManager = abilityManager;
			this.saveLoadManager = saveLoadManager;
			waveCanvasGO = wavecanvasGO;
		}

		public void Enter()
		{
			saveLoadManager.SaveRun();
			inputReader.EnableDroneControls(enable: true);
			inputReader.EnableTrainControls(enable: true);
			abilityManager.EnableAbilities(enable: true);
			progressionManager.RunNextWave();
			waveCanvasGO.SetActive(value: true);
		}

		public void Update(float deltaTime)
		{
		}

		public void Exit()
		{
			inputReader.EnableDroneControls(enable: false);
			inputReader.EnableTrainControls(enable: false);
			abilityManager.EnableAbilities(enable: false);
			waveCanvasGO.SetActive(value: false);
		}
	}
}
