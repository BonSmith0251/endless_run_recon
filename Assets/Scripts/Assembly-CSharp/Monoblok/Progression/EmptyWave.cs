namespace Monoblok.Progression
{
	public class EmptyWave : Wave
	{
		public EmptyWave(int waveIndex, WaveData data)
			: base(waveIndex, data)
		{
		}

		protected override void OnStartWave()
		{
		}

		protected override void OnUpdateWave(float deltaTime)
		{
		}
	}
}
