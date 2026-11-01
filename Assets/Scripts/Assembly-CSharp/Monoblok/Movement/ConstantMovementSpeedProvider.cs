using UnityEngine;

namespace Monoblok.Movement
{
	public class ConstantMovementSpeedProvider : MovementSpeedProvider
	{
		[SerializeField]
		private float speed = 5f;

		public override float GetSpeed()
		{
			return speed;
		}
	}
}
