using System;
using Monoblok.StateMachine;

namespace Monoblok.GameManagement
{
	public class ThanksForPlayingDismissed : ICondition
	{
		private readonly ThanksForPlayingScreen thxScreen;

		public event Action OnMet;

		public ThanksForPlayingDismissed(ThanksForPlayingScreen thxScreen)
		{
			this.thxScreen = thxScreen;
		}

		public void Initialize()
		{
			thxScreen.OnContinueClicked += HandleContinueClicked;
		}

		public void Dispose()
		{
			thxScreen.OnContinueClicked += HandleContinueClicked;
		}

		private void HandleContinueClicked()
		{
			this.OnMet?.Invoke();
		}
	}
}
