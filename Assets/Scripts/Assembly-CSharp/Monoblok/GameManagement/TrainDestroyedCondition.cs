using Monoblok.SA;

namespace Monoblok.GameManagement
{
	public class TrainDestroyedCondition : WinOrLoseCondition
	{
		private readonly TrainRuntimeValue trainRuntimeValue;

		public TrainDestroyedCondition(TrainRuntimeValue trainRuntimeValue)
		{
			this.trainRuntimeValue = trainRuntimeValue;
		}

		public override void Initialize()
		{
			trainRuntimeValue.Value.Health.OnHealthZero += HandleTrainHealthZero;
		}

		public override void Dispose()
		{
			if (!(trainRuntimeValue.Value == null))
			{
				trainRuntimeValue.Value.Health.OnHealthZero -= HandleTrainHealthZero;
			}
		}

		private void HandleTrainHealthZero()
		{
			NotifyConditionMet();
		}
	}
}
