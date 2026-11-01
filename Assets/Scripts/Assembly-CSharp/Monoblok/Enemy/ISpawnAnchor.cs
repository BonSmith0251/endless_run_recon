using UnityEngine;

namespace Monoblok.Enemy
{
	public interface ISpawnAnchor
	{
		Vector3 GetAnchor(Camera cam, Transform origin);
	}
}
