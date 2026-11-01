using System;
using Monoblok.StateMachine;
using Monoblok.Train;

namespace Monoblok.GameManagement
{
	public class TrainConfigSelectedCondition : ICondition
	{
		private readonly TrainConfigSelector selector;

		public event Action OnMet;

		public TrainConfigSelectedCondition(TrainConfigSelector selector)
		{
			this.selector = selector;
		}

		public void Initialize()
		{
			selector.OnSelectionComplete += HandleSelectionComplete;
		}

		public void Dispose()
		{
			selector.OnSelectionComplete -= HandleSelectionComplete;
		}

		private void HandleSelectionComplete()
		{
			this.OnMet?.Invoke();
		}
	}
}
