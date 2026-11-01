using UnityEngine;

namespace Monoblok.Weapon
{
	public class DirectionTargetProvider : WeaponTargetProvider
	{
		[SerializeField]
		private Vector2 direction;

		private void OnValidate()
		{
			direction.x = Mathf.Clamp01(direction.x);
			direction.y = Mathf.Clamp01(direction.y);
		}

		private void Awake()
		{
			target = new DirectionTarget(base.transform, direction);
		}

		public void SetDirection(Vector2 direction)
		{
			this.direction = direction.normalized;
			target = new DirectionTarget(base.transform, this.direction);
		}

		public override void OnRangeChanged(float newRange)
		{
		}
	}
}
