using System;
using Monoblok.StateMachine;

namespace Monoblok.GameManagement
{
	public class CutsceneStoppedCondition : ICondition
	{
		private readonly CutscenePlayer player;

		public event Action OnMet;

		public CutsceneStoppedCondition(CutscenePlayer player)
		{
			this.player = player;
		}

		public void Initialize()
		{
			player.OnCutsceneStopped += HandleCutsceneStopped;
		}

		private void HandleCutsceneStopped()
		{
			this.OnMet?.Invoke();
		}

		public void Dispose()
		{
			player.OnCutsceneStopped -= HandleCutsceneStopped;
		}
	}
}
