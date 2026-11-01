using UnityEngine;

namespace Monoblok.Core.Placement
{
	public interface IPlacementStrategy
	{
		Vector2 GetPosition(Transform origin);

		void VisualizeBounds(Transform origin)
		{
		}
	}
}
