using UnityEngine;

namespace Monoblok.Weapon
{
	public class ForwardTargetProvider : WeaponTargetProvider
	{
		[SerializeField]
		private float minSpeed = 0.01f;

		private Vector3 previousPosition;

		private void Awake()
		{
			previousPosition = base.transform.position;
		}

		private void Update()
		{
			Vector3 vector = (base.transform.position - previousPosition) / Time.deltaTime;
			previousPosition = base.transform.position;
			if (vector.sqrMagnitude < minSpeed * minSpeed)
			{
				target = null;
			}
			else
			{
				target = new DirectionTarget(base.transform, vector.normalized);
			}
		}

		public override void OnRangeChanged(float newRange)
		{
		}
	}
}
