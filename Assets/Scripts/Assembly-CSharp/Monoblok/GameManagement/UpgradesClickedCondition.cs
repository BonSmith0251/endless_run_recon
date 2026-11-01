using System;
using Monoblok.StateMachine;
using Monoblok.Train;

namespace Monoblok.GameManagement
{
	public class UpgradesClickedCondition : ICondition
	{
		private readonly TrainConfigSelectorView trainConfigSelectorView;

		public event Action OnMet;

		public UpgradesClickedCondition(TrainConfigSelectorView trainConfigSelectorView)
		{
			this.trainConfigSelectorView = trainConfigSelectorView;
		}

		public void Initialize()
		{
			trainConfigSelectorView.OnUpgradesClicked += HandleButtonClick;
		}

		public void Dispose()
		{
			trainConfigSelectorView.OnUpgradesClicked -= HandleButtonClick;
		}

		private void HandleButtonClick()
		{
			this.OnMet?.Invoke();
		}
	}
}
