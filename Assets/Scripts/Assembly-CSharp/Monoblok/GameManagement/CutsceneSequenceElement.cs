using System;
using Monoblok.Core;
using UnityEngine.Playables;

namespace Monoblok.GameManagement
{
	public class CutsceneSequenceElement : ISequenceElement
	{
		private readonly CutscenePlayer player;

		private readonly PlayableDirector cutscene;

		public event Action OnComplete;

		public CutsceneSequenceElement(CutscenePlayer player, PlayableDirector cutscene)
		{
			this.player = player;
			this.cutscene = cutscene;
		}

		public void Enter()
		{
			player.OnCutsceneStopped += HandleStopped;
			player.PlayCutscene(cutscene);
		}

		private void HandleStopped()
		{
			this.OnComplete?.Invoke();
		}

		public void Exit()
		{
			player.OnCutsceneStopped -= HandleStopped;
		}
	}
}
