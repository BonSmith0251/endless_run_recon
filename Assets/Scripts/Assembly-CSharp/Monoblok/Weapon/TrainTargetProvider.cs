using Monoblok.Core;
using Monoblok.Movement;
using Monoblok.SA;
using UnityEngine;

namespace Monoblok.Weapon
{
	public class TrainTargetProvider : WeaponTargetProvider
	{
		[SerializeField]
		private TrainRuntimeValue trainValue;

		private void OnEnable()
		{
			target = new TransformTarget(trainValue.Value.Carts.GetRandom().transform);
		}

		public override void OnRangeChanged(float newRange)
		{
		}
	}
}
