using UnityEngine;

namespace Monoblok.Core.Placement
{
	public class AroundBoundsPlacementStrategy : IPlacementStrategy
	{
		private readonly Vector2 size;

		private readonly float offset;

		public AroundBoundsPlacementStrategy(Vector2 size, float offset)
		{
			this.size = size;
			this.offset = offset;
		}

		public Vector2 GetPosition(Transform origin)
		{
			Vector2 vector = size * 0.5f;
			return Random.Range(0, 4) switch
			{
				0 => (Vector2)origin.position + new Vector2(Random.Range(0f - vector.x, vector.x), vector.y + offset), 
				1 => (Vector2)origin.position + new Vector2(Random.Range(0f - vector.x, vector.x), 0f - vector.y - offset), 
				2 => (Vector2)origin.position + new Vector2(0f - vector.x - offset, Random.Range(0f - vector.y, vector.y)), 
				_ => (Vector2)origin.position + new Vector2(vector.x + offset, Random.Range(0f - vector.y, vector.y)), 
			};
		}

		public void VisualizeBounds(Transform origin)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(origin.position, size);
			Gizmos.DrawWireCube(origin.position, size + new Vector2(offset, offset));
		}
	}
}
