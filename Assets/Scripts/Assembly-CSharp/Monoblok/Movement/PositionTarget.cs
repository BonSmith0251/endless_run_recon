using UnityEngine;

namespace Monoblok.Movement
{
	public class PositionTarget : ITarget
	{
		private Vector3 position;

		public Vector3 Position => position;

		public PositionTarget(Vector3 position)
		{
			this.position = position;
		}

		public void UpdateTarget(Vector3 pos)
		{
			position = pos;
		}
	}
}
