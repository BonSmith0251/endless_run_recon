using Monoblok.Progression;

namespace Monoblok.GameManagement
{
	public class WaveReachedCondition : WinOrLoseCondition
	{
		private readonly Monoblok.Progression.Progression progression;

		private readonly int targetWave;

		public WaveReachedCondition(Monoblok.Progression.Progression progression, int targetWave)
		{
			this.progression = progression;
			this.targetWave = targetWave;
		}

		public override void Initialize()
		{
			progression.OnJourneyEnded += HandleJourneyEnded;
		}

		private void HandleJourneyEnded()
		{
			if (progression.CurrentWaveIndex >= targetWave)
			{
				NotifyConditionMet();
			}
		}

		public override void Dispose()
		{
			progression.OnJourneyEnded -= HandleJourneyEnded;
		}
	}
}
