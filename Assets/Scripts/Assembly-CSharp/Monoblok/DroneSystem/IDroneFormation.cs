using UnityEngine;

namespace Monoblok.DroneSystem
{
	public interface IDroneFormation
	{
		Vector3[] GetPositions(Vector3 origin, int count);
	}
}
