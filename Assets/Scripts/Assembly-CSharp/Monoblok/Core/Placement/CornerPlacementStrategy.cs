using Monoblok.Isometric;
using UnityEngine;

namespace Monoblok.Core.Placement
{
	public class CornerPlacementStrategy : IPlacementStrategy
	{
		private readonly float radius;

		private readonly float spread;

		private readonly Camera cam;

		private readonly float padding;

		public CornerPlacementStrategy(float radius, float spread)
		{
			this.radius = radius;
			this.spread = spread;
		}

		public CornerPlacementStrategy(Camera cam, float padding, float spread)
		{
			this.cam = cam;
			this.padding = padding;
			this.spread = spread;
		}

		public Vector2 GetPosition(Transform origin)
		{
			GetSpawnPoints(origin, out var topSpawn, out var bottomSpawn);
			return ((Random.Range(0, 2) == 0) ? topSpawn : bottomSpawn) + (Vector3)Iso.Left * Random.Range(0f - spread, spread);
		}

		public void VisualizeBounds(Transform origin)
		{
			GetSpawnPoints(origin, out var topSpawn, out var bottomSpawn);
			Gizmos.color = Color.yellow;
			DrawSpawnLine(topSpawn);
			DrawSpawnLine(bottomSpawn);
			Gizmos.color = new Color(1f, 1f, 0f, 0.15f);
			Gizmos.DrawSphere(topSpawn, 0.25f);
			Gizmos.DrawSphere(bottomSpawn, 0.25f);
		}

		private void GetSpawnPoints(Transform origin, out Vector3 topSpawn, out Vector3 bottomSpawn)
		{
			if (cam != null)
			{
				float orthographicSize = cam.orthographicSize;
				float num = orthographicSize * cam.aspect;
				Vector3 position = cam.transform.position;
				topSpawn = position + new Vector3(num, orthographicSize, 0f) + (Vector3)Iso.Up * padding;
				bottomSpawn = position + new Vector3(0f - num, 0f - orthographicSize, 0f) + (Vector3)Iso.Down * padding;
			}
			else
			{
				topSpawn = origin.position + (Vector3)Iso.Up * radius;
				bottomSpawn = origin.position + (Vector3)Iso.Down * radius;
			}
		}

		private void DrawSpawnLine(Vector3 center)
		{
			Gizmos.DrawLine(center + (Vector3)Iso.Left * (0f - spread), center + (Vector3)Iso.Left * spread);
		}
	}
}
