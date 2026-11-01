using UnityEngine;

namespace Monoblok.Enemy
{
	[CreateAssetMenu(menuName = "Game/Enemy/Spawn Patterns/Horizontal Line")]
	public class HorizontalLinePatternConfig : SpawnPatternConfig
	{
		[SerializeField]
		private float spacing = 2f;

		[SerializeField]
		private float delayBetween = 0.15f;

		protected override SpawnPoint[] GetSpawnPoints(Vector3 anchor, int count)
		{
			SpawnPoint[] array = new SpawnPoint[count];
			float num = spacing * (float)(count - 1);
			for (int i = 0; i < count; i++)
			{
				float num2 = ((count > 1) ? ((float)i / (float)(count - 1)) : 0f);
				Vector3 vector = Vector3.right * (num2 * num - num * 0.5f);
				array[i] = new SpawnPoint
				{
					position = anchor + vector,
					delay = ((i == 0) ? 0f : delayBetween)
				};
			}
			return array;
		}
	}
}
