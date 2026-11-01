using System;
using UnityEngine;

namespace Monoblok.DamageSystem
{
	public abstract class DodgeableDamageable : Damageable
	{
		[SerializeField]
		private DodgeDamage dodge;

		public event Action<Damage> OnDodged;

		public new void Damage(Damage dmg)
		{
			if (dodge != null && dodge.Dodge())
			{
				this.OnDodged?.Invoke(dmg);
				Debug.Log("DODGED");
			}
			else
			{
				base.Damage(dmg);
			}
		}
	}
}
