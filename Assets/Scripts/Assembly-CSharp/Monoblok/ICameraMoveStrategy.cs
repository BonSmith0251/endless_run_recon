using UnityEngine;

namespace Monoblok
{
	public interface ICameraMoveStrategy
	{
		Vector3? GetDesiredPosition(Vector3 currentPosition);
	}
}
