namespace Monoblok.Progression
{
	public class TimedWave : Wave<TimedWaveData>
	{
		private readonly float duration;

		public TimedWave(float duration, TimedWaveData data, int waveIndex)
			: base(waveIndex, data)
		{
			this.duration = duration;
		}

		protected override void OnStartWave()
		{
			base.Data.SetDuration(duration);
		}

		protected override void OnUpdateWave(float deltaTime)
		{
			base.Data.Tick(deltaTime);
			if (!(base.Data.RemainingTime > 0f))
			{
				CompleteWave();
			}
		}
	}
}
