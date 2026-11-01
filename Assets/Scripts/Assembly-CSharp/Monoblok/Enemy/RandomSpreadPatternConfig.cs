using UnityEngine;

namespace Monoblok.Enemy
{
	[CreateAssetMenu(menuName = "Game/Enemy/Spawn Patterns/Random Spread")]
	public class RandomSpreadPatternConfig : SpawnPatternConfig
	{
		[SerializeField]
		private float spreadRadius = 3f;

		[SerializeField]
		private float delayBetween = 0.1f;

		protected override SpawnPoint[] GetSpawnPoints(Vector3 anchor, int count)
		{
			SpawnPoint[] array = new SpawnPoint[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = new SpawnPoint
				{
					position = anchor + (Vector3)(Random.insideUnitCircle * spreadRadius),
					delay = ((i == 0) ? 0f : delayBetween)
				};
			}
			return array;
		}
	}
}
