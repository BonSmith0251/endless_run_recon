using Monoblok.Movement;
using UnityEngine;

namespace Monoblok.Weapon
{
	public abstract class WeaponTargetProvider : MonoBehaviour
	{
		protected ITarget target;

		public ITarget Target => target;

		public abstract void OnRangeChanged(float newRange);
	}
}
