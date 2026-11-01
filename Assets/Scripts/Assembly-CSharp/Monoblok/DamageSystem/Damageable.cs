using System;
using UnityEngine;

namespace Monoblok.DamageSystem
{
	public abstract class Damageable : MonoBehaviour, IDamageable
	{
		public event Action<Damage> OnDamaged;

		public event Action<Damage> OnHealthDamaged;

		public event Action<Damage> OnShieldDamaged;

		public void Damage(Damage dmg)
		{
			var (flag, flag2) = HandleDamage(dmg);
			if (flag || flag2)
			{
				this.OnDamaged?.Invoke(dmg);
				if (flag2)
				{
					this.OnShieldDamaged?.Invoke(dmg);
				}
				if (flag)
				{
					this.OnHealthDamaged?.Invoke(dmg);
				}
			}
		}

		protected abstract (bool hitHealth, bool hitShield) HandleDamage(Damage dmg);
	}
}
