using System;
using Monoblok.StateMachine;

namespace Monoblok.GameManagement
{
	public class EndlessModeBlend : ICompletable
	{
		private readonly EndlessModeTransition transition;

		public event Action OnComplete;

		public EndlessModeBlend(EndlessModeTransition transition)
		{
			this.transition = transition;
		}

		public void Initialize()
		{
			transition.OnTransitionComplete += HandleTransitionComplete;
			transition.StartTransition();
		}

		private void HandleTransitionComplete()
		{
			transition.OnTransitionComplete -= HandleTransitionComplete;
			this.OnComplete?.Invoke();
		}
	}
}
