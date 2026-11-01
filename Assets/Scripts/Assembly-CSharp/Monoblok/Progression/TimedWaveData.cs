using UnityEngine;

namespace Monoblok.Progression
{
	[CreateAssetMenu(menuName = "Game/Progression/Timed Wave Data")]
	public class TimedWaveData : WaveData
	{
		private float duration;

		private float time;

		public float RemainingTime => Mathf.Max(0f, duration - time);

		public float NormalizedTime
		{
			get
			{
				if (!(duration > 0f))
				{
					return 0f;
				}
				return Mathf.Clamp01(time / duration);
			}
		}

		public void SetDuration(float duration)
		{
			this.duration = duration;
			time = 0f;
		}

		public void Tick(float deltaTime)
		{
			time += deltaTime;
		}
	}
}
