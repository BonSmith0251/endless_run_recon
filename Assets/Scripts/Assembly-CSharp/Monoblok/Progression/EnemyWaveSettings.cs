using UnityEngine;

namespace Monoblok.Progression
{
	[CreateAssetMenu(menuName = "Game/Wave Settings/Enemy Wave Settings")]
	public class EnemyWaveSettings : ScriptableObject
	{
		[field: SerializeField]
		public float BaseSpawnRate { get; private set; }

		[field: SerializeField]
		public float SpawnRateMultiplier { get; private set; }
	}
}
