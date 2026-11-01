using UnityEngine;

namespace Monoblok.Progression
{
	[CreateAssetMenu(menuName = "Game/Wave Settings/Timed Wave Settings")]
	public class TimedWaveSettings : ScriptableObject
	{
		[field: SerializeField]
		public float BaseDuration { get; private set; }

		[field: SerializeField]
		public float DurationIncrement { get; private set; }

		[field: SerializeField]
		public float MaxDuration { get; private set; }
	}
}
