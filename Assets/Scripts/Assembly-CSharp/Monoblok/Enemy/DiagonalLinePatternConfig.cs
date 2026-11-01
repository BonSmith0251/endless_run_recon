using Monoblok.Isometric;
using UnityEngine;

namespace Monoblok.Enemy
{
	[CreateAssetMenu(menuName = "Game/Enemy/Spawn Patterns/Diagonal Line")]
	public class DiagonalLinePatternConfig : SpawnPatternConfig
	{
		[SerializeField]
		private float spacing = 2f;

		[SerializeField]
		private float delayBetween = 0.15f;

		protected override SpawnPoint[] GetSpawnPoints(Vector3 anchor, int count)
		{
			SpawnPoint[] array = new SpawnPoint[count];
			Vector2 left = Iso.Left;
			Vector2 right = Iso.Right;
			float num = spacing * (float)(count - 1);
			for (int i = 0; i < count; i++)
			{
				float t = ((count > 1) ? ((float)i / (float)(count - 1)) : 0f);
				Vector3 vector = Vector2.Lerp(left, right, t) * (num * 0.5f);
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
