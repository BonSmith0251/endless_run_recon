using Monoblok.Movement;

namespace Monoblok.Weapon
{
	public class OriginTargetProvider : WeaponTargetProvider
	{
		private void Awake()
		{
			target = new TransformTarget(base.transform);
		}

		public override void OnRangeChanged(float newRange)
		{
		}
	}
}
