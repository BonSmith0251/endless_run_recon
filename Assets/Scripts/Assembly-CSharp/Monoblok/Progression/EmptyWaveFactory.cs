using System;

namespace Monoblok.Progression
{
	[Serializable]
	public class EmptyWaveFactory : IWaveFactory
	{
		public Wave GetWave(int waveIndex)
		{
			return new EmptyWave(waveIndex, null);
		}
	}
}
