using Monoblok.Train;
using UnityEngine;

namespace Monoblok.DamageSystem
{
	public class TrainCartDamageable : Damageable
	{
		[SerializeField]
		private TrainCart cart;

		private Damage.DamageType damageTypeMask = (Damage.DamageType)(-1);

		protected override (bool hitHealth, bool hitShield) HandleDamage(Damage dmg)
		{
			if (dmg.Type != 0 && (damageTypeMask & dmg.Type) == 0)
			{
				return (hitHealth: false, hitShield: false);
			}
			cart.Train.Health.Damage(dmg);
			return (hitHealth: true, hitShield: false);
		}

		public void SetDamageTypeMask(Damage.DamageType mask)
		{
			damageTypeMask = mask;
		}
	}
}
