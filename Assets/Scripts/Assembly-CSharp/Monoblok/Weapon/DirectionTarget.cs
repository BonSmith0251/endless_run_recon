using Monoblok.Movement;
using UnityEngine;

namespace Monoblok.Weapon
{
	public class DirectionTarget : ITarget
	{
		private readonly Transform origin;

		private readonly Vector3 direction;

		public Vector3 Position => origin.position + direction;

		public DirectionTarget(Transform origin, Vector2 direction)
		{
			this.origin = origin;
			this.direction = direction;
		}
	}
}
