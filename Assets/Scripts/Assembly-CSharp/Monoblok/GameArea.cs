using UnityEngine;

namespace Monoblok
{
	[CreateAssetMenu(menuName = "Game/Game Area")]
	public class GameArea : ScriptableObject
	{
		[SerializeField]
		private Vector3 center;

		[SerializeField]
		private Vector3 size;

		public Bounds Bounds => new Bounds(center, size);

		public Vector3 GetRandomPosition()
		{
			float x = Random.Range(Bounds.min.x, Bounds.max.x);
			float y = Random.Range(Bounds.min.y, Bounds.max.y);
			return new Vector3(x, y);
		}
	}
}
