using UnityEngine;

namespace Monoblok.Movement
{
	public abstract class MovementBase : MonoBehaviour, IMove
	{
		public abstract void SetDestination(ITarget target);

		public abstract void Stop();
	}
}
