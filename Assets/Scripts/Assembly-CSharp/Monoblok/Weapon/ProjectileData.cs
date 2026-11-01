using UnityEngine;

namespace Monoblok.Weapon
{
	public readonly struct ProjectileData
	{
		public readonly Vector3 Origin;

		public readonly Vector3 Direction;

		public readonly float Speed;

		public readonly float Range;

		public readonly int Piercing;

		public readonly int Layer;

		public ProjectileData(Vector3 origin, Vector3 direction, float speed, float range, int piercing, int layer)
		{
			Origin = origin;
			Direction = direction;
			Speed = speed;
			Range = range;
			Piercing = piercing;
			Layer = layer;
		}
	}
}
