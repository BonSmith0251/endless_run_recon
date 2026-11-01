using Monoblok.Input;
using Monoblok.StateMachine;
using UnityEngine.Playables;

namespace Monoblok.GameManagement
{
	public class CutsceneState : IState
	{
		private readonly CutscenePlayer player;

		private readonly PlayableDirector director;

		private readonly InputReader inputReader;

		public CutsceneState(CutscenePlayer player, PlayableDirector director, InputReader inputReader)
		{
			this.player = player;
			this.director = director;
			this.inputReader = inputReader;
		}

		public void Enter()
		{
			inputReader.EnableCutsceneControls(enable: true);
			player.PlayCutscene(director);
		}

		public void Exit()
		{
			inputReader.EnableCutsceneControls(enable: false);
		}

		public void Update(float deltaTime)
		{
		}
	}
}
