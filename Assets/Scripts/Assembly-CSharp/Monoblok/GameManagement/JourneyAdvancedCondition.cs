using System;
using Monoblok.StateMachine;

namespace Monoblok.GameManagement
{
	public class JourneyAdvancedCondition : ICondition
	{
		private readonly JourneyAdvancer advancer;

		public event Action OnMet;

		public JourneyAdvancedCondition(JourneyAdvancer advancer)
		{
			this.advancer = advancer;
		}

		public void Dispose()
		{
			advancer.OnAdvanceClicked -= HandleAdvanceClicked;
		}

		public void Initialize()
		{
			advancer.OnAdvanceClicked += HandleAdvanceClicked;
		}

		private void HandleAdvanceClicked()
		{
			this.OnMet?.Invoke();
		}
	}
}
