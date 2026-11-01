using System;
using Monoblok.Progression;
using Monoblok.StateMachine;

namespace Monoblok.GameManagement
{
	public class JourneyEndedCondition : ICondition
	{
		private readonly Monoblok.Progression.Progression progression;

		public event Action OnMet;

		public JourneyEndedCondition(Monoblok.Progression.Progression progression)
		{
			this.progression = progression;
		}

		public void Dispose()
		{
			progression.OnJourneyEnded -= HandleJourneyEnded;
		}

		public void Initialize()
		{
			progression.OnJourneyEnded += HandleJourneyEnded;
		}

		private void HandleJourneyEnded()
		{
			this.OnMet?.Invoke();
		}
	}
}
