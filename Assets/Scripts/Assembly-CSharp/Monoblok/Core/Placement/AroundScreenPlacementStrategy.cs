using UnityEngine;

namespace Monoblok.Core.Placement
{
	public class AroundScreenPlacementStrategy : IPlacementStrategy
	{
		public readonly Camera cam;

		public readonly float offset;

		public AroundScreenPlacementStrategy(Camera cam, float offset)
		{
			this.cam = cam;
			this.offset = offset;
		}

		public Vector2 GetPosition(Transform origin)
		{
			int num = Random.Range(0, 4);
			Vector3 zero = Vector3.zero;
			zero = num switch
			{
				0 => new Vector3(Random.value, 1f + offset, cam.nearClipPlane), 
				1 => new Vector3(Random.value, 0f - offset, cam.nearClipPlane), 
				2 => new Vector3(0f - offset, Random.value, cam.nearClipPlane), 
				_ => new Vector3(1f + offset, Random.value, cam.nearClipPlane), 
			};
			return cam.ViewportToWorldPoint(zero);
		}
	}
}
