using UnityEngine;

namespace Monoblok.Movement
{
	public abstract class MovementSpeedProvider : MonoBehaviour
	{
		public abstract float GetSpeed();
	}
}
