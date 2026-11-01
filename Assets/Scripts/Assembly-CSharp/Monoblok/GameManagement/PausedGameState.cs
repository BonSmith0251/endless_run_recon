using FMODUnity;
using Monoblok.StateMachine;
using PrimeTween;
using UnityEngine;

namespace Monoblok.GameManagement
{
	public class PausedGameState : IState
	{
		private readonly MainMenu mainMenu;

		private const string PauseParameterName = "Paused";

		public PausedGameState(MainMenu mainMenu)
		{
			this.mainMenu = mainMenu;
		}

		public void Enter()
		{
			Time.timeScale = 0f;
			RuntimeManager.StudioSystem.getParameterByName("Paused", out var value);
			Tween.Custom(this, value, 1f, 0.3f, delegate(PausedGameState t, float v)
			{
				t.UpdatePausedParameter(v);
			}, default(Easing), 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
			mainMenu.SetState(MainMenu.MainMenuState.Paused);
			mainMenu.Show();
		}

		public void Update(float deltaTime)
		{
		}

		public void Exit()
		{
			Time.timeScale = 1f;
			RuntimeManager.StudioSystem.getParameterByName("Paused", out var value);
			Tween.Custom(this, value, 0f, 0.3f, delegate(PausedGameState t, float v)
			{
				t.UpdatePausedParameter(v);
			}, default(Easing), 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
			mainMenu.Hide();
		}

		private void UpdatePausedParameter(float value)
		{
			RuntimeManager.StudioSystem.setParameterByName("Paused", value);
		}
	}
}
