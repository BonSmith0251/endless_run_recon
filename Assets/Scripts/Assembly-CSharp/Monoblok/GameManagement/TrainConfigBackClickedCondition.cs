using System;
using Monoblok.StateMachine;
using Monoblok.Train;

namespace Monoblok.GameManagement
{
	public class TrainConfigBackClickedCondition : ICondition
	{
		private readonly TrainConfigSelector selector;

		public event Action OnMet;

		public TrainConfigBackClickedCondition(TrainConfigSelector selector)
		{
			this.selector = selector;
		}

		public void Initialize()
		{
			selector.OnBackClicked += HandleBackClicked;
		}

		public void Dispose()
		{
			selector.OnBackClicked -= HandleBackClicked;
		}

		private void HandleBackClicked()
		{
			this.OnMet?.Invoke();
		}
	}
}
