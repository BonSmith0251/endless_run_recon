using UnityEngine;

namespace Monoblok.Enemy
{
	[CreateAssetMenu(menuName = "Game/Enemy/Spawn Patterns/Anchor")]
	public class AnchorPatternConfig : SpawnPatternConfig
	{
		[SerializeField]
		private float delayBetween;

		protected override SpawnPoint[] GetSpawnPoints(Vector3 anchor, int count)
		{
			SpawnPoint[] array = new SpawnPoint[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = new SpawnPoint
				{
					position = anchor,
					delay = ((i == 0) ? 0f : delayBetween)
				};
			}
			return array;
		}
	}
}
